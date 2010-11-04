namespace CityLizard.Xml
{
    using IO = System.IO;
    using X = System.Xml;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    /// <summary>
    /// Represents the content of an XML comment.
    /// </summary>
    public sealed class Comment : CharacterData, IComment
    {
        /// <summary>
        /// Initializes a new instance of the Comment class.
        /// </summary>
        /// <param name="value">
        /// The content of the comment.
        /// </param>
        public Comment(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Writes the comment to the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="parentNamespace">Ignored.</param>
        public override void ToTextWriter(
            IO.TextWriter writer, string parentNamespace)
        {
            writer.Write("<!--");
            writer.WriteText(this.Value);
            writer.Write("-->");
        }

        public override void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace)
        {
            writer.WriteComment(this.Value);
        }
    }
}