namespace CityLizard.Xml
{
    using X = System.Xml;
    using C = System.Collections.Generic;

    public class EmptyElement: ElementBase
    {
        protected EmptyElement(ElementBase.Header h)
            : base(h)
        {
        }

        /// <summary>
        /// C. HTML Compatibility Guidelines.
        /// (http://www.w3.org/TR/xhtml1/#guidelines).
        /// 
        /// C.2. Empty Elements
        /// 
        /// Include a space before the trailing / and > of empty 
        /// elements, e.g. <br />, <hr /> and <img src="karen.jpg" 
        /// alt="Karen" />. Also, use the minimized tag syntax for empty 
        /// elements, e.g. <br />, as the alternative syntax <br></br> 
        /// allowed by XML gives uncertain results in many existing user 
        /// agents.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parentNamespace"></param>
        public override void ToXmlWriter(
            X.XmlWriter writer, string parentNamespace)
        {
            base.WriteStart(writer, parentNamespace);
            writer.WriteEndElement();
        }

        public override C.IEnumerable<INode> ContentList
        {
            get { return new INode[0]; }
        }
    }
}
