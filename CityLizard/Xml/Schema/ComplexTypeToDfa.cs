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
        public ComplexTypeToDfa(ElementSet toDo)
        {
            this.ToDo = toDo;
        }

        public F.Dfa<X.XmlQualifiedName> Apply(XS.XmlSchemaComplexType type)
        {
            var set = new Fsm.Name { 0 };
            this.Apply(set, type.Particle);
            return new F.Dfa<X.XmlQualifiedName>(this.Fsm, set);
        }

        private readonly F.Fsm<X.XmlQualifiedName> Fsm =
            new F.Fsm<X.XmlQualifiedName>();

        private readonly ElementSet ToDo;

        private void ApplyOne(Fsm.Name set, XS.XmlSchemaParticle p)
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
                    // var x = new C.HashSet<int>(set);
                    var x = new Fsm.Name(set);
                    set.Clear();
                    foreach (var i in choice.ItemsTyped())
                    {
                        // var xi = new C.HashSet<int>(x);
                        var xi = new Fsm.Name(x);
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
                    var setMap = new C.Dictionary<CS.BitVector32, Fsm.Name> 
                        { { new CS.BitVector32(0), set } };
                    for (var i = 0; i < list.Count; ++i)
                    {
                        var newSetMap = 
                            new C.Dictionary<CS.BitVector32, Fsm.Name>();
                        foreach (var pair in setMap)
                        {
                            for (var j = 0; j < list.Count; ++j)
                            {
                                var k = pair.Key;
                                var m = 1 << j;
                                if (!k[m])
                                {
                                    k[m] = true;
                                    Fsm.Name s;
                                    if (!newSetMap.TryGetValue(k, out s))
                                    {
                                        newSetMap[k] = s = new Fsm.Name();
                                    }
                                    //     
                                    var x = new Fsm.Name(pair.Value);
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

        private void Apply(Fsm.Name set, XS.XmlSchemaParticle p)
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


    }
}
