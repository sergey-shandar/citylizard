namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using C = System.Collections.Generic;
    using CD = System.CodeDom;
    using S = System;
    using A = System.CodeDom.MemberAttributes;

    using CS = CodeDom.CSharp;
    using D = CodeDom.CodeDom;
    using E = Xml.Linked.Element;
    using F = Fsm;

    using Extension;
    using System.Linq;

    public class Compiler: D
    {
        T.Unit U;
        ElementSet Done;
        C.IEnumerable<XS.XmlSchemaElement> ToDo;

        private class Attributes
        {
            public readonly C.List<T.Parameter> Parameters = 
                new C.List<T.Parameter>();
            public readonly C.List<T.Invoke> Invokes = new C.List<T.Invoke>();
            public C.IEnumerable<XS.XmlSchemaAttribute> A;
        }

        private bool AddAttributes(Attributes attributes, bool required)
        {
            var result = false;
            foreach (
                var a in
                attributes.A.Where(
                    x =>
                        x.QualifiedName.Namespace == "" &&
                        (x.Use == XS.XmlSchemaUse.Required) == required))
            {
                var name = a.QualifiedName.Name;
                var p = Parameter(
                    TypeRef<string>(),
                    CS.Name.Cast(name),
                    required ? null : Primitive(null));
                attributes.Parameters.Add(p);
                attributes.Invokes.Add(
                    Invoke(
                        required ?
                            "AddRequiredAttribute" : "AddOptionalAttribute")
                        [Primitive(name)]
                        [p.Ref()]);
                result = true;
            }
            return result;
        }

        private static readonly C.IEqualityComparer<C.HashSet<int>> comparer =
            C.HashSet<int>.CreateSetComparer();

        private static C.HashSet<int> Start = new C.HashSet<int> { 0 };

        private static string Name(C.HashSet<int> s)
        {
            var r = "";
            foreach (var i in s)
            {
                r += "_" + i;
            }
            return r;
        }

        private static string Name(
            C.HashSet<int> self, string selfName, C.HashSet<int> key)
        {
            return comparer.Equals(key, self) ? selfName : Name(key);
        }

        private void SetType(
            ElementSet newToDo,
            XS.XmlSchemaElement element,
            bool isRoot = false)
        {
            var qName = element.QualifiedName;
            var type = element.ElementSchemaType;

            var complexType = type as XS.XmlSchemaComplexType;

            var attributes = new Attributes();

            var name = CS.Name.Cast(qName.Name);
            var implementation = 
                Parameter(TypeRef<Xml.Implementation>(), "Implementation");

            var csType = Type(Name: name, Attributes: A.Public);
            //
            var x = 
                Type
                    (Name: "X",
                        IsPartial: true,
                        Attributes: A.Public
                    )
                    [TypeRef<Xml.Implementation>()]
                    [Type
                        (   Name: "T",
                            IsPartial: true,
                            Attributes: A.Static | A.Public
                        )
                        [csType]
                    ];
            this.U.Add(Namespace(CS.Namespace.Cast(qName.Namespace))[x]);

            // simple type
            if(complexType == null)
            {
                csType.Add(TypeRef<E.Simple>());
            }
            // complex type
            else
            {
                // Attributes.
                attributes.A = complexType.AttributeUsesTyped();
                var attributesRequired = this.AddAttributes(attributes, true);
                this.AddAttributes(attributes, false);

                // DFA
                var dfa = new ComplexTypeToDfa(newToDo).Apply(complexType);

                // At least one such element has to exsist.
                var self = dfa.D.First(p => p.Value.Accept).Key;

                // If no transitions from start then element is empty.
                // Exactly one start exsist in DFA.
                var isEmpty = dfa.D[Start].Count == 0;

                var elementType =
                    complexType.IsMixed ?
                        E.Type.Mixed :
                    isEmpty ?
                        E.Type.Empty :
                    // else
                        E.Type.NotMixed;

                // Base type. One of Mixed, Empty, NotMixed.
                var baseTypeRef = 
                    elementType == E.Type.Mixed ?
                        TypeRef<E.Mixed>() :
                    elementType == E.Type.Empty ?
                        TypeRef<E.Empty>() :
                    // else
                        TypeRef<E.NotMixed>();

                //
                csType.Add(baseTypeRef);

                //
                foreach (var p in dfa.D)
                {
                    T.Type pType;
                    string pName;

                    //
                    if (comparer.Equals(p.Key, self))
                    {
                        pType = csType;
                        pName = name;

                        if (elementType != E.Type.Empty)
                        {
                            var part0 = Parameter(baseTypeRef, "Part0");
                            pType.Add(
                                Constructor(Attributes: A.Assembly)
                                    [part0]
                                    [Invoke("SetUp")[part0.Ref()]]);
                        }
                    }
                    else
                    {
                        var local = Name(p.Key);
                        pType = Type(
                            Name: local, Attributes: A.Public);
                        pType.Add(baseTypeRef);
                        csType.Add(pType);
                        pName = name + "." + local;

                        // Implicit cast.
                        if (p.Value.Accept)
                        {
                            var typeRef = TypeRef(name);
                            var parameter = Parameter(TypeRef(local), local);
                            pType.Add(
                                Method(typeRef, A.Public)
                                    [parameter]
                                    [Return
                                        (New(typeRef)[parameter.Ref()])
                                    ]);
                        }
                    }

                    //
                    foreach (var s in p.Value)
                    {                
                        var returnTypeName = Name(self, name, s.Value);
                        var returnTypeRef = TypeRef(returnTypeName);
                        var parameterName = CS.Name.Cast(s.Key.Name);
                        var parameter = 
                            Parameter(TypeRef(parameterName), parameterName);
                        var parameterRef = parameter.Ref();
                        var get = Get();

                        //
                        if (comparer.Equals(p.Key, s.Value))
                        {
                            pType.Add(
                                Method("Add", A.Public)
                                    [parameter]
                                    [Invoke("AddLinkedNodeOptional")
                                        [parameterRef]
                                    ]);
                            get.Add(Invoke("Add")[parameterRef]);
                            get.Add(Return(This()));
                        }
                        else
                        {
                            get.Add(
                                Return
                                    (New(returnTypeRef)
                                        [This()]
                                        [parameterRef]
                                    ));
                        }
                        pType.Add(
                            Property("Item", returnTypeRef, A.Public)
                                [parameter]
                                [get]);
                    }

                    var ctor = Constructor(Attributes: A.Assembly);
                    pType.Add(ctor);

                    // Comments.
                    if (elementType != E.Type.Empty)
                    {
                        var parameter = Parameter(TypeRef<Linked.Comment>(), "Comment");
                        pType.Add(
                            Property("Item", TypeRef(pName), A.Public)
                                [parameter]
                                [Get()
                                    [Invoke("Add")[parameter.Ref()]]
                                    [Return(This())]
                                ]);
                    }

                    // Text.
                    if (elementType == E.Type.Mixed)
                    {
                        {
                            var parameter = Parameter(TypeRef<string>(), "Text");
                            pType.Add(
                                Property("Item", TypeRef(pName), A.Public)
                                    [parameter]
                                    [Get()
                                        [Invoke("Add")[parameter.Ref()]]
                                        [Return(This())]
                                    ]);
                        }
                        //
                        {
                            var parameter = Parameter(TypeRef<Linked.Text>(), "Text");
                            pType.Add(
                                Property("Item", TypeRef(pName), A.Public)
                                    [parameter]
                                    [Get()
                                        [Invoke("Add")[parameter.Ref()]]
                                        [Return(This())]
                                    ]);
                        }
                    }

                    // If start element.
                    // Exactly one start exsists in DFA.
                    if (comparer.Equals(p.Key, Start))
                    {
                        // Constructor.
                        ctor.Add(implementation);
                        ctor.Add(attributes.Parameters);
                        // Constructor body.
                        ctor.Add(
                            Invoke("SetUp")
                                [implementation.Ref()]
                                [Primitive(qName.Namespace)]
                                [Primitive(qName.Name)]);
                        ctor.Add(attributes.Invokes);

                        var typeRef = TypeRef("T." + pName);

                        // Method.
                        x.Add(
                            Method
                                (   Name: name + "_",
                                    Attributes: A.Public,
                                    Return: typeRef
                                )
                                [attributes.Parameters]
                                [Return
                                    (New(typeRef)
                                        [This()]
                                        [attributes.Parameters.
                                            Select(y => y.Ref())
                                        ]
                                    )
                                ]);

                        // Property
                        if (!attributesRequired)
                        {
                            x.Add(
                                Property
                                    (   Name: name, 
                                        Type: typeRef, 
                                        Attributes: A.Public
                                    )
                                    [Get()
                                        [Return(New(typeRef)[This()])]
                                    ]);
                        }
                    }
                    else
                    {
                        var part0 = Parameter(baseTypeRef, "Part0");
                        var elementP = Parameter(
                            TypeRef<Linked.Element.Element>(), "Element");
                        // Constructor parameters.
                        ctor.Add(part0);
                        ctor.Add(elementP);
                        // Constructor body.
                        ctor.Add(
                            Invoke("SetUp")[part0.Ref()][elementP.Ref()]);
                    }
                }
                //
            }
        }

        private bool AddElementSet()
        {
            var newToDo = new ElementSet();
            foreach (var x in this.ToDo)
            {
                this.Done.Add(x);
                this.SetType(newToDo, x);
            }
            newToDo.ExceptWith(this.Done);
            this.ToDo = newToDo;
            return newToDo.Count != 0;
        }

        public T.Unit Load(X.XmlReader reader)
        {
            var schema = new XS.XmlSchemaSet();
            schema.Add(null, reader);
            schema.Compile();
            //
            this.U = Unit();
            //
            this.ToDo = schema.GlobalElementsTyped();
            this.Done = new ElementSet();
            while (this.AddElementSet()) { }
            //
            return this.U;
        }

        public T.Unit Load(string fileName)
        {
            using(var xmlReader = new X.XmlTextReader(fileName))
            {
                return this.Load(xmlReader);
            }
        }
    }
}
