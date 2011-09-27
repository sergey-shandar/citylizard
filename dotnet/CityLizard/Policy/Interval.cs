namespace CityLizard.Policy
{
    public struct Interval<P, T>
        where P : struct, INumeric<T>
        where T : System.IComparable<T>
    {
        public T Lower;
        public T Upper;

        public T Width
        {
            get { return new P().Subtract(this.Upper, this.Lower); }
        }
    }
}
