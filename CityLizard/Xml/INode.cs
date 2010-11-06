namespace CityLizard.Xml
{
    using IO = System.IO;
    using X = System.Xml;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// The node interface represents a single node in the XML document tree.
    /// </summary>
    public interface INode
    {
        /*
        /// <summary>
        /// Writes the node to the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="parentNamespace">
        /// true to write without xmlns attribute; otherwise, false.
        /// </param>
        void ToTextWriter(IO.TextWriter writer, string parentNamespace);
        */

        /// <summary>
        /// Writes the node to the XML writer.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="parentNamespace">
        /// true to write without xmlns attribute; otherwise, false.
        /// </param>
        void ToXmlWriter(X.XmlWriter writer, string parentNamespace);
    }
}