namespace CityLizard.CodeDom
{
    using D = System.CodeDom;

    public partial class CodeDom
    {
        public static class T
        {
            public class Unit : D.CodeCompileUnit
            {
                public Unit this[Namespace Namespace]
                {
                    get
                    {
                        this.Namespaces.Add(Namespace);
                        return this;
                    }
                }
            }

            public class Namespace : D.CodeNamespace
            {
                public Namespace(string Name): base(Name)
                {
                }
            }
        }

        public T.Unit Unit()
        {
            return new T.Unit();
        }

        public T.Namespace Namespace(string Name)
        {
            return new T.Namespace(Name);
        }
    }
}
