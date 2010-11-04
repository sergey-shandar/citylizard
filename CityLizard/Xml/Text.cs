namespace CityLizard.Xml
{
    using IO = System.IO;
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
            IO.TextWriter writer, string parentNamespace)
        {
            writer.WriteText(this.Value);
        }
    }
}