namespace CityLizard.Xml
{
    using IO = System.IO;
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
            IO.TextWriter writer, string parentNamespace);

        public override string ToString()
        {
            var w = new IO.StringWriter();
            w.WriteNode(this, null);
            return w.ToString();
        }
    }
}