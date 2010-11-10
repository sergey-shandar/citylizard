namespace CityLizard.Xml.Linked.Element
{
    using X = System.Xml;

    /// <summary>
    /// The abstract XML element.
    /// </summary>
    public abstract class Element: LinkedNode, IQName
    {
        /// <summary>
        /// Sets up the implementation and QName.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="qName">The QName.</param>
        protected void SetUp(Implementation implementation, QName qName)
        {
            this.Implementation = implementation;
            this.QName = qName;
        }

        /// <summary>
        /// Sets up the implementation and QName.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="namespace">The QName namespace.</param>
        /// <param name="name">The QName name.</param>
        protected virtual void SetUpNew(
            Implementation implementation, string @namespace, string name)
        {
            this.SetUp(implementation, new QName(@namespace, name));
        }

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
