namespace CityLizard.Xml.Linked
{
    using S = System;

    /// <summary>
    /// Represents the content of an XML comment.
    /// </summary>
    public sealed class Comment: LinkedNode, ICharacterData
    {
        private string V;

        /// <summary>
        /// The general comment error.
        /// </summary>
        public abstract class CommentError : Error
        {
            /// <summary>
            /// Initialize the error.
            /// </summary>
            /// <param name="message">The error message.</param>
            protected CommentError(string message)
                : base(message)
            {
            }
        }

        /// <summary>
        /// Error: for compatibility, the string " -- " (double-hyphen) 
        /// must not occur within comments.
        /// </summary>
        public sealed class DoubleHyphenError : CommentError
        {
            /// <summary>
            /// Initialize the error.
            /// </summary>
            public DoubleHyphenError(): 
                base(
                    "the string \" -- \" (double-hyphen) must not occur within comments.")
            {
            }
        }

        /// <summary>
        /// Error: the XML grammar does not allow a comment ending in --->
        /// </summary>
        public sealed class InvalidEndingError : CommentError
        {
            /// <summary>
            /// Initialize the error.
            /// </summary>
            public InvalidEndingError(): 
                base("the XML grammar does not allow a comment ending in --->")
            {
            }
        }

        /// <summary>
        /// Escaping current comment.
        /// 
        /// Extensible Markup Language (XML) 1.0 (Fifth Edition)
        /// 2.5 Comments
        /// (http://www.w3.org/TR/2008/REC-xml-20081126/#sec-comments)
        /// 
        /// For compatibility, the string " -- " (double-hyphen) must not occur 
        /// within comments.
        /// 
        /// Note that the grammar does not allow a comment ending in --->
        /// </summary>
        private string Escape(string value)
        {
            var result = "";
            while (true)
            {
                var i = value.IndexOf('-');
                if (i == -1)
                {
                    result += Value;
                    break;
                }
                else if (i == value.Length - 1)
                {
                    // Error:
                    // the XML grammar does not allow a comment ending in --->
                    this.HandleError(new InvalidEndingError());
                    // Fixing the error.
                    result += value + ' ';
                    break;
                }
                else
                {
                    ++i;
                    result += value.Substring(0, i);
                    value = value.Substring(i);
                    if (value[0] == '-')
                    {
                        // error: 
                        // For compatibility, the string " -- " (double-hyphen) 
                        // must not occur within comments.
                        this.HandleError(new DoubleHyphenError());
                        // Fixing the error.
                        result += ' ';
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// The comment text.
        /// </summary>
        public string Value 
        { 
            get { return this.V; }
            set { this.V = this.Escape(value); }
        }

        /// <summary>
        /// Saves the current comment to the specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public override void WriteTo(System.Xml.XmlWriter writer)
        {
            writer.WriteComment(this.V);
        }
    }
}
