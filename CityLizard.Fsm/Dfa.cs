namespace CityLizard.Fsm
{
    using C= System.Collections.Generic;
    using S = System;

    using System.Linq;
    using CityLizard.Collections.Extension;

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
            }

            /// <summary>
            /// Equals.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(State other)
            {
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
        }
    }
}
