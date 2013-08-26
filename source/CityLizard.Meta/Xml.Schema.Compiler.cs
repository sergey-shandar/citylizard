namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using C = System.Collections.Generic;
    using CD = System.CodeDom;
    using S = System;
    using CDC = System.CodeDom.Compiler;
    using A = System.CodeDom.MemberAttributes;
    using IO = System.IO;
    using MCS = Microsoft.CSharp;

    using CS = CodeDom.CSharp;
    using D = CodeDom.Code;
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

        private static Fsm.Name Start = new Fsm.Name { 0 };

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

        private void AddProperty(
            T.Type type,
            T.TypeRef result,
            T.Parameter parameter,
            T.Get get)            
        {
            type.Add(
                Property("Item", result, A.Public)
                    [parameter]
                    [get]);
        }

        private void AddProperty(
            T.Type type, 
            T.TypeRef result, 
            T.TypeRef parameter, 
            string parameterName)
        {
            var p = Parameter(parameter, parameterName);
            this.AddProperty(
                type,
                result,
                p,
                Get()
                    [Invoke("Add")[p.Ref()]]
                    [Return(This())]);
        }

        /// <summary>
        /// A -X-> A: A.P |= X[1; +Inf)
        /// A -X-> B: B.P |= A.P + X
        /// A -X-> ... B -Y-> A: A |=
        ///
        /// </summary>
        /// <param name="newToDo"></param>
        /// <param name="element"></param>
        /// <param name="isRoot"></param>
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

            // Constructor body.
            var mainSetUp = Invoke("SetUp")
                [implementation.Ref()]
                [Primitive(qName.Namespace)]
                [Primitive(qName.Name)];
            // Main constructor.
            var mainCtor = 
                Constructor(Attributes: A.Assembly)[implementation][mainSetUp];

            // simple type
            if(complexType == null)
            {
                // base type
                csType.Add(TypeRef<E.Simple>());

                var parameter = Parameter(TypeRef<string>(), "Value");
                mainCtor.Add(parameter);
                mainSetUp.Add(parameter.Ref());
                csType.Add(mainCtor);

                var typeRef = TypeRef("T." + name);
                // Method.
                x.Add(
                    Method
                        (Name: name + "_",
                            Attributes: A.Public,
                            Return: typeRef
                        )
                        [parameter]
                        [Return
                            (New(typeRef)
                                [This()]
                                [parameter.Ref()]
                            )
                        ]);
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

                        string parameterName;
                        string parameterTypeName;

                        // Any element.
                        if (s.Key.Name == "")
                        {
                            parameterName = "Element";
                            parameterTypeName = "CityLizard.Xml.Linked.Element.Element";
                        }
                        else
                        {
                            parameterName = CS.Name.Cast(s.Key.Name);
                            parameterTypeName = "T." + parameterName;
                        }

                        var parameter = Parameter(
                            TypeRef(parameterTypeName), parameterName);
                        var parameterRef = parameter.Ref();

                        //
                        if (comparer.Equals(p.Key, s.Value))
                        {
                            pType.Add(
                                Method("Add", A.Public)
                                    [parameter]
                                    [Invoke("AddLinkedNodeOptional")
                                        [parameterRef]
                                    ]);
                            this.AddProperty(
                                pType,
                                returnTypeRef,
                                TypeRef(parameterTypeName),
                                parameterName);
                            var listTypeRef = TypeRef(
                                "System.Collections.Generic.IEnumerable<" + 
                                parameterTypeName +
                                ">");
                            var listParameter = Parameter(
                                listTypeRef, parameterName);
                            pType.Add(
                                Method("Add", A.Public)
                                    [listParameter]
                                    [Invoke("AddElementList")
                                        [listParameter.Ref()]
                                    ]);
                            this.AddProperty(
                                pType, 
                                returnTypeRef, 
                                listTypeRef, 
                                parameterName);
                        }
                        else
                        {
                            this.AddProperty(
                                pType,
                                returnTypeRef,
                                parameter,
                                Get()
                                    [Return
                                            (New(returnTypeRef)
                                                [This()]
                                                [parameterRef]
                                            )
                                    ]);
                        }
                    }

                    // Comments.
                    if (elementType != E.Type.Empty)
                    {
                        this.AddProperty(
                            pType, 
                            TypeRef(pName), 
                            TypeRef<Linked.Comment>(), 
                            "Comment");
                    }

                    // Text.
                    if (elementType == E.Type.Mixed)
                    {
                        this.AddProperty(
                            pType, TypeRef(pName), TypeRef<string>(), "Text");
                        this.AddProperty(
                            pType, 
                            TypeRef(pName), 
                            TypeRef<Linked.Text>(), 
                            "Text");
                    }

                    // If start element.
                    // Exactly one start exsists in DFA.
                    if (comparer.Equals(p.Key, Start))
                    {
                        // Constructor.
                        mainCtor.Add(attributes.Parameters);
                        // Constructor body.
                        mainCtor.Add(attributes.Invokes);
                        //
                        pType.Add(mainCtor);

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
                        pType.Add(
                            Constructor(Attributes: A.Assembly)
                                [part0]
                                [elementP]
                                [Invoke("SetUp")[part0.Ref()][elementP.Ref()]]);
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

        public static string Compile(string fileName)
        {
            var unit = new Compiler().Load(fileName);
            var t = new IO.StringWriter();
            new MCS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                unit, t, new CDC.CodeGeneratorOptions());
            return t.ToString();
        }
    }
}
