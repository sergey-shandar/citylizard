namespace CityLizard
{
    using C = System.Collections.Generic;
    using S = System;

    using System.Linq;

    public class Fsm<T>
    {
        public struct Transition
        {
            public T Symbol;
            public int State;
        }

        public readonly C.List<C.HashSet<Transition>> StateList = 
            new C.List<C.HashSet<Transition>>();

        public Fsm()
        {
            this.AddState();
        }

        private int AddState()
        {
            this.StateList.Add(new C.HashSet<Transition>());
            return this.StateList.Count - 1;
        }

        private int Add(int state, T symbol, int to)
        {
            this.StateList[state].Add(
                new Transition { Symbol = symbol, State = to });
            return to;
        }

        private int Add(C.ISet<int> set, T symbol, int to)
        {
            foreach (var state in set)
            {
                Add(state, symbol, to);
            }
            return to;
        }

        public int AddNew(C.ISet<int> set, T symbol)
        {
            return this.Add(set, symbol, this.AddState());
        }

        public delegate void Transform(C.ISet<int> stateSet);

        public void Loop(C.ISet<int> set, Transform transform, int min, int max)
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
    }
}
