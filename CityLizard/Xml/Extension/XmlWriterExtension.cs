namespace CityLizard.Xml.Extension
{
    using X = System.Xml;
    using C = System.Collections.Generic;

    public static class XmlWriterExtension
    {
        public static void WriteNode(
            this X.XmlWriter this_, INode node, string parentNamespace)
        {
            node.ToXmlWriter(this_, parentNamespace);
        }

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
