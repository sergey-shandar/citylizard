namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using C = System.Collections.Generic;
    using XS = System.Xml.Schema;
    using S = System;

    using F = Fsm;

    using Extension;

    internal class ComplexTypeToDfa
    {
        private readonly F.Fsm<X.XmlQualifiedName> Fsm =
            new F.Fsm<X.XmlQualifiedName>();

        private readonly ElementSet ToDo;

        public ComplexTypeToDfa(ElementSet toDo)
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

            // all
            // set -A-> {A} -B-> {AB}
            // set -B-> {B} -A-> {AB}
            {
                var all = p as XS.XmlSchemaAll;
                if (all != null)
                {
                    
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

            // any
            {
                var any = p as XS.XmlSchemaAny;
                if (any != null)
                {
                    var i = this.Fsm.AddNew(set, new X.XmlQualifiedName());
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
}
