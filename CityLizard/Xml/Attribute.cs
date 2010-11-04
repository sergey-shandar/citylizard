namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    public sealed class Attribute : TextValue, IAttribute
    {
        private readonly string Ns;

        private readonly string Name;

        public Attribute(string ns, string name, string value)
            : base(value)
        {
            this.Ns = ns;
            this.Name = name;
        }

        string IName.Namespace
        {
            get { return this.Ns; }
        }

        string IName.Name
        {
            get { return this.Name; }
        }

        public override void ToTextWriter(
            T.StringBuilder builder, string parentNamespace)
        {
            builder.AppendAttribute(this.Name, this.Value);
        }
    }
}