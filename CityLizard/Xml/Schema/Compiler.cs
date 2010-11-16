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
            public C.List<T.Parameter> Parameters = new C.List<T.Parameter>();
            public C.IEnumerable<XS.XmlSchemaAttribute> A;
        }

        private void AddAttributes(Attributes attributes, bool required)
        {
            foreach (
                var a in
                attributes.A.Where(
                    x => (x.Use == XS.XmlSchemaUse.Required) == required))
            {
                attributes.Parameters.Add(Parameter<string>(
                    CS.Name.Cast(a.QualifiedName.Name),
                    required ? null: Primitive(null)));
            }
        }

        private void SetType(
            ElementSet newToDo,
            XS.XmlSchemaElement element,
            bool isRoot = false)
        {
            var qName = element.QualifiedName;
            var type = element.ElementSchemaType;

            var complexType = type as XS.XmlSchemaComplexType;
            T.TypeRef baseTypeRef;

            var attributes = new Attributes();

            // simple type
            if(complexType == null)
            {
                baseTypeRef = TypeRef<E.Simple>();
            }
            // complex type
            else
            {
                var dfa = new ComplexTypeToDfa(newToDo).Apply(complexType);
                attributes.A = complexType.AttributeUsesTyped();
                this.AddAttributes(attributes, true);
                this.AddAttributes(attributes, false);
                baseTypeRef = 
                    complexType.IsMixed ? 
                        TypeRef<E.Mixed>() : 
                    dfa.D[new C.HashSet<int> { 0 }].Count == 0 ?
                        TypeRef<E.Empty>() :
                    // else
                        TypeRef<E.NotMixed>();
            }

            //
            var name = CS.Name.Cast(qName.Name);
            var implementation = 
                Parameter<Xml.Implementation>("Implementation");
            var typeRef = TypeRef("T." + name);
            this.U.Append(Namespace(CS.Namespace.Cast(qName.Namespace))
                [Type
                    (   Name: "X", 
                        IsPartial: true, 
                        Attributes: A.Public
                    )
                    [TypeRef<Xml.Implementation>()]
                    [Type
                        (   Name: "T", 
                            IsPartial: true, 
                            Attributes: A.Static | A.Public
                        )
                        [Type(Name: name, Attributes: A.Public)
                            [baseTypeRef]
                            [Constructor(Attributes: A.Public)
                                [implementation]
                                [attributes.Parameters]
                                [implementation.Ref()]
                                [attributes.Parameters.Select(
                                    x => 
                                        Invoke(
                                            x.Value == null ? 
                                                "AddRequiredAttribute": 
                                                "AddOptionalAttribute")
                                            [x.Ref()])
                                ]
                            ]
                        ]
                    ]
                    [Method
                        (   Name: name + "_", 
                            Attributes: A.Public, 
                            Return: typeRef
                        )
                        [attributes.Parameters]
                        [Return
                            (   New(typeRef)
                                    [This()]
                                    [attributes.Parameters.Select(x => x.Ref())]
                            )
                        ]
                    ]
                ]);
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
