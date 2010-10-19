namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public abstract class TextValue : Node, ITextValue
    {
        public readonly string Value;

        public TextValue(string value)
        {
            this.Value = value;
        }

        string ITextValue.Value
        {
            get { return this.Value; }
        }
    }
}