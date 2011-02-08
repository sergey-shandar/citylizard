using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace www_w3_org._1999.xhtml.Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class Generator : X
        {
            public T.html Generate()
            {
                return
                    html
                        [head
                            [title["Title of the page"]]
                            [meta_(
                                content: "text/html;charset=UTF-8",
                                http_equiv: "Content-Type")
                            ]
                            [link_(href: "css/style.css", rel: "stylesheet", type: "text/css")]
                            [script_(type: "text/javascript", src: "/JavaScript/jquery-1.4.2.min.js")]
                        ]
                        [body
                            [div
                                [h1["Test Form to Test"]]
                                [form_(action: "post", id: "Form1")
                                    [div
                                        [label["Parameter"]]
                                        [input_(type: "text", value: "Enter value")]
                                        [input_(type: "submit", value: "Submit!")]
                                    ]
                                ]
                                [div
                                    [Comment("comment")]
                                    [p["Textual description of the footer"]]
                                    [a_(href: "http://google.com/")
                                        [span["You can find us here"]]
                                    ]
                                    [div["Another nested container"]]
                                ]
                            ]
                        ];
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var h = new Generator().Generate();
            var s = h.ToString();
            this.webBrowser1.DocumentText = s;
        }
    }
}
