namespace CityLizard.Tree
{
    using S = System;
    using D = System.Diagnostics;

    public enum Direction
    {
        Left,
        Right,
    }

    /// <summary>
    /// Basic tree structure.
    /// </summary>
    /// <typeparam name="T">User data.</typeparam>
    public class Base<T>
    {
        /// <summary>
        /// Position.
        /// </summary>
        public struct Position
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
            public Position(Node before, Node after)
            {
                this.Before = before;
                this.After = after;
            }

            public Node One()
            {
                return this.Before == this.After ? this.Before : null;
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

            public Node this[Direction direction]
            {
                get { return direction == Direction.Left ? this.Left : this.Right; }
                set
                {
                    if (direction == Direction.Left)
                    {
                        this.Left = value;
                    }
                    else
                    {
                        D.Debug.Assert(direction == Direction.Right);
                        this.Right = value;
                    }
                }
            }

            /// <summary>
            /// User data.
            /// </summary>
            public T Value;

            public void SetLeftChild(Node newChild)
            {
                newChild.SetParent(this);
                this.Left = newChild;
            }

            public void SetRightChild(Node newChild)
            {
                newChild.SetParent(this);
                this.Right = newChild;
            }

            public Node Next()
            {
                var i = this.Right;
                if (i != null)
                {
                    while (i.Right != null)
                    {
                        i = i.Right;
                    } ;
                    return i;
                }
                else
                {
                    var parent = this.Parent;
                    return parent != null && parent.Left == this ? parent : null;
                }
            }

            public Node Prior()
            {
                var i = this.Left;
                if (i != null)
                {
                    while(i.Left != null)
                    {
                        i = i.Left;
                    }
                    return i;
                }
                else
                {
                    var parent = this.Parent;
                    return parent != null && parent.Right == this ? parent : null;
                }
            }

            public Node Step(Direction direction)
            {
                var i = this[direction];
                if (i != null)
                {
                    while (i[direction] != null)
                    {
                        i = i[direction];
                    }
                    return i;
                }
                else
                {
                    var parent = this.Parent;
                    return 
                        parent != null && parent[direction.Revert()] == this ?
                            parent :
                            null;
                }
            }
        }

        public Node Root = new Node();
        public Node End = new Node();
        public Node Begin;

        public Base()
        {
            this.Begin = this.End;
        }

        /// <summary>
        /// Find an insertion place.
        /// </summary>
        /// <param name="compare">User data comparison function.</param>
        /// <returns>A pair.</returns>
        public Position Find(S.IComparable<T> compare)
        {
            var result = new Position();
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
                    return new Position(i, i);
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
                D.Debug.Assert(this.Root == oldChild);

                this.Root = newChild;
            }
            else
            {
                if (parent.Left == oldChild)
                {
                    parent.Left = newChild;
                }
                else
                {
                    D.Debug.Assert(parent.Right == oldChild);
                    parent.Right = newChild;
                }
            }
            newChild.Parent = parent;
        }

        public void RightRotation(Node node)
        {
            var left = node.Left;
            this.ChangeChild(node, left);
            node.SetLeftChild(left.Right);
            left.SetRightChild(node);
        }

        public void LeftRotation(Node node)
        {
            var right = node.Right;
            this.ChangeChild(node, right);
            node.SetRightChild(right.Left);
            right.SetLeftChild(node);
        }

        public Node Insert(Position position, T value)
        {
            D.Debug.Assert(position.One() != null);

            var result = new Node { Value = value };

            if (position.Before == null && position.After == null)
            {
                this.Root = result;
                result.Parent = null;
            }
            else if (position.Before != null && position.Before.Right == null)
            {
                position.Before.Right = result;
                result.Parent = position.Before;
            }
            else
            {
                D.Debug.Assert(position.After != null && position.After.Left == null);
                
                position.After.Left = result;
                result.Parent = position.After;
            }
            return result;
        }

        public Position Remove(Node node)
        {
            D.Debug.Assert(node != null);

            var left = node.Left;
            var right = node.Right;
            if (left != null)
            {
                this.ChangeChild(node, left);
            }
            else
            {
                this.ChangeChild(node, right);
            }

            // var result = new Position(node.Prior(), node.Next());
            /*
            var parent = node.Parent;
            if (parent != null)
            {
                if (parent.Left == node)
                {
                }
                else
                {
                    D.Debug.Assert(parent.Right == node);
                }
            }
            else
            {
            }
             * */
            return new Position();
        }
    }

    /// <summary>
    /// Extension methods. First parameter can be null.
    /// </summary>
    public static class BaseNodeExtension
    {
        public static Direction Revert(this Direction this_)
        {
            D.Debug.Assert(this_ == Direction.Left || this_ == Direction.Right);

            return this_ == Direction.Left ? Direction.Right : Direction.Left;
        }

        public static void SetParent<T>(
            this Base<T>.Node this_, Base<T>.Node parent)
        {
            if (this_ != null)
            {
                this_.Parent = parent;
            }
        }

        public static Base<T>.Node GetParent<T>(this Base<T>.Node this_)
        {
            return this_ != null ? this_.Parent : null;
        }

        public static Base<T>.Node GetGrandParent<T>(this Base<T>.Node this_)
        {
            return this_.GetParent().GetParent();
        }

        public static Base<T>.Node GetSibling<T>(this Base<T>.Node this_)
        {
            var parent = this_.GetParent();
            return
                parent == null ? null :
                this_ == parent.Left ? parent.Right : parent.Left;
        }

        public static Base<T>.Node GetUncle<T>(this Base<T>.Node this_)
        {
            return this_.GetParent().GetSibling();
        }
    }
}
