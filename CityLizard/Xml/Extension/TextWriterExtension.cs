namespace CityLizard.Xml.Extension
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// TextWriter extension methods.
    /// </summary>
    public static class TextWriterExtension
    {
        /// <summary>
        /// Writes the node to the current writer instance.
        /// </summary>
        /// <param name="this_">The current writer instance.</param>
        /// <param name="node">The node to writes from.</param>
        /// <param name="parentNamespace">
        /// True to write without xmlns attribute; otherwise, false.
        /// </param>
        public static void WriteNode(
            this IO.TextWriter this_, INode node, string parentNamespace)
        {
            node.ToTextWriter(this_, parentNamespace);
        }

        /// <summary>
        /// Writes the attribute to the current writer instance.
        /// </summary>
        /// <param name="this_">The current writer instance.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public static void WriteAttribute(
            this IO.TextWriter this_, string name, string value)
        {
            this_.Write(' ');
            this_.Write(name);
            this_.Write("=\"");
            this_.Write(value);
            this_.Write('"');
        }

        /// <summary>
        /// Writes the list of nodes to the current writer instance.
        /// </summary>
        /// <typeparam name="I">The type of nodes.</typeparam>
        /// <param name="this_">The current writer instance.</param>
        /// <param name="list">The list of nodes.</param>
        /// <param name="parentNamespace">
        /// True to write without xmlns attribute; otherwise, false.
        /// </param>
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

        /// <summary>
        /// Escapes and writes the text to the current writer instance.
        /// </summary>
        /// <param name="writer">The current writer instance.</param>
        /// <param name="text">The text.</param>
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
