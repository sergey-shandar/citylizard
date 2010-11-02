namespace CityLizard.Fsm
{
    using C= System.Collections.Generic;
    using S = System;

    using System.Linq;

    /// <summary>
    /// Deterministic finite automaton.
    /// <see cref="http://en.wikipedia.org/wiki/Deterministic_finite_automaton"
    /// />
    /// </summary>
    /// <typeparam name="Symbol">Symbol type.</typeparam>
    public class Dfa<Symbol>
    {
        private static readonly C.IEqualityComparer<C.HashSet<int>> comparer =
            C.HashSet<int>.CreateSetComparer();

        /// <summary>
        /// State.
        /// </summary>
        public class State : 
            C.Dictionary<Symbol, C.HashSet<int>>, S.IEquatable<State>
        {
            /// <summary>
            /// true if this state is accept state.
            /// <see cref=
            /// "http://en.wikipedia.org/wiki/Finite-state_machine#Accept_state"
            /// />
            /// </summary>
            public readonly bool Accept = false;

            public State(
                Fsm<Symbol> fsm, C.HashSet<int> accept, C.HashSet<int> key)
            {
                // copy all FSM transitions.
                foreach (var fsmState in key)
                {
                    if (!this.Accept)
                    {
                        this.Accept = accept.Contains(fsmState);
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
                if (this.Accept != other.Accept || this.Count != other.Count)
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

        /// <summary>
        /// Dictionary of states by state id.
        /// </summary>
        public class Dictionary : C.Dictionary<C.HashSet<int>, State>
        {
            public Dictionary(): base(comparer)
            {
            }
        }

        /// <summary>
        /// Instance of the dictionary.
        /// </summary>
        public Dictionary D = new Dictionary();

        /// <summary>
        /// The constructor builds DFA.
        /// </summary>
        /// <param name="fsm">Finite-state machine.</param>
        /// <param name="accept">Accept state.</param>
        public Dfa(Fsm<Symbol> fsm, C.HashSet<int> accept)
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
                            var state = new State(fsm, accept, key);
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
