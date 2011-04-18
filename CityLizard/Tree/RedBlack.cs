namespace CityLizard.Tree
{
    public class RedBlack<T>
    {
        private struct Data
        {
            public bool Red;
            public T Value;
        }

        private Base<Data> Base;

        private Base<Data>.Node Insert(Base<Data>.Position position, T value)
        {
            // Root.
            var node = this.Base.Insert(position, new Data { Red = true, Value = value });
            this.Insert1(node);
            return node;
        }

        private void Insert1(Base<Data>.Node node)
        {
            if (node.Parent == null)
            {
                node.Value.Red = false;
            }
            else
            {
                this.Insert2(node);
            }
        }

        private void Insert2(Base<Data>.Node node)
        {
            if (node.Parent.Value.Red)
            {
                this.Insert3(node);
            }
        }

        private void Insert3(Base<Data>.Node node)
        {
            var u = node.GetUncle();

            if (u != null && u.Value.Red)
            {
                node.Parent.Value.Red = false;
                u.Value.Red = false;
                var g = u.GetParent();
                g.Value.Red = true;
                this.Insert1(g);
            }
            else
            {
                this.Insert4(node);
            }
        }

        private void Insert4(Base<Data>.Node node)
        {
            var g = node.GetGrandParent();
 
            if (node == node.Parent.Right && node.Parent == g.Left)
            {
                this.Base.LeftRotation(node.Parent);
                node = node.Left;
            } 
            else if (node == node.Parent.Left && node.Parent == g.Right) 
            {
                this.Base.RightRotation(node.Parent);
                node = node.Right;
            }
            this.Insert5(node);
        }

        private void Insert5(Base<Data>.Node node)
        {
            var g = node.GetGrandParent();
 
            node.Parent.Value.Red = false;
            g.Value.Red = true;

            if (node == node.Parent.Left && node.Parent == g.Left) 
            {
                this.Base.RightRotation(g);
            }
            else if (node == node.Parent.Right && node.Parent == g.Right)
            {
                this.Base.LeftRotation(g);
            }
        }
    }
}
