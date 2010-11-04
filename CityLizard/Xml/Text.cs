namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    public sealed class Text : TextValue, IText
    {
        public Text(string value)
            : base(value)
        {
        }

        public override void ToTextWriter(
            T.StringBuilder builder, string parentNamespace)
        {
            builder.AppendText(this.Value);
        }
    }
}