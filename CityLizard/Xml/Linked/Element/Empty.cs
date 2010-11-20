namespace CityLizard.Xml.Linked.Element
{
    using X = System.Xml;

    /// <summary>
    /// The empty XML element.
    /// </summary>
    public abstract class Empty: Complex
    {
        /// <summary>
        /// Returns Type.Empty.
        /// </summary>
        public override Type Type
        {
            get { return Type.Empty; }
        }

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
