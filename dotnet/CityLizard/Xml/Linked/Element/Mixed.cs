namespace CityLizard.Xml.Linked.Element
{
    /// <summary>
    /// The mixed XML element.
    /// </summary>
    public abstract class Mixed: NotEmpty
    {
        /// <summary>
        /// Returns Type.Mixed.
        /// </summary>
        public override Type Type
        {
            get { return Type.Mixed; }
        }

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
        public void Add(string text)
        {
            var t = new Text();
            this.Add(t);
            t.Value = text;
        }
    }
}
