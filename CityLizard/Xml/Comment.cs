namespace CityLizard.Xml
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    public sealed class Comment : TextValue, IComment
    {
        public Comment(string value)
            : base(value)
        {
        }

        public override void ToTextWriter(
            IO.TextWriter writer, string parentNamespace)
        {
            writer.Write("<!--");
            writer.WriteText(this.Value);
            writer.Write("-->");
        }
    }
}