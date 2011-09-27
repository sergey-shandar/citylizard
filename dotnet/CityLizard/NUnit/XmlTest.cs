namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;

    // using Xml.Extension;

    [N.TestFixture]
    public static class XmlTest
    {
        class NotEmpty: Xml.Linked.Element.Mixed
        {
            public NotEmpty(Xml.QName q)
            {
                this.SetUp(null, q.Namespace, q.LocalName);
            }
        }

        class Empty : Xml.Linked.Element.Empty
        {
            public Empty(Xml.QName q)
            {
                this.SetUp(null, q.Namespace, q.LocalName);
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
            EmptyTest(
                "http://example.com/", 
                "main", 
                "<main xmlns=\"http://example.com/\" />");
            NotEmptyTest(
                "http://example.com/",
                "main",
                "<main xmlns=\"http://example.com/\"></main>");
        }

        public class X: Xml.Implementation
        {

            public T.html._0 html()
            {
                return new T.html._0(this);
            }

            public T.head._0 head(string id = null)
            {
                return new T.head._0(this, id);
            }

            public T.body body()
            {
                return new T.body(this);
            }

            public T.title title()
            {
                return new T.title(this);
            }

            public class T
            {
                public class html : Xml.Linked.Element.NotMixed
                {
                    internal html(_1 part0, body child)
                    {
                        this.SetUp(part0, child);
                    }

                    public class _0 : Xml.Linked.Element.NotMixed
                    {
                        internal _0(Xml.Implementation implementation)
                        {
                            this.SetUp(
                                implementation, "http://example.org/", "html");
                        }

                        public _1 this[head head]
                        {
                            get
                            {
                                return new _1(this, head);
                            }
                        }
                    }

                    public class _1 : Xml.Linked.Element.NotMixed
                    {
                        internal _1(_0 part0, head head)
                        {
                            this.SetUp(part0, head);
                        }

                        public html this[body body]
                        {
                            get
                            {
                                return new html(this, body);
                            }
                        }
                    }
                }

                public class head : Xml.Linked.Element.NotMixed
                {
                    internal head(_0 part0, Xml.Linked.Element.Element child)
                    {
                        this.SetUp(part0, child);
                    }

                    internal head(_0 part0)
                    {
                        this.SetUp(part0);
                    }

                    public class _0 : Xml.Linked.Element.NotMixed
                    {
                        internal _0(
                            Xml.Implementation implementation,
                            string id = null)
                        {
                            this.SetUp(
                                implementation, "http://example.org/", "head");
                            this.AddOptionalAttribute("id", id);
                        }

                        public head this[title title]
                        {
                            get
                            {
                                return new head(this, title);
                            }
                        }

                        public _0 this[Xml.Linked.Comment comment]
                        {
                            get
                            {
                                this.AddComment(comment.Value);
                                return this;
                            }
                        }

                        public static implicit operator head(_0 _0)
                        {
                            return new head(_0);
                        }
                    }
                }

                public class body : Xml.Linked.Element.Mixed
                {
                    internal body(Xml.Implementation implementation)
                    {
                        this.SetUp(
                            implementation, "http://example.org/", "body");
                    }
                }

                public class title : Xml.Linked.Element.Empty
                {
                    internal title(Xml.Implementation implementation)
                    {
                        this.SetUp(
                            implementation, "http://example.org/", "title");
                    }
                }
            }
        }

        [N.Test]
        public static void IdeaTest()
        {
            var x = new X();
            X.T.html h = x.html()[x.head()[x.title()]][x.body()];
            N.Assert.AreEqual(
                "<html xmlns=\"http://example.org/\"><head><title /></head><body></body></html>",
                h.ToString());
        }

        [N.Test]
        public static void IdeaTest1()
        {
            var x = new X();
            X.T.html h = x.html()[x.head("xxx")[x.title()]][x.body()];
            N.Assert.AreEqual(
                "<html xmlns=\"http://example.org/\"><head id=\"xxx\"><title /></head><body></body></html>",
                h.ToString());
        }

        [N.Test]
        public static void IdeaTest2()
        {
            var x = new X();
            X.T.html h = x.html()[x.head("yyy")][x.body()];
            N.Assert.AreEqual(
                "<html xmlns=\"http://example.org/\"><head id=\"yyy\"></head><body></body></html>",
                h.ToString());
        }

        [N.Test]
        public static void NullRef()
        {
            var x = new X();
            X.T.html._0 h = null;
            N.Assert.Throws<System.NullReferenceException>(
                () => { var he = h[x.head()]; });
        }

        [N.Test]
        public static void NullRef2()
        {
            var x = new X();
            X.T.head._0 h = null;
            N.Assert.Throws<System.NullReferenceException>(
                () => { X.T.head he = h; });
        }

        public class XX : Xml.Implementation
        {
            public static void Comment()
            {
                var x = new X();
                X.T.html h = x.html()[x.head()[x.Comment("RRR<>")]][x.body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--RRR<>--></head><body></body></html>",
                    h.ToString());
            }
        }

        [N.Test]
        public static void Comment()
        {
            XX.Comment();
        }

        public class X2 : Xml.Implementation
        {
            public static void Comment()
            {
                var x = new X();
                X.T.html h = x.html()[x.head()[x.Comment("RRR--aaa")]][x.body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--RRR- -aaa--></head><body></body></html>",
                    h.ToString());

                X.T.html h2 = x.html()[x.head()[x.Comment("RRR--aaa-")]][x.body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!--RRR- -aaa- --></head><body></body></html>",
                    h2.ToString());

                X.T.html h3 = x.html()[x.head()[x.Comment("-RRR--aaa-")]][x.body()];
                N.Assert.AreEqual(
                    "<html xmlns=\"http://example.org/\"><head><!---RRR- -aaa- --></head><body></body></html>",
                    h3.ToString());

                X.T.html h4 = x.html()[x.head()[x.Comment("--RRR--aaa-")]][x.body()];
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
