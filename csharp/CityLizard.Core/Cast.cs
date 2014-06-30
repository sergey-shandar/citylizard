using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityLizard.Collections;

namespace CityLizard
{
    public class DownCast<T>
    {
        public DownCast(T value)
        {
            Value = value;
        }

        public Optional<TR> _<TR>()
            where TR: T
        {
            return Value is TR ? ((TR)Value).Optional() : Optional<TR>.Absent();
        }

        private readonly T Value;
    }

    public static class CastExtension
    {
        public static DownCast<T> DownCast<T>(this T value)
        {
            return new DownCast<T>(value);
        }

        public static T UpCast<T>(this T value)
        {
            return value;
        }
    }
}
