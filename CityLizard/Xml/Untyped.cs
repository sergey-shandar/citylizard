namespace CityLizard.Xml
{
    using System.Xml.Linq;

    public class Untyped
    {
        public string Namespace { get; private set; }

        public Untyped(string namespace_)
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
            }
        }

        public static XName N(string localName, string namespace_)
        {
            return XName.Get(localName, namespace_);
        }

        public static XName N(string expandedName)
        {
            return XName.Get(expandedName);
        }

        public static T.A A(XName N, string value)
        {
            return new T.A(N, value);
        }

        public static T.A A(string expandedName, string value)
        {
            return A(N(expandedName), value);
        }

        public static T.A A(string localName, string namespace_, string value)
        {
            return A(N(localName, namespace_), value);
        }

        public static T.C C(string Comment)
        {
            return new T.C(Comment);
        }

        public static T.X X(XName N, params T.A[] A)
        {
            return new T.X(N, A);
        }

        public static T.X X(string expandedName, params T.A[] A)
        {
            return X(N(expandedName), A);
        }

        public static T.X X(string localName, string namespace_, params T.A[] A)
        {
            return X(N(localName, namespace_), A);
        }

        public static T.E E(XName N, params T.A[] A)
        {
            return new T.E(N, A);
        }

        public static T.E E(string expandedName, params T.A[] A)
        {
            return E(N(expandedName), A);
        }

        public static T.E E(string localName, string namespace_, params T.A[] A)
        {
            return E(N(localName, namespace_), A);
        }
    }
}
