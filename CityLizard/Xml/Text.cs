namespace CityLizard.Xml
{
    using IO = System.IO;
    using X = System.Xml;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;
    
    /// <summary>
    /// Represents the text content of an element.
    /// </summary>
    public sealed class Text : CharacterData, IText
    {
        /// <summary>
        /// Initializes a new instance of the XmlText class.
        /// </summary>
        /// <param name="value">
        /// The content of the node.
        /// </param>
        public Text(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Writes the node to the XML writer.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="parentNamespace">Parent namespace.</param>
        public override void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace)
        {
            writer.WriteString(this.Value);
        }
    }
}