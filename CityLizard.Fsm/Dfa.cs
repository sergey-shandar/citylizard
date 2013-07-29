namespace CityLizard.Fsm
{
    public class Dfa<Symbol>
    {
        public abstract class State: System.IEquatable<State>
        {
            public abstract bool Equals(State other);
        }
    }
}
