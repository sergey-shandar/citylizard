namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using C = Code;

    using System.Linq;

    /// <summary>
    /// Attribute declaration collection extension.
    /// </summary>
    public static class AttributeDeclarationCollectionExtension
    {
        /// <summary>
        /// Add an attribute declaration.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="dC">Collection.</param>
        /// <param name="aP">Arguments.</param>
        /// <returns>Created attribute declaration.</returns>
        public static D.CodeAttributeDeclaration AddDeclaration<T>(
            this D.CodeAttributeDeclarationCollection dC,
            params D.CodeAttributeArgument[] aP)
        {
            var d = C.AttributeDeclaration<T>(aP);
            dC.Add(d);
            return d;
        }

        /// <summary>
        /// Add an attribute declaration initialized by strings.
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="dC">Collection.</param>
        /// <param name="aP">Arguments.</param>
        /// <returns>Created attribute declaration.</returns>
        public static D.CodeAttributeDeclaration AddDeclarationString<T>(
            this D.CodeAttributeDeclarationCollection dC,
            params string[] aP)
        {
            return dC.AddDeclaration<T>(
                aP.Select(x => C.AttributeArgument(C.PrimitiveExpression(x))).
                    ToArray());
        }
    }
}
