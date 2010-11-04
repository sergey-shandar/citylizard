namespace CityLizard.Xml.Extension
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public static class TextWriterExtension
    {
        public static void WriteNode(
            this IO.TextWriter this_, INode o, string parentNamespace)
        {
            o.ToTextWriter(this_, parentNamespace);
        }

        public static void WriteAttribute(
            this IO.TextWriter this_, string name, string value)
        {
            this_.Write(' ');
            this_.Write(name);
            this_.Write("=\"");
            this_.Write(value);
            this_.Write('"');
        }

        public static void WriteList<I>(
            this IO.TextWriter this_, 
            C.IEnumerable<I> list, 
            string parentNamespace)
            where I: INode
        {
            foreach (var i in list)
            {
                this_.WriteNode(i, parentNamespace);
            }
        }

        private struct Range
        {
            public IO.TextWriter Writer;
            public string Text;
            public int Begin;
            public int End;
            
            public Range(IO.TextWriter writer, string text)
            {
                this.Writer = writer;
                this.Text = text;
                this.Begin = 0;
                this.End = 0;
            }

            public void Write(string c)
            {
                this.Flush();
                this.Writer.Write(c);
            }

            public void Flush()
            {
                this.Writer.Write(
                    this.Text, this.Begin, this.End - this.Begin);
                this.Begin = this.End + 1;
            }
        }

        public static void WriteText(this IO.TextWriter writer, string text)
        {
            var r = new Range(writer, text);
            for(; r.End < text.Length; ++r.End)
            {
                switch (text[r.End])
                {
                    case '&':
                        r.Write("&amp;");
                        break;
                    case '<':
                        r.Write("&lt;");
                        break;
                    case '>':
                        r.Write("&gt;");
                        break;
                    case '"':
                        r.Write("&quot;");
                        break;
                }
            }
            r.Flush();
        }
    }
}
