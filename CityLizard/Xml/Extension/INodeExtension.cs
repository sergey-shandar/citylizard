namespace CityLizard.Xml.Extension
{
    using IO = System.IO;
    using X = System.Xml;

    public static class INodeExtension
    {
        public static void ToTextStream(
            this INode this_, IO.TextWriter textWriter)
        {
            using (var xmlWriter = new X.XmlTextWriter(textWriter))
            {
                xmlWriter.WriteNode(this_, null);
            }
        }
    }
}
