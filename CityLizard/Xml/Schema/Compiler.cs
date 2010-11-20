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

        private void AddAttributes(Attributes attributes, bool required)
        {
            foreach (
                var a in
                attributes.A.Where(
                    x => (x.Use == XS.XmlSchemaUse.Required) == required))
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
            }
        }

        private static readonly C.IEqualityComparer<C.HashSet<int>> comparer =
            C.HashSet<int>.CreateSetComparer();

        private void AddBase(
            T.Type type, XS.XmlSchemaComplexType complexType, bool isEmpty)
        {
            type.Append(
                complexType.IsMixed ?
                    TypeRef<E.Mixed>() :
                isEmpty ?
                    TypeRef<E.Empty>() :
                // else
                    TypeRef<E.NotMixed>());
        }

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
            this.U.Append(Namespace(CS.Namespace.Cast(qName.Namespace))[x]);

            // simple type
            if(complexType == null)
            {
                csType.Append(TypeRef<E.Simple>());
            }
            // complex type
            else
            {
                // Attributes.
                attributes.A = complexType.AttributeUsesTyped();
                this.AddAttributes(attributes, true);
                this.AddAttributes(attributes, false);

                // DFA
                var dfa = new ComplexTypeToDfa(newToDo).Apply(complexType);

                // At least one such element has to exsist.
                var self = dfa.D.First(p => p.Value.Accept).Key;

                // If no transitions from start then element is empty.
                // Exactly one start exsist in DFA.
                var isEmpty = dfa.D[Start].Count == 0;

                //
                this.AddBase(csType, complexType, isEmpty);

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
                    }
                    else
                    {
                        var local = Name(p.Key);
                        pType = Type(
                            Name: local, Attributes: A.Public);
                        this.AddBase(pType, complexType, isEmpty);
                        csType.Append(pType);
                        pName = name + "." + local;
                    }

                    //
                    foreach (var s in p.Value)
                    {                        
                        var returnTypeRef = TypeRef(Name(self, name, s.Value));
                        pType.Append(
                            Property("Item", returnTypeRef, A.Public)
                                );
                    }

                    // If start element.
                    // Exactly one start exsists in DFA.
                    if (comparer.Equals(p.Key, Start))
                    {
                        // Constructor.
                        pType.Append(
                            Constructor(Attributes: A.Assembly)
                                [implementation]
                                [attributes.Parameters]
                                [Invoke("SetUpNew")
                                    [implementation.Ref()]
                                    [Primitive(qName.Namespace)]
                                    [Primitive(qName.Name)]
                                ]
                                [attributes.Invokes]);

                        var typeRef = TypeRef("T." + pName);

                        // Method.
                        x.Append(
                            Method
                                (   Name: name,
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
