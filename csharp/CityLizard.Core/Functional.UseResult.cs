using System;

namespace CityLizard.Functional
{
    public static class Extension
    {
        public static Result UseResult<Result>(Func<Func<Result>, Result> f)
        {
            Result result = default(Result);
            result = f(() => result);
            return result;
        }
    }
}
