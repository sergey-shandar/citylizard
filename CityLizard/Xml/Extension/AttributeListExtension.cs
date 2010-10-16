namespace CityLizard.Xml.Extension
{
    using C = System.Collections.Generic;

    public static class AttributeListExtension
    {
        public static void Add(
            this C.IList<IAttribute> x, string name, string value)
        {
            x.Add(new Attribute(null, name, value));
        }
    }
}
