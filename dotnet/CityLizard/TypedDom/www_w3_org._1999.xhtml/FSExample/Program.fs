let x = new www_w3_org._1999.xhtml.X()

let r = 
    x.html
        .[x.head.
            [x.title.["Title of the page"]]
            .[x.meta_(
                content = "text/html;charset=UTF-8",
                http_equiv = "Content-Type")
            ]
            .[x.link_(href = "css/style.css", rel = "stylesheet", ``type`` = "text/css")]
            .[x.script_(``type`` = "text/javascript", src = "/JavaScript/jquery-1.4.2.min.js")]
        ]
        .[x.body
            .[x.div
                .[x.h1.["Test Form to Test"]]
                .[x.form_(action = "post", id = "Form1")
                    .[x.div
                        .[x.label.["Parameter"]]
                        .[x.input_(``type`` = "text", value = "Enter value")]
                        .[x.input_(``type`` = "submit", value = "Submit!")]
                    ]
                ]
                .[x.div
                    .[x.p.["Textual description of the footer"]]
                    .[x.a_(href = "http://google.com/")
                        .[x.span.["You can find us here"]]
                    ]
                    .[x.div.["Another nested container"]]
                ]
            ]
        ]

System.Console.WriteLine(r)
System.Console.ReadLine() |> ignore


