namespace CityLizard.Xml.PoC.Linked.Element
{
    using C = System.Collections.Generic;

    /// <summary>
    /// The not empty XML element.
    /// </summary>
    public abstract class NotEmpty: Complex
    {
        /// <summary>
        /// The list of linked nodes.
        /// </summary>
        private readonly C.List<LinkedNode> N = new C.List<LinkedNode>();

        /// <summary>
        /// Adds the linked node.
        /// </summary>
        /// <param name="n">The linked node.</param>
        protected void AddLinkedNode(LinkedNode n)
        {
            this.N.Add(this.InitChild(n));
        }

        /// <summary>
        /// The linked nodes.
        /// </summary>
        public C.IEnumerable<LinkedNode> LinkedNodes { get { return this.N; } }

        /// <summary>
        /// Adds the comment node.
        /// </summary>
        /// <param name="comment">The comment node.</param>
        public void Add(Comment comment)
        {
            this.AddLinkedNode(comment);
        }

        /// <summary>
        /// Adds the comment.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public void AddComment(string comment)
        {
            var c = new Comment();
            this.Add(c);
            c.Value = comment;
        }

        /// <summary>
        /// Saves the element to the specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public override void WriteTo(System.Xml.XmlWriter writer)
        {
            this.WriterStartAndAttributesTo(writer);
            foreach (var n in this.N)
            {
                n.WriteTo(writer);
            }
            writer.WriteFullEndElement();
        }
    }
}
