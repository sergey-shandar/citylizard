namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using C = System.Collections.Generic;
    using CD = System.CodeDom;
    using S = System;

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

        private void SetType(
            ElementSet newToDo,
            XS.XmlSchemaElement element,
            bool isRoot = false)
        {
            var qName = element.QualifiedName;
            var type = element.ElementSchemaType;

            var complexType = type as XS.XmlSchemaComplexType;
            T.TypeRef baseTypeRef;

            var attributeList = new C.List<T.Parameter>();

            // simple type
            if(complexType == null)
            {
                baseTypeRef = TypeRef<E.Simple>();
            }
            // complex type
            else
            {
                var dfa = new ComplexTypeToDfa(newToDo).Apply(complexType);
                var attributes = complexType.AttributeUsesTyped();
                foreach (
                    var a in 
                    attributes.Where(x => x.Use == XS.XmlSchemaUse.Required))
                {
                    attributeList.Add(Parameter<string>(CS.Name.Cast(a.QualifiedName.Name)));
                }
                foreach (
                    var a in 
                    attributes.Where(x => x.Use != XS.XmlSchemaUse.Optional))
                {
                    attributeList.Add(
                        Parameter<string>(
                            CS.Name.Cast(a.QualifiedName.Name), Primitive(null)));
                }
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
                        Attributes: CD.MemberAttributes.Public
                    )
                    [TypeRef<Xml.Implementation>()]
                    [Type
                        (   Name: "T", 
                            IsPartial: true, 
                            Attributes: 
                                CD.MemberAttributes.Static | 
                                CD.MemberAttributes.Public
                        )
                        [Type
                            (   Name: name,
                                Attributes: CD.MemberAttributes.Public
                            )
                            [baseTypeRef]
                            [Constructor(Attributes: CD.MemberAttributes.Public)
                                [implementation]
                                [attributeList]
                                [implementation.Ref()]
                            ]
                        ]
                    ]
                    [Method
                        (   Name: name + "_",
                            Attributes: CD.MemberAttributes.Public,
                            Return: typeRef
                        )
                        [attributeList]
                        [Return
                            (   New(typeRef)
                                    [This()]
                                    [attributeList.Select(x => x.Ref())]
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
