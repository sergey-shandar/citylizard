namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using C = Code;

    /// <summary>
    /// Namespace collection extension methods.
    /// </summary>
    public static class NamespaceCollectionExtension
    {
        /// <summary>
        /// Add a new namespace.
        /// </summary>
        /// <param name="nC">Namespace collection.</param>
        /// <param name="name">Namespace name.</param>
        /// <returns>Namespace.</returns>
        public static D.CodeNamespace AddNamespace(
            this D.CodeNamespaceCollection nC, string name)
        {
            var n = C.Namespace(name);
            nC.Add(n);
            return n;
        }
    }
}
