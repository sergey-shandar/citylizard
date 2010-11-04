namespace CityLizard.Xml
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    public sealed class Attribute : CharacterData, IAttribute
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
            IO.TextWriter writer, string parentNamespace)
        {
            writer.WriteAttribute(this.Name, this.Value);
        }
    }
}