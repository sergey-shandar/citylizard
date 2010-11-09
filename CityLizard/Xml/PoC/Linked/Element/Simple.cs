namespace CityLizard.Xml.PoC.Linked.Element
{
    /// <summary>
    /// The simple XML element.
    /// </summary>
    public abstract class Simple: Element, ICharacterData
    {
        /// <summary>
        /// Saves the simple element to the specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public override void WriteTo(System.Xml.XmlWriter writer)
        {
            this.WriteStartTo(writer);
            writer.WriteString(Value);
            writer.WriteFullEndElement();
        }

        /// <summary>
        /// The value of the element.
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}
