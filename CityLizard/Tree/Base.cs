namespace CityLizard.Tree
{
    using S = System;

    /// <summary>
    /// Basic tree structure.
    /// </summary>
    /// <typeparam name="T">User data.</typeparam>
    class Base<T>
    {
        /// <summary>
        /// Pair.
        /// </summary>
        public struct Pair
        {
            /// <summary>
            /// Left.
            /// </summary>
            public Node Left;

            /// <summary>
            /// Right.
            /// </summary>
            public Node Right;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="left">Left.</param>
            /// <param name="right">Right.</param>
            public Pair(Node left, Node right)
            {
                this.Left = left;
                this.Right = right;
            }
        }

        /// <summary>
        /// Tree node.
        /// </summary>
        public class Node
        {
            /// <summary>
            /// Parent.
            /// </summary>
            public Node Parent;

            /// <summary>
            /// Left and Right.
            /// </summary>
            public Pair Between;

            /// <summary>
            /// User data.
            /// </summary>
            public T Value;
        }

        private Node Root;

        /// <summary>
        /// Find an insertion place.
        /// </summary>
        /// <param name="compare">User data comparison function.</param>
        /// <returns>A pair.</returns>
        public Pair Find(S.IComparable<T> compare)
        {
            var result = new Pair();
            var i = this.Root;
            while(i != null)
            {
                var sign = compare.CompareTo(i.Value);
                if (sign < 0)
                {
                    result.Right = i;
                    i = i.Between.Left;
                }
                else if (sign > 0)
                {
                    result.Left = i;
                    i = i.Between.Right;
                }
                // sign == 0
                else
                {
                    return new Pair(i, i);
                }
            }
            return result;
        }
    }
}
