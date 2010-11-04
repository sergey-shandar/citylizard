namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    /// <summary>
    /// The Node object represents a single node in the document tree.
    /// </summary>
    public abstract class Node : INode
    {
        public abstract void ToTextWriter(
            T.StringBuilder builder, string parentNamespace);

        public override string ToString()
        {
            var builder = new T.StringBuilder();
            builder.AppendNode(this, null);
            return builder.ToString();
        }
    }
}