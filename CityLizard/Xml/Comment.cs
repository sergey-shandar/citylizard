namespace CityLizard.Xml
{
    using T = System.Text;
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
            T.StringBuilder builder, string parentNamespace)
        {
            builder.Append("<!--");
            builder.AppendText(this.Value);
            builder.Append("-->");
        }
    }
}