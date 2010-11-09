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
        /// The general not empty element error.
        /// </summary>
        public abstract class NotEmptyError: Error
        {
            /// <summary>
            /// Initialize the error.
            /// </summary>
            /// <param name="message">The error message.</param>
            protected NotEmptyError(string message)
                : base(message)
            {
            }
        }

        /// <summary>
        /// The list of linked nodes, part 0.
        /// </summary>
        private C.IEnumerable<LinkedNode> Part0;

        /// <summary>
        /// The list of linked nodes, part 1.
        /// </summary>
        private readonly C.List<LinkedNode> Part1 = new C.List<LinkedNode>();

        /// <summary>
        /// Sets the part 0 of the element.
        /// </summary>
        /// <param name="part0">The part 0.</param>
        protected void SetUp(NotEmpty part0)
        {
            this.QName = part0.QName;
            this.A = part0.A;
            this.Part0 = part0.LinkedNodes;
        }
        
        /// <summary>
        /// Sets the part 0 of the element and the child.
        /// </summary>
        /// <param name="part0">The part 0.</param>
        /// <param name="child">The child.</param>
        protected void SetUp(NotEmpty part0, Element child)
        {
            this.SetUp(part0);
            this.AddLinkedNodeRequired(child);
        }

        /// <summary>
        /// Adds the linked node.
        /// </summary>
        /// <param name="n">The linked node.</param>
        protected bool AddLinkedNodeOptional(LinkedNode n)
        {
            var result = n != null;
            if(result)
            {
                this.Part1.Add(this.InitChild(n));
            }
            return result;
        }

        /// <summary>
        /// Error: a required child element can not be null reference.
        /// </summary>
        public sealed class NullChildError : NotEmptyError
        {
            /// <summary>
            /// Initialize the error.
            /// </summary>
            public NullChildError()
                : base("A required child element can not be null reference.")
            {
            }
        }

        /// <summary>
        /// Adds the reqiured linked element.
        /// </summary>
        /// <param name="n">The linked element.</param>
        protected void AddLinkedNodeRequired(Element n)
        {
            if (!this.AddLinkedNodeOptional(n))
            {
                this.HandleError(new NullChildError());
            }
        }

        /// <summary>
        /// The linked nodes.
        /// </summary>
        public C.IEnumerable<LinkedNode> LinkedNodes 
        { 
            get 
            { 
                return 
                    this.Part0 == null ? 
                        this.Part1: 
                        this.Part0.Concat(this.Part1);
            } 
        }

        /// <summary>
        /// Adds the comment node.
        /// </summary>
        /// <param name="comment">The comment node.</param>
        public void Add(Comment comment)
        {
            this.AddLinkedNodeOptional(comment);
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
            foreach (var n in this.LinkedNodes)
            {
                n.WriteTo(writer);
            }
            writer.WriteFullEndElement();
        }
    }
}
