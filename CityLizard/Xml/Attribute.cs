namespace CityLizard.Xml
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;
    using X = System.Xml;

    using System.Linq;
    using Extension;

    /// <summary>
    /// Represents an XML attribute.
    /// </summary>
    public sealed class Attribute : CharacterData, IAttribute
    {
        internal Attribute(string namespace_, string name, string value)
            : base(value)
        {
            this.Namespace = namespace_;
            this.Name = name;
        }

        /// <summary>
        /// The namespace.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Writes the attribute to the XML writer.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="parentNamespace">Ignored.</param>
        public override void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace)
        {
            writer.WriteAttributeString(this.Name, this.Value);
        }

    }
}