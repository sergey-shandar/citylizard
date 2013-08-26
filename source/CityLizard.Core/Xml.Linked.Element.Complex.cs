namespace CityLizard.Xml.Linked.Element
{
    using C = System.Collections.Generic;
    using X = System.Xml;

    using System.Linq;

    /// <summary>
    /// The complex XML element.
    /// </summary>
    public abstract class Complex: Element
    {
        /// <summary>
        /// The attribute list class.
        /// </summary>
        protected class AttributeList
        {
            /// <summary>
            /// The list of required attributes.
            /// </summary>
            public C.List<Attribute> Required = new C.List<Attribute>();

            /// <summary>
            /// The list of optional attributes.
            /// </summary>
            public C.List<Attribute> Optional = new C.List<Attribute>();

            /// <summary>
            /// The attributes.
            /// </summary>
            public C.IEnumerable<Attribute> All()
            {
                return this.
                    Required.
                    Concat(this.Optional.Where(a => a.Value != null));
            }
        }

        /// <summary>
        /// The attribute list.
        /// </summary>
        protected AttributeList A;

        /// <summary>
        /// Sets up the implementation, QName and attributes.
        /// </summary>
        /// <param name="implementation">The implementation.</param>
        /// <param name="namespace">The namespace.</param>
        /// <param name="name">The local name.</param>
        protected override void SetUp(
            Implementation implementation, string @namespace, string name)
        {
            base.SetUp(implementation, @namespace, name);
            this.A = new AttributeList();
        }

        /// <summary>
        /// Adds a required attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        protected void AddRequiredAttribute(string name, string value)
        {
            this.A.Required.Add(new Attribute(name, value));
        }

        /// <summary>
        /// Adds an optional attribute.
        /// </summary>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        protected void AddOptionalAttribute(string name, string value)
        {
            this.A.Optional.Add(new Attribute(name, value));
        }

        /// <summary>
        /// The XML attributes.
        /// </summary>
        public C.IEnumerable<Attribute> Attributes 
        { 
            get 
            {
                return this.A.All();
            } 
        }

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
            foreach (var a in this.Attributes)
            {
                a.WriteTo(writer);
            }
        }
    }
}
