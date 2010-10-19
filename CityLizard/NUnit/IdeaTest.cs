namespace CityLizard.NUnit
{
    class IdeaTest
    {
        public class X
        {
            public class T
            {
                public class html
                {
                    public class _0
                    {
                        public _1 this[head head]
                        {
                            get
                            {
                                return new _1();
                            }
                        }
                    }

                    public class _1
                    {
                        public html this[body body]
                        {
                            get
                            {
                                return new html();
                            }
                        }
                    }
                }

                public class head
                {
                    public class _0
                    {
                        public _0 this[script script]
                        {
                            get
                            {
                                return this;
                            }
                        }

                        public _0 this[meta meta]
                        {
                            get
                            {
                                return this;
                            }
                        }

                        public _0 this[link link]
                        {
                            get
                            {
                                return this;
                            }
                        }

                        public head this[title title]
                        {
                            get
                            {
                                return new head();
                            }
                        }
                    }

                    public head this[script script]
                    {
                        get
                        {
                            return this;
                        }
                    }

                    public head this[meta meta]
                    {
                        get
                        {
                            return this;
                        }
                    }

                    public head this[link link]
                    {
                        get
                        {
                            return this;
                        }
                    }
                }

                public class body
                {
                    public body this[div div]
                    {
                        get
                        {
                            return this;
                        }
                    }
                }

                public class script
                {
                }

                public class meta
                {
                }

                public class link
                {
                }

                public class title
                {
                    public class _0
                    {
                        public static implicit operator title(_0 s0)
                        {
                            return new title();
                        }
                        public title this[string text]
                        {
                            get
                            {
                                return new title();
                            }
                        }
                    }
                }

                public class div
                {
                    public div this[h1 h1]
                    {
                        get
                        {
                            return this;
                        }
                    }
                    public div this[form form]
                    {
                        get
                        {
                            return this;
                        }
                    }
                    public div this[div div]
                    {
                        get
                        {
                            return this;
                        }
                    }
                    public div this[p p]
                    {
                        get
                        {
                            return this;
                        }
                    }
                    public div this[a a]
                    {
                        get
                        {
                            return this;
                        }
                    }
                    public div this[string text]
                    {
                        get
                        {
                            return this;
                        }
                    }
                }

                public class h1
                {
                    public class _0
                    {
                        public static implicit operator h1(_0 s0)
                        {
                            return new h1();
                        }
                        public h1 this[string text]
                        {
                            get
                            {
                                return new h1();
                            }
                        }
                    }
                }

                public class label
                {
                    public class _0
                    {
                        public static implicit operator label(_0 x)
                        {
                            return new label();
                        }
                        public label this[string text]
                        {
                            get
                            {
                                return new label();
                            }
                        }
                    }
                }

                public class form
                {
                    public class _0
                    {
                        public static implicit operator form(_0 x)
                        {
                            return new form();
                        }
                        public form this[string text]
                        {
                            get
                            {
                                return new form();
                            }
                        }
                        public _0 this[label label]
                        {
                            get
                            {
                                return this;
                            }
                        }
                        public _0 this[input input]
                        {
                            get
                            {
                                return this;
                            }
                        }
                        public _0 this[br br]
                        {
                            get
                            {
                                return this;
                            }
                        }
                    }

                    public _0 this[label label]
                    {
                        get
                        {
                            return new _0();
                        }
                    }
                    public _0 this[input input]
                    {
                        get
                        {
                            return new _0();
                        }
                    }
                    public _0 this[br br]
                    {
                        get
                        {
                            return new _0();
                        }
                    }
                }

                public class input
                {
                }

                public class br
                {
                }

                public class p
                {
                    public class _0
                    {
                        public static implicit operator p(_0 x)
                        {
                            return new p();
                        }
                        public p this[string text]
                        {
                            get
                            {
                                return new p();
                            }
                        }
                    }
                }

                public class a
                {
                    public a this[span span]
                    {
                        get
                        {
                            return this;
                        }
                    }
                }

                public class span
                {
                    public class _0
                    {
                        public static implicit operator span(_0 x)
                        {
                            return new span();
                        }
                        public span this[string text]
                        {
                            get
                            {
                                return new span();
                            }
                        }
                    }
                }
            }

            public static T.html._0 html_(string id = null)
            {
                return new T.html._0();
            }

            public static X.T.html._0 html
            {
                get
                {
                    return new X.T.html._0();
                }
            }

            public static X.T.head._0 head
            {
                get
                {
                    return new X.T.head._0();
                }
            }

            public static X.T.body body
            {
                get
                {
                    return new X.T.body();
                }
            }

            public static X.T.body body_(
                string id = null, string @class = null, string onclick = null)
            {
                return new X.T.body();
            }

            public static X.T.script script
            {
                get
                {
                    return new X.T.script();
                }
            }

            public static X.T.script script_(string href = null, string type = null)
            {
                return new X.T.script();
            }

            public static X.T.meta meta
            {
                get
                {
                    return new X.T.meta();
                }
            }

            public static X.T.meta meta_(string http_equip = null, string content = null, string charset = null)
            {
                return new X.T.meta();
            }

            public static X.T.link link
            {
                get
                {
                    return new X.T.link();
                }
            }

            public static X.T.link link_(string href = null, string rel = null, string type = null)
            {
                return new X.T.link();
            }

            public static X.T.title._0 title
            {
                get
                {
                    return new X.T.title._0();
                }
            }

            public static T.div div
            {
                get
                {
                    return new T.div();
                }
            }

            public static T.h1._0 h1
            {
                get
                {
                    return new T.h1._0();
                }
            }

            public static T.form._0 form
            {
                get
                {
                    return new T.form._0();
                }
            }

            public static T.form._0 form_(string id = null, string type = null)
            {
                return new T.form._0();
            }

            public static T.label._0 label
            {
                get
                {
                    return new T.label._0();
                }
            }

            public static T.input input
            {
                get
                {
                    return new T.input();
                }
            }

            public static T.input input_(string type = null, string value = null)
            {
                return new T.input();
            }

            public static T.br br
            {
                get
                {
                    return new T.br();
                }
            }

            public static T.p._0 p
            {
                get
                {
                    return new T.p._0();
                }
            }

            public static T.a a_(string href)
            {
                return new T.a();
            }

            public static T.span._0 span
            {
                get
                {
                    return new T.span._0();
                }
            }
        }

        class Y : X
        {
            private void D()
            {
                var h =
                    html_(id: "xxx")
                        [head
                            [script]
                            [script]
                            [title]
                            [script]
                            [script]
                        ]
                        [body_(id: "id0", @class: "www", onclick: "eee")
                        ];
                var y =
                    html
                        [head
                            [title]
                        ]
                        [body];
                var z =
                    html
                        [head[title]]
                        [body];

                ///
                var x =
                    html
                        [head
                            [title["Title of the page"]]
                            [meta_(http_equip: "contentype", content: "html", charset: "utf-8")]
                            [link_(href: "css/style.css", rel: "stylesheer", type: "css")]
                            [script_(href: "/JavaScripts/jquery-1.4.2.min.js", type: "javascript")]
                        ]
                        [body
                            [div
                                [h1["Test Form to Test"]]
                                [form_(id: "Form1", type: "post")
                                    [label["Parameter"]]
                                    ["="]
                                    [input_(type: "text", value: "Enter value")]
                                    [br]
                                    [input_(type: "submit", value: "Submit!")]
                                ]
                                [div
                                    [p["Textual description of the footer"]]
                                    [a_(href: "http://google.com")
                                        [span["You can find us here"]]
                                    ]
                                ]
                                [div["Another nested container"]]
                            ]
                        ];
                /*
html[
    head[
        title[ "Title of the page" ],
        meta.attr(http-equiv: "contenttype", content: "html", charset: "utf-8"),
        link.attr(href: "css/style.css", rel: "stylesheet", type: "css"),
        script.attr(href:"/JavaScripts/jquery-1.4.2.min.js", type: "javascript")
    ],
    body[
        div[
            h1[ "Test Form to Test" ],
            form.attr(id: "Form1", type: "post")[
                label[ "Parameter" ], "=", input.attr(type:"text", value: "Enter value"), br,
                input.attr(type: "submit", value: "Submit !")
            ],
            div[
                p[ "Textual description of the footer" ],
                a.attr(href: "http://google.com" )[
                    span[ "You can find us here"]
                ]
            ],
            div[ "Another nested container" ]
        ]
    ]
]
                 * */
            }
        }
    }
}
