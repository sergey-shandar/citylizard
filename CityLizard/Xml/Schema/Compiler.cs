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

    public class Compiler: D
    {
        T.Unit U;
        ElementSet Done;
        C.IEnumerable<XS.XmlSchemaElement> ToDo;

        class TypeDfa
        {
            private F.Fsm<X.XmlQualifiedName> Fsm = 
                new F.Fsm<X.XmlQualifiedName>();

            private ElementSet ToDo;

            public TypeDfa(ElementSet toDo)
            {
                this.ToDo = toDo;
            }

            private void ApplyOne(C.ISet<int> set, XS.XmlSchemaParticle p)
            {
                // sequence
                {
                    var sequence = p as XS.XmlSchemaSequence;
                    if (sequence != null)
                    {
                        foreach (var i in sequence.ItemsTyped())
                        {
                            this.Apply(set, i);
                        }
                        return;
                    }
                }

                // choice
                {
                    var choice = p as XS.XmlSchemaChoice;
                    if (choice != null)
                    {
                        var x = new C.HashSet<int>(set);
                        set.Clear();
                        foreach (var i in choice.ItemsTyped())
                        {
                            var xi = new C.HashSet<int>(x);
                            this.Apply(xi, i);
                            set.UnionWith(xi);
                        }
                        return;
                    }
                }

                // element
                {
                    var element = p as XS.XmlSchemaElement;
                    if (element != null)
                    {
                        this.ToDo.Add(element);
                        //
                        var i = this.Fsm.AddNew(set, element.QualifiedName);
                        set.Clear();
                        set.Add(i);
                        return;
                    }
                }

                //            
                throw new S.Exception(
                    "unknown XmlSchemaObject type: " + p.ToString());
            }

            private void Apply(C.ISet<int> set, XS.XmlSchemaParticle p)
            {
                // group ref
                {
                    var groupRef = p as XS.XmlSchemaGroupRef;
                    if (groupRef != null)
                    {
                        p = groupRef.Particle;
                    }
                }

                if (p == null)
                {
                    return;
                }

                var min = (int)p.MinOccurs;
                var max =
                    p.MaxOccurs == decimal.MaxValue ?
                        int.MaxValue :
                    // else
                        (int)p.MaxOccurs;

                this.Fsm.Loop(set, x => this.ApplyOne(x, p), min, max);
            }

            public F.Dfa<X.XmlQualifiedName> Apply(XS.XmlSchemaComplexType type)
            {
                var set = new C.HashSet<int> { 0 };
                this.Apply(set, type.Particle);
                return new F.Dfa<X.XmlQualifiedName>(this.Fsm, set);
            }
        }

        private static void ToDfa(
            ElementSet newToDo, XS.XmlSchemaComplexType type)
        {
            new TypeDfa(newToDo).Apply(type);
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
            if(complexType == null)
            {
                baseTypeRef = TypeRef<E.Simple>();
            }
            else
            {
                ToDfa(newToDo, complexType);
                baseTypeRef = 
                    complexType.IsMixed ? 
                        TypeRef<E.Mixed>() : 
                        TypeRef<E.NotMixed>();
            }
            this.U.Append(Namespace(CS.Namespace.Cast(qName.Namespace))
                [Type(
                    Name: "X", 
                    IsPartial: true, 
                    Attributes: CD.MemberAttributes.Public)
                    [TypeRef<Xml.Implementation>()]
                    [Type(
                        Name: "T", 
                        IsPartial: true, 
                        Attributes: 
                            CD.MemberAttributes.Static | 
                            CD.MemberAttributes.Public)
                        [Type(
                            Name: CS.Name.Cast(qName.Name),
                            Attributes: CD.MemberAttributes.Public)
                            [baseTypeRef]
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
            while (AddElementSet()) { }
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
