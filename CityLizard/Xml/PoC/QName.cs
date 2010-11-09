namespace CityLizard.Xml.PoC
{
    /// <summary>
    /// The XML QName (qualified name) defines a valid identifier for elements 
    /// and attributes.
    /// </summary>
    public struct QName
    {
        /// <summary>
        /// The XML namespace.
        /// </summary>
        public string Namespace;

        /// <summary>
        /// The XML local name.
        /// </summary>
        public string LocalName;

        /// <summary>
        /// Creates an XML QName.
        /// </summary>
        /// <param name="namespace">The XML namespace.</param>
        /// <param name="localName">The XML local name.</param>
        public QName(string @namespace, string localName)
        {
            this.Namespace = @namespace;
            this.LocalName = localName;
        }

        /// <summary>
        /// Creates an XML QName.
        /// </summary>
        /// <param name="localName">The XML local name.</param>
        public QName(string localName): this(null, localName)
        {
        }
    }
}
