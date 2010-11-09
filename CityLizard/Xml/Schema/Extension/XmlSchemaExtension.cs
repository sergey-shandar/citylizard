namespace CityLizard.Xml.Schema.Extension
{
    using C = System.Collections.Generic;
    using XS = System.Xml.Schema;

    using System.Linq;

    public static class XmlSchemaExtension
    {
        public static C.IEnumerable<XS.XmlSchemaElement> GlobalElementsTyped(
            this XS.XmlSchemaSet x)
        {
            return x.GlobalElements.Values.Cast<XS.XmlSchemaElement>();
        }

        public static C.IEnumerable<XS.XmlSchemaType> GlobalTypesTyped(
            this XS.XmlSchemaSet x)
        {
            return x.GlobalTypes.Values.Cast<XS.XmlSchemaType>();
        }

        public static C.IEnumerable<XS.XmlSchemaParticle> ItemsTyped(
            this XS.XmlSchemaSequence x)
        {
            return x.Items.Cast<XS.XmlSchemaParticle>();
        }

        public static C.IEnumerable<XS.XmlSchemaParticle> ItemsTyped(
            this XS.XmlSchemaChoice x)
        {
            return x.Items.Cast<XS.XmlSchemaParticle>();
        }

        public static C.IEnumerable<XS.XmlSchemaAttribute> AttributeUsesTyped(
            this XS.XmlSchemaComplexType x)
        {
            return x.AttributeUses.Values.Cast<XS.XmlSchemaAttribute>();
        }

        public static bool IsDigit(this char x)
        {
            return char.IsDigit(x);
        }
    }
}
