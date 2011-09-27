namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;

    [N.TestFixture]
    class XmlUntypedTest
    {
        public class G : Xml.Untyped
        {
            public G(): base("http://www.w3.org/1999/xhtml")
            {
            }

            public T.X Do()
            {
                return
                    X("html")
                        [X("head")
                            [X("title")["Title of the page"]]
                            [E("meta",
                                A("content", "text/html;charset=UTF-8"),
                                A("http_equiv", "Content-Type"))
                            ]
                            [E("link", 
                                A("href", "css/style.css"), 
                                A("rel", "stylesheet"), 
                                A("type", "text/css"))
                            ]
                            [X("script", 
                                A("type", "text/javascript"), 
                                A("src", "/JavaScript/jquery-1.4.2.min.js"))
                            ]
                        ]
                        [X("body")
                            [X("div")
                                [X("h1")["Test Form to Test"]]
                                [X("form", 
                                    A("action", "post"), A("id", "Form1"))
                                    [X("div")
                                        [X("label")["Parameter"]]
                                        [E("input", 
                                            A("type", "text"), 
                                            A("value", "Enter value"))
                                        ]
                                        [E("input", 
                                            A("type", "submit"), 
                                            A("value", "Submit!"))
                                        ]
                                    ]
                                ]
                                [X("div")
                                    [X("p")
                                        ["Textual description of the footer"]
                                    ]
                                    [X("a", A("href", "http://google.com/"))
                                        [X("span")["You can find us here"]]
                                    ]
                                    [X("div")["Another nested container"]]
                                ]
                            ]
                        ];
            }
        }

        [N.Test]
        public void Test()
        {
            var x = new G().Do();
        }
    }
}
