namespace CityLizard.Xml
{
    public abstract class ElementBase: Node, IElementBase
    {
        public string Namespace
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        protected ElementBase(string namespace_, string name)
        {
            this.Namespace = namespace_;
            this.Name = name;
        }
    }
}
