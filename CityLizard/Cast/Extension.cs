namespace CityLizard.Cast
{
    public static class Extension
    {
        public static T UpCast<T>(T derived)
        {
            return derived;
        }
    }
}
