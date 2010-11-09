namespace CityLizard.Xml.Linked.Element
{
    using X = System.Xml;

    /// <summary>
    /// The abstract XML element.
    /// </summary>
    public abstract class Element: LinkedNode, IQName
    {
        /// <summary>
        /// The QName of the element.
        /// </summary>
        public QName QName { get; protected set; }

        /// <summary>
        /// Writes the start of the element to the specified 
        /// System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        protected void WriteStartTo(X.XmlWriter writer)
        {
            var n = this.QName;
            if (
                this.Parent == null ||
                this.Parent.QName.Namespace != n.Namespace)
            {
                writer.WriteStartElement(n.LocalName, n.Namespace);
            }
            else
            {
                writer.WriteStartElement(n.LocalName);
            }
        }
    }
}
