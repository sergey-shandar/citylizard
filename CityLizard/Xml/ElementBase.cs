namespace CityLizard.Xml
{
    using C = System.Collections.Generic;
    using X = System.Xml;

    using Extension;
    using System.Linq;

    public abstract class ElementBase: Node, IElementBase
    {
        public class Header
        {
            public readonly string Namespace;
            public readonly string Name;

            public readonly C.IList<IAttribute> RequiredAttributeList =
                new C.List<IAttribute>();

            public void AddRequiredAttribute(string name, string value)
            {
                this.RequiredAttributeList.Add(name, value);
            }

            public readonly C.IList<IAttribute> OptionalAttributeList =
                new C.List<IAttribute>();

            public void AddOptionalAttribute(string name, string value)
            {
                this.OptionalAttributeList.Add(name, value);
            }

            public C.IEnumerable<IAttribute> AttributeList
            {
                get
                {
                    return this.RequiredAttributeList.Concat(
                        this.OptionalAttributeList.Where(a => a.Value != null));
                }
            }

            public Header(string namespace_, string name)
            {
                this.Namespace = namespace_;
                this.Name = name;
            }
        }

        protected Header H;

        public string Namespace
        {
            get { return this.H.Namespace; }
        }

        public string Name
        {
            get { return this.H.Name; }
        }

        protected ElementBase(Header h)
        {
            this.H = h;
        }

        protected void WriteStart(X.XmlWriter writer, string parentNamespace)
        {
            if (parentNamespace != this.H.Namespace)
            {
                writer.WriteStartElement(this.H.Name, this.H.Namespace);
            }
            else
            {
                writer.WriteStartElement(this.H.Name);
            }
            writer.WriteList(this.H.AttributeList, this.H.Namespace);
        }

        #region IElementBase

        public C.IEnumerable<IAttribute> AttributeList
        {
            get { return this.H.AttributeList; }
        }

        #endregion
    }
}
