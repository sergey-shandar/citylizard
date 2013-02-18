namespace CityLizard.Xml
{
    using System.Xml.Linq;

    public class Untyped
    {
        public string Namespace { get; private set; }

        public Untyped(string namespace_ = "")
        {
            this.Namespace = namespace_;
        }

        public class T
        {
            public class A : XAttribute
            {
                public A(XName N, string value): base(N, value)
                {
                }
            }

            public class C : XComment
            {
                public C(string Comment): base(Comment)
                {
                }
            }

            public class Element : XElement
            {
                public Element(XName N, string Content, params XAttribute[] A) :
                    base(N, Content)
                {
                    foreach (var a in A)
                    {
                        this.SetAttributeValue(a.Name, a.Value);
                    }
                }
            }

            public class E : Element
            {
                public E(XName N, params XAttribute[] A) :
                    base(N, null, A)
                {
                }
            }

            public class X : Element
            {
                public X(XName N, params XAttribute[] A): 
                    base(N, "", A)
                {
                }

                public X this[string Text]
                {
                    get
                    {
                        this.Add(Text);
                        return this;
                    }
                }

                public X this[C C]
                {
                    get
                    {
                        this.Add(C);
                        return this;
                    }
                }

                public X this[E E]
                {
                    get
                    {
                        this.Add(E);
                        return this;
                    }
                }

                public X this[X X]
                {
                    get
                    {
                        this.Add(X);
                        return this;
                    }
                }

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

        public XName LocalName(string localName)
        {
            return XName.Get(localName, this.Namespace);
        }

        public static XName GlobalName(string localName)
        {
            return XName.Get(localName, string.Empty);
        }

        public static T.A A(string globalName, string value)
        {
            return new T.A(GlobalName(globalName), value);
        }

        public T.A L(string localName, string value)
        {
            return new T.A(LocalName(localName), value);
        }

        public static T.C C(string Comment)
        {
            return new T.C(Comment);
        }

        public T.X X(string localName, params T.A[] A)
        {
            return new T.X(this.LocalName(localName), A);
        }

        public T.E E(string localName, params T.A[] A)
        {
            return new T.E(this.LocalName(localName), A);
        }
    }
}
