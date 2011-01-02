namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using C = System.Collections.Generic;
    using CS = System.Collections.Specialized;
    using XS = System.Xml.Schema;
    using S = System;

    using F = Fsm;

    using Extension;
    using System.Linq;

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

            // all (requires for nuget: http://nuget.codeplex.com/)
            // set -A-> {A} -B-> {AB}
            // set -B-> {B} -A-> {AB}
            {
                var all = p as XS.XmlSchemaAll;
                if (all != null)
                {
                    var list = all.ItemsTyped().ToList();
                    var setMap = new C.Dictionary<CS.BitVector32, C.ISet<int>>
                        { { new CS.BitVector32(0), set } };
                    for (var i = 0; i < list.Count; ++i)
                    {
                        var newSetMap = 
                            new C.Dictionary<CS.BitVector32, C.ISet<int>>();
                        foreach (var pair in setMap)
                        {
                            for (var j = 0; j < list.Count; ++j)
                            {
                                var k = pair.Key;
                                var m = 1 << j;
                                if (!k[m])
                                {
                                    k[m] = true;
                                    C.ISet<int> s;
                                    if (!newSetMap.TryGetValue(k, out s))
                                    {
                                        newSetMap[k] = s = new C.HashSet<int>();
                                    }
                                    //     
                                    var x = new C.HashSet<int>(pair.Value);
                                    this.Apply(x, list[j]);
                                    s.UnionWith(x);
                                }
                            }
                        }
                        setMap = newSetMap;
                    }
                    set.Clear();
                    set.UnionWith(setMap.First().Value);
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
