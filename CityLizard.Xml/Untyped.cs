namespace CityLizard.Xml
{
    using System.Xml.Linq;

    /// <summary>
    /// Untyped XML builder.
    /// </summary>
    public class Untyped
    {
        /// <summary>
        /// Namespace.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="namespace_">document namespace</param>
        public Untyped(string namespace_ = "")
        {
            this.Namespace = namespace_;
        }

        /// <summary>
        /// XML type builder.
        /// </summary>
        public class T
        {
            /// <summary>
            /// Attribute.
            /// </summary>
            public class A : XAttribute
            {
                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="N">attribute name</param>
                /// <param name="value">attribute value</param>
                public A(XName N, string value): base(N, value)
                {
                }
            }

            /// <summary>
            /// Comment.
            /// </summary>
            public class C : XComment
            {
                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="Comment">comment text.</param>
                public C(string Comment): base(Comment)
                {
                }
            }

            /// <summary>
            /// Element base.
            /// </summary>
            public class Element : XElement
            {
                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="N">element name</param>
                /// <param name="Content">content</param>
                /// <param name="A">attributes</param>
                public Element(XName N, string Content, params XAttribute[] A) :
                    base(N, Content)
                {
                    foreach (var a in A)
                    {
                        this.SetAttributeValue(a.Name, a.Value);
                    }
                }
            }

            /// <summary>
            /// Empty element.
            /// </summary>
            public class E : Element
            {
                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="N">element name</param>
                /// <param name="A">attributes</param>
                public E(XName N, params XAttribute[] A) :
                    base(N, null, A)
                {
                }
            }

            /// <summary>
            /// Element.
            /// </summary>
            public class X : Element
            {
                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="N">name</param>
                /// <param name="A">attributes</param>
                public X(XName N, params XAttribute[] A): 
                    base(N, "", A)
                {
                }

                /// <summary>
                /// Adds a text to the element.
                /// </summary>
                /// <param name="Text">text</param>
                /// <returns>itself</returns>
                public X this[string Text]
                {
                    get
                    {
                        this.Add(Text);
                        return this;
                    }
                }

                /// <summary>
                /// Adds a comment to the elemnt.
                /// </summary>
                /// <param name="C">comment</param>
                /// <returns>itself</returns>
                public X this[C C]
                {
                    get
                    {
                        this.Add(C);
                        return this;
                    }
                }

                /// <summary>
                /// Adds an empty element as a child. 
                /// </summary>
                /// <param name="E">a child</param>
                /// <returns>itself</returns>
                public X this[E E]
                {
                    get
                    {
                        this.Add(E);
                        return this;
                    }
                }

                /// <summary>
                /// Adds a child element.
                /// </summary>
                /// <param name="X">a child element.</param>
                /// <returns>itself</returns>
                public X this[X X]
                {
                    get
                    {
                        this.Add(X);
                        return this;
                    }
                }

                /// <summary>
                /// Adds a child element.
                /// </summary>
                /// <param name="Element"></param>
                /// <returns></returns>
                public X this[Element Element]
                {
                    get
                    {
                        this.Add(Element);
                        return this;
                    }
                }
            }
        }

        /// <summary>
        /// local name.
        /// </summary>
        /// <param name="localName"></param>
        /// <returns></returns>
        public XName LocalName(string localName)
        {
            return XName.Get(localName, this.Namespace);
        }

        /// <summary>
        /// global name.
        /// </summary>
        /// <param name="localName"></param>
        /// <returns></returns>
        public static XName GlobalName(string localName)
        {
            return XName.Get(localName, string.Empty);
        }

        /// <summary>
        /// Attribute.
        /// </summary>
        /// <param name="globalName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T.A A(string globalName, string value)
        {
            return new T.A(GlobalName(globalName), value);
        }

        /// <summary>
        /// Local attribute.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T.A L(string localName, string value)
        {
            return new T.A(LocalName(localName), value);
        }

        /// <summary>
        /// comment.
        /// </summary>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public static T.C C(string Comment)
        {
            return new T.C(Comment);
        }

        /// <summary>
        /// Local element.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public T.X X(string localName, params T.A[] A)
        {
            return new T.X(this.LocalName(localName), A);
        }

        /// <summary>
        /// Local empty element.
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public T.E E(string localName, params T.A[] A)
        {
            return new T.E(this.LocalName(localName), A);
        }
    }
}
