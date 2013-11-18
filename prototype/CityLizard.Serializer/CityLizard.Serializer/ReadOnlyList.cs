using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLizard.Serializer
{
    public class ReadOnlyList<T>: IReadOnlyList<T>
    {
        public readonly T[] array;

        public T this[int index]
        {
            get { return array[index]; }
        }

        public int Count
        {
            get { return array.Length; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return array.UpCast<IEnumerable<T>>().GetEnumerator();
        }

        System.Collections.IEnumerator
            System.Collections.IEnumerable.GetEnumerator()
        {
            return array.GetEnumerator();
        }

        public ReadOnlyList(IEnumerable<T> valueList)
        {
            array = valueList.ToArray();
        }

        public ReadOnlyList(params T[] valueList):
            this(valueList.UpCast<IEnumerable<T>>())
        {
        }
    }
}
