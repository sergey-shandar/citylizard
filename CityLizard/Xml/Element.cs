namespace CityLizard.Xml
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    public class Element : Node, IElement
    {
        public class Header
        {
            public readonly string Ns;
            public readonly string Name;
            public readonly bool IsEmpty;

            public readonly C.IList<IAttribute> RequiredAttributeList =
                new C.List<IAttribute>();

            public void AddRequiredAttribute(string name, string value)
            {
                this.RequiredAttributeList.Add(name, value);
            }

            public readonly C.IList<IAttribute> OptionalAttributeList =
                new C.List<IAttribute>();

            public void AddOptionalAttribute(string name, string value)
            {
                this.OptionalAttributeList.Add(name, value);
            }

            public C.IEnumerable<IAttribute> AttributeList
            {
                get
                {
                    return this.RequiredAttributeList.Concat(
                        this.OptionalAttributeList.Where(a => a.Value != null));
                }
            }

            public Header(
                string ns, 
                string name, 
                bool isEmpty)
            {
                this.Ns = ns;
                this.Name = name;
                this.Name = name;
                this.IsEmpty = isEmpty;
            }
        }

        private readonly Element Part0 = null;

        protected readonly C.IList<INode> Part1 =
            new C.List<INode>();

        private readonly Header H;

        public Element(Header h)
        {
            this.H = h;
        }

        public Element(Element part0)
            : this(part0.H)
        {
            this.Part0 = part0;
        }

        public Element(Element part0, Element child): this(part0)
        {
            this.AddElement(child);
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/Aa691340
        /// C# 7.4.3 Function member invocation
        /// ...
        /// The value of E is checked to be valid. If the value of E is null, a 
        /// System.NullReferenceException is thrown and no further steps are 
        /// executed.
        /// ...
        /// </summary>
        public void NotNull()
        {
        }

        protected void AddElement(Element e)
        {
            this.Part1.Add(e);
        }

        protected void AddText(string value)
        {
            this.Part1.Add(new Text(value));
        }

        protected void AddComment(IComment comment)
        {
            this.Part1.Add(comment);
        }

        protected void AddComment(string value)
        {
            this.AddComment(new Comment(value));
        }

        public string Namespace
        {
            get { return this.H.Ns; }
        }

        public string Name
        {
            get { return this.H.Name; }
        }

        /// <summary>
        /// {attribute}
        /// </summary>
        public C.IEnumerable<IAttribute> AttributeList
        {
            get
            {
                return this.H.AttributeList;
            }
        }

        /// <summary>
        /// {element|text|comment}
        /// </summary>
        public C.IEnumerable<INode> ContentList
        {
            get
            {
                return this.Part0 == null ?
                    this.Part1 :
                    this.Part0.ContentList.Concat(this.Part1);
            }
        }

        /// <summary>
        /// {attribute|element|text|comment}
        /// </summary>
        public C.IEnumerable<INode> NodeList
        {
            get
            {
                return this.AttributeList.Cast<INode>().
                    Concat(this.ContentList);
            }
        }

        /// <summary>
        /// http://www.w3.org/TR/xhtml1/#guidelines">. C. HTML 
        /// Compatibility Guidelines.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parentNamespace"></param>
        public override void ToTextWriter(
            IO.TextWriter writer, string parentNamespace)
        {
            writer.Write('<');
            writer.Write(this.H.Name);
            if (parentNamespace != this.H.Ns)
            {
                writer.WriteAttribute("xmlns", this.H.Ns);
            }
            writer.WriteList(this.H.AttributeList, this.H.Ns);
            if (this.H.IsEmpty)
            {
                // C.2. Empty Elements
                // 
                // Include a space before the trailing / and > of empty 
                // elements, e.g. <br />, <hr /> and <img src="karen.jpg" 
                // alt="Karen" />. Also, use the minimized tag syntax for empty 
                // elements, e.g. <br />, as the alternative syntax <br></br> 
                // allowed by XML gives uncertain results in many existing user 
                // agents.
                writer.Write(" />");
            }
            else
            {
                // C.3. Element Minimization and Empty Element Content
                //
                // Given an empty instance of an element whose content model is 
                // not EMPTY (for example, an empty title or paragraph) do not 
                // use the minimized form (e.g. use <p> </p> and not <p />).
                writer.Write('>');
                writer.WriteList(this.ContentList, this.H.Ns);
                writer.Write("</");
                writer.Write(this.H.Name);
                writer.Write('>');
            }
        }

        public override void ToXmlWriter(
            S.Xml.XmlWriter writer, string parentNamespace)
        {
            if (parentNamespace != this.H.Ns)
            {
                writer.WriteStartElement(this.H.Name, this.H.Ns);
            }
            else
            {
                writer.WriteStartElement(this.H.Name);
            }
            writer.WriteList(this.H.AttributeList, this.H.Ns);
            // writer.
        }
    }
}