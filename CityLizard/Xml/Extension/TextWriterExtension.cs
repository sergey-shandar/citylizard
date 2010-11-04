namespace CityLizard.Xml.Extension
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public static class TextWriterExtension
    {
        public static void AppendNode(
            this T.StringBuilder t, INode o, string parentNamespace)
        {
            o.ToTextWriter(t, parentNamespace);
        }

        public static void AppendAttribute(
            this T.StringBuilder t, string name, string value)
        {
            t.Append(' ');
            t.Append(name);
            t.Append("=\"");
            t.Append(value);
            t.Append('"');
        }

        public static void AppendList<I>(
            this T.StringBuilder t, 
            C.IEnumerable<I> list, 
            string parentNamespace)
            where I: INode
        {
            foreach (var i in list)
            {
                t.AppendNode(i, parentNamespace);
            }
        }

        private struct Range
        {
            public T.StringBuilder Builder;
            public string Text;
            public int Begin;
            public int End;
            
            public Range(T.StringBuilder builder, string text)
            {
                this.Builder = builder;
                this.Text = text;
                this.Begin = 0;
                this.End = 0;
            }

            public void Append(string c)
            {
                this.Flush();
                this.Builder.Append(c);
            }

            public void Flush()
            {
                this.Builder.Append(
                    this.Text, this.Begin, this.End - this.Begin);
                this.Begin = this.End + 1;
            }
        }

        public static void AppendText(this T.StringBuilder builder, string text)
        {
            var r = new Range(builder, text);
            for(; r.End < text.Length; ++r.End)
            {
                switch (text[r.End])
                {
                    case '&':
                        r.Append("&amp;");
                        break;
                    case '<':
                        r.Append("&lt;");
                        break;
                    case '>':
                        r.Append("&gt;");
                        break;
                    case '"':
                        r.Append("&quot;");
                        break;
                }
            }
            r.Flush();
        }
    }
}
