namespace CityLizard.Xml
{
    using IO = System.IO;
    using X = System.Xml;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    /// <summary>
    /// Represents the content of an XML comment.
    /// </summary>
    public sealed class Comment : CharacterData, IComment
    {
        /// <summary>
        /// Extensible Markup Language (XML) 1.0 (Fifth Edition)
        /// 2.5 Comments
        /// (http://www.w3.org/TR/2008/REC-xml-20081126/#sec-comments)
        /// 
        /// For compatibility, the string " -- " (double-hyphen) must not occur 
        /// within comments.
        /// 
        /// Note that the grammar does not allow a comment ending in --->
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string Escape(string value)
        {
            var result = "";
            while (true)
            {
                var i = value.IndexOf('-');
                if (i == -1)
                {
                    result += value;
                    break;
                }
                else if (i == value.Length - 1)
                {
                    // warning: 
                    // the XML grammar does not allow a comment ending in --->
                    result += value + ' ';
                    break;
                }
                else 
                {
                    ++i;
                    result += value.Substring(0, i);
                    value = value.Substring(i);
                    if (value[0] == '-')
                    {
                        // warning: 
                        // For compatibility, the string " -- " (double-hyphen) 
                        // must not occur within comments.
                        result += ' ';
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the Comment class.
        /// </summary>
        /// <param name="value">
        /// The content of the comment.
        /// </param>
        public Comment(string value)
            : base(Escape(value))
        { 
        }

        /*
        /// <summary>
        /// Writes the comment to the text writer.
        /// </summary>
        /// <param name="writer">The text writer.</param>
        /// <param name="parentNamespace">Ignored.</param>
        public override void ToTextWriter(
            IO.TextWriter writer, string parentNamespace)
        {
            writer.Write("<!--");
            writer.WriteText(this.Value);
            writer.Write("-->");
        }
         * */

        public override void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace)
        {
            writer.WriteComment(this.Value);
        }
    }
}