namespace CityLizard.Fsm
{
    using C= System.Collections.Generic;
    using S = System;

    using System.Linq;
    using CityLizard.Collections;

    /// <summary>
    /// Deterministic finite automaton.
    /// http://en.wikipedia.org/wiki/Deterministic_finite_automaton
    /// </summary>
    /// <typeparam name="Symbol">Symbol type.</typeparam>
    public class Dfa<Symbol>
    {
        /// <summary>
        /// State.
        /// </summary>
        public class State : 
            C.Dictionary<Symbol, Name>, S.IEquatable<State>
        {
            /// <summary>
            /// true if this state is accept state.
            /// http://en.wikipedia.org/wiki/Finite-state_machine#Accept_state
            /// </summary>
            public readonly bool Accept = false;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="fsm">Finite-state machine.</param>
            /// <param name="accept">FSM accept states.</param>
            /// <param name="key">FSM states.</param>
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
                        var state = this.TryGet(transition.Symbol);
                        if (!state.HasValue)
                        {
                            state = new Collections.Optional<Name>(new Name());
                            this[transition.Symbol] = state.Value;
                        }
                        state.Value.Add(transition.State);
                    }
                }
            }

            /// <summary>
            /// Equals.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(State other)
            {
                if (this.Accept != other.Accept || this.Count != other.Count)
                {
                    return false;
                }
                foreach (var p in this)
                {
                    var otherState = other.TryGet(p.Key);
                    if (!otherState.HasValue || p.Value != otherState.Value)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Instance of the dictionary.
        /// </summary>
        public C.Dictionary<Name, State> D = new C.Dictionary<Name, State>();

        /// <summary>
        /// The constructor builds DFA.
        /// </summary>
        /// <param name="fsm">Finite-state machine.</param>
        /// <param name="accept">Accept state.</param>
        public Dfa(Fsm<Symbol> fsm, C.HashSet<int> accept)
        {
            var startKey = new Name() { 0 };
            {
                var toDo = new C.HashSet<Name>() { startKey };
                //
                while (toDo.Count != 0)
                {
                    var newToDo = new C.HashSet<Name>();
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
                var newD = new C.Dictionary<Name, State>();
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
                            var copyK = new Name(k);
                            foreach(var state in D.Values)
                            {
                                foreach (var transition in state.Values)
                                {
                                    if (copyK == transition)
                                    {
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
