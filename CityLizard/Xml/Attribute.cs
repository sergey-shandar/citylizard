namespace CityLizard.Xml
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;
    using X = System.Xml;

    using System.Linq;
    using Extension;

    public sealed class Attribute : CharacterData, IAttribute
    {
        public Attribute(string namespace_, string name, string value)
            : base(value)
        {
            this.Namespace = namespace_;
            this.Name = name;
        }

        public string Namespace { get; private set; }

        public string Name { get; private set; }

        public override void ToTextWriter(
            IO.TextWriter writer, string parentNamespace)
        {
            writer.WriteAttribute(this.Name, this.Value);
        }

        public override void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace)
        {
            writer.WriteAttributeString(this.Name, this.Value);
        }

    }
}