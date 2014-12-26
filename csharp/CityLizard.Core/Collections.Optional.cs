using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CityLizard.Xml;

namespace CityLizard.Collections
{
    public abstract class Optional<T>
    {
        public abstract TR Select<TR>(
            Func<T, TR> thenFunc, System.Func<TR> elseFunc);

        public TR Select<TR>(Func<T, TR> thenFunc, TR defaultValue)
        {
            return Select(thenFunc, () => defaultValue);
        }

        public Optional<TR> Select<TR>(Func<T, TR> func)
        {
            return Select(v => func(v).Optional(), Optional<TR>.Absent);
        }

        public T Default(System.Func<T> elseFunc)
        {
            return Select(v => v, elseFunc);
        }

        public T Default(T value)
        {
            return Default(() => value);
        }

        public void ForEach(Action<T> thenAction, Action elseAction)
        {
            Select(
                v =>
                {
                    thenAction(v);
                    return new Void();
                },
                () =>
                {
                    elseAction();
                    return new Void();
                });
        }

        public void ForEach(Action<T> action)
        {
            ForEach(action, () => { });
        }

        public bool ValueEqual(T value)
        {
            return Select(v => v.Equals(value), false);
        }

        public bool Equal(Optional<T> other)
        {
            return Select(other.ValueEqual, () => !other.HasValue);
        }

        public bool HasValue { get { return Select(v => true, false); } }

        public IEnumerable<T> ToEnumerable()
        {
            return Select(v => new[] {v}, Enumerable.Empty<T>());
        }

        public override bool Equals(object obj)
        {
            return obj.
                DownCast()._<Optional<T>>().
                Select(Equal, false);
        }

        public override int GetHashCode()
        {
            return Select(v => v.GetHashCode(), 0);
        }

        public override string ToString()
        {
            return "(" + Select(v => v.ToString(), "") + ")";
        }

        public static Optional<T> Absent()
        {
            return new OptionalAbsent();
        }

        private class OptionalAbsent: Optional<T>
        {
            public override TR Select<TR>(Func<T, TR> thenFunc, System.Func<TR> elseFunc)
            {
                return elseFunc();
            }
        }
    }

    public static class OptionalExtensions
    {
        public static Optional<T> Optional<T>(this T value)
        {
            return new OptionalValue<T>(value);
        }

        private sealed class OptionalValue<T> : Optional<T>
        {
            public override TR Select<TR>(Func<T, TR> thenFunc, System.Func<TR> elseFunc)
            {
                return thenFunc(Value);
            }

            public OptionalValue(T value)
            {
                Value = value;
            }

            private readonly T Value;
        }
    }

}
