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
            /// Before.
            /// </summary>
            public Node Before;

            /// <summary>
            /// Right.
            /// </summary>
            public Node After;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="before">Before.</param>
            /// <param name="after">After.</param>
            public Pair(Node before, Node after)
            {
                this.Before = before;
                this.After = after;
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
            /// Left.
            /// </summary>
            public Node Left;

            /// <summary>
            /// Right.
            /// </summary>
            public Node Right;

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
                    result.After = i;
                    i = i.Left;
                }
                else if (sign > 0)
                {
                    result.Before = i;
                    i = i.Right;
                }
                // sign == 0
                else
                {
                    return new Pair(i, i);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldChild">Must not be null.</param>
        /// <param name="newChild">Must not be null.</param>
        private void ChangeChild(Node oldChild, Node newChild)
        {
            var parent = oldChild.Parent;
            if (parent == null)
            {
                this.Root = newChild;
            }
            else if (parent.Left == oldChild)
            {
                parent.Left = newChild;
            }
            else
            {
                parent.Right = newChild;
            }
            newChild.Parent = parent;
        }

        private static void SetParent(Node child, Node parent)
        {
            if (child != null)
            {
                child.Parent = parent;
            }
        }

        private static void SetLeftChild(Node parent, Node newChild)
        {
            SetParent(newChild, parent);
            parent.Left = newChild;
        }

        private static void SetRightChild(Node parent, Node newChild)
        {
            SetParent(newChild, parent);
            parent.Right = newChild;
        }

        public void RightRotation(Node node)
        {
            var left = node.Left;
            this.ChangeChild(node, left);
            SetLeftChild(node, left.Right);
            SetRightChild(left, node);
        }

        public void LeftRotation(Node node)
        {
            var right = node.Right;
            this.ChangeChild(node, right);
            SetRightChild(node, right.Left);
            SetLeftChild(right, node);
        }

        public static Node Parent(Node node)
        {
            return node != null ? node.Parent : null;
        }

        public static Node GrandParent(Node node)
        {
            return Parent(Parent(node));
        }

        public static Node Sibling(Node node)
        {
            var parent = Parent(node);
            return 
                parent == null ? null :
                node == parent.Left ? parent.Right : parent.Left;
        }

        public static Node Uncle(Node node)
        {
            return Sibling(Parent(node));
        }
    }
}
