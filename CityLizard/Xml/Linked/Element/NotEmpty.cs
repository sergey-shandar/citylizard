namespace CityLizard.Xml.Linked.Element
{
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// The not empty XML element.
    /// </summary>
    public abstract class NotEmpty: Complex
    {
        /// <summary>
        /// The list of linked nodes, part 0.
        /// </summary>
        protected C.List<LinkedNode> Part0;

        /// <summary>
        /// The list of linked nodes, part 1.
        /// </summary>
        private readonly C.List<LinkedNode> Part1 = new C.List<LinkedNode>();

        /// <summary>
        /// Adds the linked node.
        /// </summary>
        /// <param name="n">The linked node.</param>
        protected void AddLinkedNode(LinkedNode n)
        {
            this.Part1.Add(this.InitChild(n));
        }

        /// <summary>
        /// The linked nodes.
        /// </summary>
        public C.IEnumerable<LinkedNode> LinkedNodes 
        { 
            get 
            { 
                return 
                    this.Part0 == null ? this.Part1: this.Part0.Concat(this.Part1);
            } 
        }

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
