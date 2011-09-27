Public Class Form1

    Public Class Generator
        Inherits www_w3_org._1999.xhtml.X

        Public Function Generate() As T.html
            Return _
                html _
                    (head _
                        (title("Title of the page")) _
                        (meta_(
                            content:="text/html;charset=UTF-8",
                            http_equiv:="Content-Type")
                        ) _
                        (link_(
                            href:="css/style.css",
                            rel:="stylesheet",
                            type:="text/css")
                        ) _
                        (script_(
                            type:="text/javascript",
                            src:="/JavaScript/jquery-1.4.2.min.js")
                        )
                    ) _
                    (body _
                        (div _
                            (h1("Test Form to Test")) _
                            (form_(action:="post", id:="Form1") _
                                (div _
                                    (label("Parameter")) _
                                    (input_(type:="text", value:="Enter value")) _
                                    (input_(type:="submit", value:="Submit!"))
                                )
                            )
                        ) _
                        (div _
                            (Comment("comment")) _
                            (p("Textual description of the footer")) _
                            (a_(href:="http://google.com/") _
                                (span("You can find us here"))
                            ) _
                            (div("Another nested container"))
                        )
                    )
        End Function

    End Class

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim h = New Generator().Generate()
        Dim s = h.ToString()
        WebBrowser1.DocumentText = s
    End Sub
End Class
