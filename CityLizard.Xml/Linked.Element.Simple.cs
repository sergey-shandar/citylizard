namespace CityLizard.Xml.Linked.Element
{
    /// <summary>
    /// The simple XML element.
    /// </summary>
    public abstract class Simple: Element, ICharacterData
    {
        /// <summary>
        /// Set up the simple element.
        /// </summary>
        /// <param name="implementation"></param>
        /// <param name="namespace"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void SetUp(
            Implementation implementation, 
            string @namespace, 
            string name, 
            string value)
        {
            base.SetUp(implementation, @namespace, name);
            this.Value = value;
        }

        /// <summary>
        /// Returns Type.Simple.
        /// </summary>
        public override Type Type
        {
            get { return Type.Simple; }
        }

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
