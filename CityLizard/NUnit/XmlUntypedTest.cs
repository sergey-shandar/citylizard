namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;

    [N.TestFixture]
    class XmlUntypedTest
    {
        public class G : Xml.Untyped
        {
            public static T.X Do()
            {
                return
                    X("{http://www.w3.org/1999/xhtml}html")
                        [X("head")
                            [X("title")["Hello world!"]]
                        ]
                        [X("body")
                            [X("div")
                                ["Hello world!"]
                                [E("br")]
                                [X("a", A("href", "http://google.com/"))
                                    ["http://google.com/"]
                                ]
                            ]
                        ];
            }
        }

        [N.Test]
        public void Test()
        {
            var x = G.Do();
        }
    }
}
