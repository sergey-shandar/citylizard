namespace CityLizard.Xml.Extension
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// TextWriter extension methods.
    /// </summary>
    public static class TextWriterExtension
    {
        /// <summary>
        /// Writes the node to the current writer instance.
        /// </summary>
        /// <param name="this_">The current writer instance.</param>
        /// <param name="node">The node to writes from.</param>
        /// <param name="parentNamespace">Parent namespace.</param>
        public static void WriteNode(
            this IO.TextWriter this_, INode node, string parentNamespace)
        {
            node.ToTextWriter(this_, parentNamespace);
        }
    }
}
