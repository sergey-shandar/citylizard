namespace CityLizard.Xml
{
    using IO = System.IO;
    using X = System.Xml;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    /// <summary>
    /// The node object represents a single node in the XML document tree.
    /// </summary>
    public abstract class Node : INode
    {
        /// <summary>
        /// Writes the node to the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="parentNamespace">
        /// true to write without xmlns attribute; otherwise, false.
        /// </param>
        public abstract void ToTextWriter(
            IO.TextWriter writer, string parentNamespace);

        public abstract void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace);

        /// <summary>
        /// Returns a string that represents the current node.
        /// </summary>
        /// <returns>String that represents the current node.</returns>
        public override string ToString()
        {
            var w = new IO.StringWriter();
            w.WriteNode(this, null);
            return w.ToString();
        }
    }
}