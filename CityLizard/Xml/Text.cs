namespace CityLizard.Xml
{
    using IO = System.IO;
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
        /// Writes the node to the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="parentNamespace">
        /// true to write without xmlns attribute; otherwise, false.
        /// </param>
        public override void ToTextWriter(
            IO.TextWriter writer, string parentNamespace)
        {
            writer.WriteText(this.Value);
        }
    }
}