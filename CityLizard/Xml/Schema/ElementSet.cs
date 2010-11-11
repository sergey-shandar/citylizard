namespace CityLizard.Xml.Schema
{
    using C = System.Collections.Generic;
    using XS = System.Xml.Schema;

    internal class ElementSet : C.HashSet<XS.XmlSchemaElement>
    {
        public class Equality : C.EqualityComparer<XS.XmlSchemaElement>
        {
            public override bool Equals(
                XS.XmlSchemaElement x, XS.XmlSchemaElement y)
            {
                return x.QualifiedName == y.QualifiedName;
            }

            public override int GetHashCode(XS.XmlSchemaElement obj)
            {
                return obj.QualifiedName.GetHashCode();
            }
        }

        public ElementSet()
            : base(new Equality())
        {
        }
    }
}
