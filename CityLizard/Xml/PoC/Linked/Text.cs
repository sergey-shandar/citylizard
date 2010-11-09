namespace CityLizard.Xml.PoC.Linked
{
    using S = System;

    /// <summary>
    /// Represents the text node.
    /// </summary>
    public sealed class Text: LinkedNode, ICharacterData
    {
        /// <summary>
        /// Saves the current text node to the specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public override void WriteTo(S.Xml.XmlWriter writer)
        {
            writer.WriteString(this.Value);
        }

        /// <summary>
        /// The value of the text node.
        /// </summary>
        public string Value { get; set; }
    }
}
