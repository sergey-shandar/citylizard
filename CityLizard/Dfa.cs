﻿namespace CityLizard
{
    using C= System.Collections.Generic;
    using S = System;

    using System.Linq;

    public class Dfa<Symbol>
    {
        static readonly C.IEqualityComparer<C.HashSet<int>> comparer =
            C.HashSet<int>.CreateSetComparer();

        public class State : 
            C.Dictionary<Symbol, C.HashSet<int>>, S.IEquatable<State>
        {
            public readonly bool Last = false;

            public State(
                Fsm<Symbol> fsm, C.HashSet<int> last, C.HashSet<int> key)
            {
                // copy all FSM transitions.
                foreach (var fsmState in key)
                {
                    if (!this.Last)
                    {
                        this.Last = last.Contains(fsmState);
                    }
                    foreach (var transition in fsm.StateList[fsmState])
                    {
                        // add transition.
                        C.HashSet<int> state;
                        if (!this.TryGetValue(transition.Symbol, out state))
                        {
                            state = new C.HashSet<int>();
                            this[transition.Symbol] = state;
                        }
                        state.Add(transition.State);
                    }
                }
            }

            public bool Equals(State other)
            {
                if (this.Last != other.Last || this.Count != other.Count)
                {
                    return false;
                }
                foreach (var p in this)
                {
                    C.HashSet<int> otherState;
                    if (!other.TryGetValue(p.Key, out otherState))
                    {
                        return false;
                    }
                    if (!comparer.Equals(p.Value, otherState))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public class Dictionary : C.Dictionary<C.HashSet<int>, State>
        {
            public Dictionary(): base(comparer)
            {
            }
        }

        public Dictionary D = new Dictionary();

        public Dfa(Fsm<Symbol> fsm, C.HashSet<int> last)
        {
            var startKey = new C.HashSet<int> { 0 };
            {
                var toDo = new C.HashSet<C.HashSet<int>>() { startKey };
                //
                while (toDo.Count != 0)
                {
                    var newToDo = new C.HashSet<C.HashSet<int>>();
                    foreach (var key in toDo)
                    {
                        if (!this.D.ContainsKey(key))
                        {
                            var state = new State(fsm, last, key);
                            //
                            this.D.Add(key, state);
                            // add to the newToDo set.
                            newToDo.UnionWith(state.Values);
                        }
                    }
                    toDo = newToDo;
                }
            }
            // optimization:
            while(true)
            {
                bool changed = false;
                //
                var newD = new Dictionary();
                newD[startKey] = this.D[startKey];
                foreach (var p in this.D)
                {
                    if(p.Key != startKey)
                    {
                        var v = p.Value;
                        var k = p.Key;
                        var newP = newD.FirstOrDefault(x => v.Equals(x.Value));
                        var newK = newP.Key;
                        if (newK == null)
                        {
                            newD[k] = v;
                        }
                        else
                        {
                            var copyK = new C.HashSet<int>(k);
                            foreach(var state in D.Values)
                            {
                                foreach (var transition in state.Values)
                                {
                                    if (comparer.Equals(copyK, transition))
                                    {
                                        // state[transition.Key] = newK;
                                        transition.Clear();
                                        transition.UnionWith(newK);
                                        changed = true;
                                    }
                                }
                            }
                        }
                    }
                }
                this.D = newD;
                if (!changed)
                {
                    break;
                }
            }
        }
    }
}
