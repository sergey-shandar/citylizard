namespace CityLizard.Xml
{
    /// <summary>
    /// Represents an XML attribute.
    /// </summary>
    public class Attribute: Node, IQName, ICharacterData
    {
        /// <summary>
        /// Saves the current attribute to the specified System.Xml.XmlWriter.
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

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public Attribute(string name, string value)
        {
            this.QName = new QName(null, name);
            this.Value = value;
        }
    }
}
