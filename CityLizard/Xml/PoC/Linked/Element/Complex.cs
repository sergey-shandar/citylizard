namespace CityLizard.Xml.PoC.Linked.Element
{
    using C = System.Collections.Generic;
    using X = System.Xml;

    /// <summary>
    /// The complex XML element.
    /// </summary>
    public abstract class Complex: Element
    {
        /// <summary>
        /// The list of attributes.
        /// </summary>
        protected readonly C.List<Attribute> A = new C.List<Attribute>();

        /// <summary>
        /// The XML attributes.
        /// </summary>
        public C.IEnumerable<Attribute> Attributes { get { return this.A; } }

        /// <summary>
        /// Writes the start of the element and its attributes to the specified
        /// System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        protected void WriterStartAndAttributesTo(X.XmlWriter writer)
        {
            this.WriteStartTo(writer);
            foreach (var a in this.A)
            {
                a.WriteTo(writer);
            }
        }
    }
}
