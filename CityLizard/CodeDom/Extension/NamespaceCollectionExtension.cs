namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using C = Code;

    public static class NamespaceCollectionExtension
    {
        public static D.CodeNamespace AddNamespace(
            this D.CodeNamespaceCollection nC, string name)
        {
            var n = C.Namespace(name);
            nC.Add(n);
            return n;
        }
    }
}
