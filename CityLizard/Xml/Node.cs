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
        /// Writes the node to the XML writer.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="parentNamespace">
        /// Parent namespace.
        /// </param>
        public abstract void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace);

        /// <summary>
        /// Returns a string that represents the current node.
        /// </summary>
        /// <returns>String that represents the current node.</returns>
        public override string ToString()
        {
            var stringWriter = new IO.StringWriter();
            this.ToTextWriter(stringWriter, null);
            return stringWriter.ToString();
        }
    }
}