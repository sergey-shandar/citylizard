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

            /// <summary>
            /// Back-links.
            /// </summary>
            public C.List<Transition> FromList = new C.List<Transition>();
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
            this.AddState();
        }

        /// <summary>
        /// Add a new state.
        /// </summary>
        /// <returns>State id.</returns>
        private int AddState()
        {
            this.StateList.Add(new State());
            return this.StateList.Count - 1;
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
            this.StateList[state].Add(
                new Transition { Symbol = symbol, State = to });
            // backlink.
            this.StateList[to].FromList.Add(
                new Transition { Symbol = symbol, State = state });
            return to;
        }

        /// <summary>
        /// Add a new transition from multiple states.
        /// </summary>
        /// <param name="set">Set of states.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <param name="to">To.</param>
        /// <returns>To.</returns>
        private int Add(C.ISet<int> set, T symbol, int to)
        {
            foreach (var state in set)
            {
                Add(state, symbol, to);
            }
            return to;
        }

        /// <summary>
        /// Add a new transition from multiple states to a new state.
        /// </summary>
        /// <param name="set">Set of 'from' states.</param>
        /// <param name="symbol">Transition symbol.</param>
        /// <returns>To.</returns>
        public int AddNew(C.ISet<int> set, T symbol)
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
        public void Loop(
            C.ISet<int> set, S.Action<C.ISet<int>> transform, int min, int max)
        {
            // min
            for (var i = 0; i < min; ++i)
            {
                transform(set);
            }
            // max
            if (max < int.MaxValue)
            {
                var copy = new C.HashSet<int>(set);
                for (var i = min; i < max; ++i)
                {
                    transform(copy);
                    set.UnionWith(copy);
                }
            }
            // infinity
            else
            {
                var label = this.AddState();
                var copy = new C.HashSet<int>(set);
                set.Add(label);
                transform(set);
                var labelTransactionList = this.StateList[label];
                foreach (var state in set)
                {
                    this.StateList[state].UnionWith(labelTransactionList);
                }
                set.UnionWith(copy);
            }
        }

        public void Combine(C.ISet<int> name)
        {
            if (name.Count != 1)
            {
                var state = this.AddState();
                foreach (var s in name)
                {
                    foreach (var t in this.StateList[s].FromList)
                    {
                        this.Add(t.State, t.Symbol, state);
                    }
                }
                name.Clear();
                name.Add(state);
            }
        }

        /*
        public void Optimize(C.ISet<int> name)
        {
            var copy = new C.HashSet<int>(name);
            var firstEmpty = -1;
            foreach (var i in copy)
            {
                var state = this.StateList[i];
                if (state.Empty)
                {
                    if (firstEmpty == -1)
                    {
                        firstEmpty = i;
                    }
                    else
                    {
                        // backlinks are required to get rid of the loops.
                        foreach (var s in this.StateList)
                        {
                            foreach (var t in s)
                            {
                                if (t.State == i)
                                {
                                    t.State = firstEmpty;   
                                }
                            }
                        }
                        //
                        name.Remove(i);
                    }
                }
            }
        }
        */
    }
}
