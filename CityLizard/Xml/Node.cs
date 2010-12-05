namespace CityLizard.Xml
{
    using X = System.Xml;
    using IO = System.IO;
    using C = System.Collections.Generic;
    using S = System;

    /// <summary>
    /// Represents a single node in the XML document.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// The general node error.
        /// </summary>
        public abstract class Error : S.Exception
        {
            /// <summary>
            /// Initialize the error.
            /// </summary>
            /// <param name="message">The error message.</param>
            protected Error(string message)
                : base(message)
            {
            }
        }

        /// <summary>
        /// The replaceable error handler.
        /// </summary>
        public Implementation Implementation { get; set; }

        /// <summary>
        /// Handles the new error.
        /// </summary>
        /// <param name="e">The new error.</param>
        protected void HandleError(Error e)
        {
            this.Implementation.ErrorHandler(this, e);
        }

        /// <summary>
        /// The parent element.
        /// </summary>
        public Linked.Element.Complex Parent 
        { 
            get; 
            protected set; 
        }
        
        /// <summary>
        /// Initializes the child node.
        /// </summary>
        /// <typeparam name="T">The child type.</typeparam>
        /// <param name="child">The child.</param>
        /// <returns>The child.</returns>
        protected T InitChild<T>(T child)
            where T: Node
        {
            child.Parent = this.Parent;
            return child;
        }

        /// <summary>
        /// When overridden in a derived class, saves the current node to the 
        /// specified System.Xml.XmlWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.Xml.XmlWriter to which you want to save.
        /// </param>
        public abstract void WriteTo(X.XmlWriter writer);

        /// <summary>
        /// Saves the current node to the specified System.IO.TextWriter.
        /// </summary>
        /// <param name="writer">
        /// The System.IO.TextWriter to which you want to save.
        /// </param>
        public void WriteTo(IO.TextWriter writer)
        {
            using(var xmlWriter = X.XmlWriter.Create(writer))
            {
                this.WriteTo(xmlWriter);
            }
        }

        /// <summary>
        /// Returns a System.String that represents the current node. 
        /// </summary>
        /// <returns>A System.String that represents the current node.</returns>
        public override string ToString()
        {
            var writer = new IO.StringWriter();
            this.WriteTo(writer);
            return writer.ToString();
        }
    }
}
