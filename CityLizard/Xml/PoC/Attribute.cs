namespace CityLizard.Xml.PoC
{
    /// <summary>
    /// Represents an XML attribute.
    /// </summary>
    public abstract class Attribute: Node, IQName, ICharacterData
    {
        /// <summary>
        /// Saves the current attribute the specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public override void WriteTo(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString(this.QName.LocalName, this.Value);
        }

        /// <summary>
        /// The QName of the attribute.
        /// </summary>
        public QName QName
        {
            get;
            protected set;
        }

        /// <summary>
        /// The value of the attribute.
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}
