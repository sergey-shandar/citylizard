namespace CityLizard.Xml.Extension
{
    using X = System.Xml;
    using C = System.Collections.Generic;

    /// <summary>
    /// XML writer extension. <see cref="System.Xml.XmlWriter"/>.
    /// </summary>
    public static class XmlWriterExtension
    {
        /// <summary>
        /// Writes the node to the current writer instance.
        /// </summary>
        /// <param name="this_">The current writer instance.</param>
        /// <param name="node">The node to writes from.</param>
        /// <param name="parentNamespace">Parent namespace.</param>
        public static void WriteNode(
            this X.XmlWriter this_, INode node, string parentNamespace)
        {
            node.ToXmlWriter(this_, parentNamespace);
        }

        /// <summary>
        /// Writes the list of nodes to the current writer instance.
        /// </summary>
        /// <typeparam name="I">The type of nodes.</typeparam>
        /// <param name="this_">The current writer instance.</param>
        /// <param name="list">The list of nodes.</param>
        /// <param name="parentNamespace">Parent namespace.</param>
        public static void WriteList<I>(
            this X.XmlWriter this_,
            C.IEnumerable<I> list,
            string parentNamespace)
            where I : INode
        {
            foreach (var i in list)
            {
                this_.WriteNode(i, parentNamespace);
            }
        }
    }
}
