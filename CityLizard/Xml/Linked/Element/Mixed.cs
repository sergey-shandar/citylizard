namespace CityLizard.Xml.Linked.Element
{
    /// <summary>
    /// The mixed XML element.
    /// </summary>
    public abstract class Mixed: NotEmpty
    {
        /// <summary>
        /// Adds the text node.
        /// </summary>
        /// <param name="text">The text node.</param>
        public void Add(Text text)
        {
            this.AddLinkedNodeOptional(text);
        }

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void AddText(string text)
        {
            var t = new Text();
            this.Add(t);
            t.Value = text;
        }
    }
}
