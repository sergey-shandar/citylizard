namespace CityLizard.Xml.Extension
{
    using IO = System.IO;
    using X = System.Xml;

    /// <summary>
    /// Node interface extensions. <see cref="CityLizard.Xml.INode"/>
    /// </summary>
    public static class INodeExtension
    {
        /// <summary>
        /// Writes the node to the text writer.
        /// </summary>
        /// <param name="this_">The node.</param>
        /// <param name="textWriter">The text writer.</param>
        /// <param name="parentNamespace">Parent namespace.</param>
        public static void ToTextWriter(
            this INode this_, IO.TextWriter textWriter, string parentNamespace)
        {
            using (var xmlWriter = new X.XmlTextWriter(textWriter))
            {
                xmlWriter.WriteNode(this_, parentNamespace);
            }
        }
    }
}
