namespace CityLizard.Xml.PoC.Linked.Element
{
    using X = System.Xml;

    /// <summary>
    /// The empty XML element.
    /// </summary>
    public abstract class Empty: Complex
    {
        /// <summary>
        /// Saves the empty element to the specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public override void WriteTo(X.XmlWriter writer)
        {
            this.WriterStartAndAttributesTo(writer);
            writer.WriteEndElement();
        }
    }
}
