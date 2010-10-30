namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;

    public static class NamespaceCollectionExtension
    {
        public static D.CodeNamespace AddNamespace(
            this D.CodeNamespaceCollection nC, string name)
        {
            var n = new D.CodeNamespace(name);
            nC.Add(n);
            return n;
        }
    }
}
