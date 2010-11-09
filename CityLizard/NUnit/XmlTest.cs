namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;

    // using Xml.Extension;

    [N.TestFixture]
    public static class XmlTest
    {
        class NotEmpty: Xml.Linked.Element.NotEmpty
        {
            public NotEmpty(Xml.QName h)
            {
                this.QName = h;
            }
        }

        class Empty : Xml.Linked.Element.Empty
        {
            public Empty(Xml.QName h)
            {
                this.QName = h;
            }
        }

        public static void NotEmptyTest(
            string ns, string name, string result)
        {
            var e = new NotEmpty(new Xml.QName(ns, name));
            N.Assert.AreEqual(result, e.ToString());
        }

        public static void EmptyTest(
            string ns, string name, string result)
        {
            var e = new Empty(new Xml.QName(ns, name));
            N.Assert.AreEqual(result, e.ToString());
        }

        [N.Test]
        public static void ElementTest()
        {
            ElementTest(
                "http://example.com/", 
                "main", 
                true, 
                "<main xmlns=\"http://example.com/\" />");
            ElementTest(
                "http://example.com/",
                "main",
                false,
                "<main xmlns=\"http://example.com/\"></main>");
        }

        public static T.html._0 html()
        {
            return new T.html._0(new Xml.ElementBase.Header(
                "http://example.org/", "html"));
        }

        public static T.head._0 head(string id = null)
        {
            var H = new Xml.ElementBase.Header("http://example.org/", "head");
            H.AddOptionalAttribute("id", id);
            return new T.head._0(H);
        }

        public static T.body body()
        {
            return new T.body(new Xml.ElementBase.Header(
                "http://example.org/", "body"));
        }

        public static T.title title()
        {
            return new T.title(new Xml.ElementBase.Header(
                "http://example.org/", "title"));
        }

        public class T
        {
            public class html: Xml.Linked.Element.Mixed
            {
                internal html(_1 part0, body child)
                {
                    this.SetPart0(part0, child);
                }

                public class _0 : Xml.Linked.Element.Mixed
                {
                    internal _0(Xml.ElementBase.Header h): base(h, false)
                    {
                    }

                    public _1 this[head head]
                    {
                        get
                        {
                            return new _1(this, head);
                        }
                    }
                }

                public class _1 : Xml.Linked.Element.Mixed
                {
                    internal _1(Xml.Element part0, Xml.Element child):
                        base(part0, child)
                    {
                    }

                    public html this[body body]
                    {
                        get
                        {
                            body.NotNull();
                            return new html(this, body);
                        }
                    }
                }
            }

            public class head : Xml.Element
            {
                internal head(Xml.Element part0, Xml.Element child):
                    base(part0, child)
                {
                }

                internal head(Xml.Element part0):
                    base(part0)
                {
                }

                public class _0 : Xml.Element
                {
                    internal _0(Xml.Element.Header h) :
                        base(h, false)
                    {
                    }

                    public head this[title title]
                    {
                        get
                        {
                            title.NotNull();
                            return new head(this, title);
                        }
                    }

                    public _0 this[Xml.Comment comment]
                    {
                        get
                        {
                            this.AddComment(comment.Value);
                            return this;
                        }
                    }

                    public static implicit operator head(_0 _0)
                    {
                        _0.NotNull();
                        return new head(_0);
                    }
                }
            }

            public class body : Xml.Element
            {
                internal body(Xml.ElementBase.Header h) :
                    base(h, false)
                {
                }
            }

            public class title: Xml.Element
            {
                internal title(Xml.ElementBase.Header h):
                    base(h, true)
                {
                }
            }
        }

        [N.Test]
        public static void IdeaTest()
        {
            T.html h = html()[head()[title()]][body()];
            N.Assert.AreEqual(
                "<html xmlns=\"http://example.org/\"><head><title /></head><body></body></html>",
                h.ToString());
        }

        [N.Test]
        public static void IdeaTest1()
        {
            T.html h = html()[head("xxx")[title()]][body()];
            N.Assert.AreEqual(
                "<html xmlns=\"http://example.org/\"><head id=\"xxx\"><title /></head><body></body></html>",
                h.ToString());
        }

        [N.Test]
        public static void IdeaTest2()
        {
            T.html h = html()[head("yyy")][body()];
            N.Assert.AreEqual(
                "<html xmlns=\"http://example.org/\"><head id=\"yyy\"></head><body></body></html>",
                h.ToString());
        }

        [N.Test]
        public static void NullRef()
        {
            T.html._0 h = null;
            N.Assert.Throws<System.NullReferenceException>(
                () => { var he = h[head()]; });
        }

        [N.Test]
        public static void NullRef2()
        {
            T.head._0 h = null;
            N.Assert.Throws<System.NullReferenceException>(
                () => { T.head he = h; });
        }

        public class X : Xml.Static
        {
            public static void Comment()
            {
                T.html h = html()[head()[Comment("RRR<>")]][body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--RRR<>--></head><body></body></html>",
                    h.ToString());
            }
        }

        [N.Test]
        public static void Comment()
        {
            X.Comment();
        }

        public class X2 : Xml.Static
        {
            public static void Comment()
            {
                T.html h = html()[head()[Comment("RRR--aaa")]][body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--RRR- -aaa--></head><body></body></html>",
                    h.ToString());

                T.html h2 = html()[head()[Comment("RRR--aaa-")]][body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--RRR- -aaa- --></head><body></body></html>",
                    h2.ToString());

                T.html h3 = html()[head()[Comment("-RRR--aaa-")]][body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!---RRR- -aaa- --></head><body></body></html>",
                    h3.ToString());

                T.html h4 = html()[head()[Comment("--RRR--aaa-")]][body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--- -RRR- -aaa- --></head><body></body></html>",
                    h4.ToString());
            }
        }

        [N.Test]
        public static void Comment2()
        {
            X2.Comment();
        }
    }
}
