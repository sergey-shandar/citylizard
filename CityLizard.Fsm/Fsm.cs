namespace CityLizard.Fsm
{
    using C = System.Collections.Generic;
    using S = System;

    using System.Linq;

    /// <summary>
    /// Finite-state machine.
    /// http://en.wikipedia.org/wiki/Finite-state_machine
    /// </summary>
    /// <typeparam name="T">Symbol type.</typeparam>
    public class Fsm<T>
    {
        /// <summary>
        /// Transition.
        /// </summary>
        public struct Transition
        {
            /// <summary>
            /// Transition symbol.
            /// </summary>
            public T Symbol;

            /// <summary>
            /// Transition to this state.
            /// </summary>
            public int State;
        }

        /// <summary>
        /// State.
        /// </summary>
        public class State : C.HashSet<Transition>
        {
            /// <summary>
            /// true if no transitions from this state.
            /// </summary>
            public bool Empty { get { return this.Count == 0; } }
        }

        /// <summary>
        /// List of states.
        /// </summary>
        public readonly C.List<State> StateList = new C.List<State>();

        /// <summary>
        /// The constructor adds a start state.
        /// http://en.wikipedia.org/wiki/Finite-state_machine#Start_state
        /// </summary>
        public Fsm()
        {
        }

        /// <summary>
        /// Add a new state.
        /// </summary>
        /// <returns>State id.</returns>
        private int AddState()
        {
        }

        /// <summary>
        /// Add a new transition.
        /// </summary>
        /// <param name="state">From.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <param name="to">To.</param>
        /// <returns>To.</returns>
        private int Add(int state, T symbol, int to)
        {
            return to;
        }

        /// <summary>
        /// Add a new transition from multiple states.
        /// </summary>
        /// <param name="set">Set of states.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <param name="to">To.</param>
        /// <returns>To.</returns>
        private int Add(Name set, T symbol, int to)
        {
            return to;
        }

        /// <summary>
        /// Add a new transition from multiple states to a new state.
        /// </summary>
        /// <param name="set">Set of 'from' states.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <returns>To.</returns>
        public int AddNew(Name set, T symbol)
        {
            return this.Add(set, symbol, this.AddState());
        }

        /// <summary>
        /// Add a loop.
        /// </summary>
        /// <param name="set">Set of 'from' states.</param>
        /// <param name="transform">Apply transitions.</param>
        /// <param name="min">Minimum number of transitions (0..).</param>
        /// <param name="max">
        /// Maximum number of transitions. No limit if the value equal 
        /// int.MaxValue.
        /// </param>
        public void Loop(Name set, S.Action<Name> transform, int min, int max)
        {
        }
    }
}
