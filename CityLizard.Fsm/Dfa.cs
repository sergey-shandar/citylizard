namespace CityLizard.Fsm
{
    public class Dfa<Symbol>
    {
        public class State : System.IEquatable<State>
        {
            public bool Equals(State other)
            {
                return true;
            }
        }
    }
}
