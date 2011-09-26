package com.codeplex.citylizard;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import org.w3c.dom.Document;
import static com.codeplex.citylizard.XmlBuilder.*;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 * An XML builder test.
 * @author Sergey Shandar
 */
public class XmlBuilderTest {
    
    public XmlBuilderTest() {
    }

    @BeforeClass
    public static void setUpClass() throws Exception {
    }

    @AfterClass
    public static void tearDownClass() throws Exception {
    }
    
    @Before
    public void setUp() {
    }
    
    @After
    public void tearDown() {
    }

    /**
     * Test of class XmlBuilder.
     */
    @Test
    public void test() throws ParserConfigurationException {
        System.out.println("XML builder");
        final Namespace x = new Namespace("http://www.w3.org/1999/xhtml");
        final Element html = 
            x.e("html",
                x.e("head",
                    x.e("title", t("Title of the page")),
                    x.e("meta",
                        a("content", "text/html;charset=UTF-8"),
                        a("http-equiv", "Content-Type")
                    ),
                    x.e("link",
                        a("href", "css/style.css"), 
                        a("rel", "stylesheet"), 
                        a("type", "text/css")
                    ),
                    x.e("script", 
                        a("type", "text/javascript"), 
                        a("src", "/JavaScript/jquery-1.4.2.min.js")
                    )
                ),
                x.e("body",
                    x.e("div",
                        x.e("h1", t("Test Form to Test")),
                        x.e("form", a("action", "post"), a("id", "Form1"))
                    ),
                    x.e("div",
                        x.e("label", t("Parameter")),
                        x.e("input", 
                            a("type", "text"), 
                            a("value", "Enter value")
                        ),
                        x.e("input", 
                            a("type", "submit"), 
                            a("value", "Submit!")
                        )
                    ),
                    x.e("div",
                        x.e("p", t("Textual description of the footer")),
                        x.e("a", a("href", "http://google.com/")),
                        x.e("span", t("You can find us here"))
                    ),
                    x.e("div", t("Another nested container"))
                )
            );
        final Document document = 
            DocumentBuilderFactory.
                newInstance().
                newDocumentBuilder().
                newDocument();
        html.addToDomDocument(document);
    }
}
