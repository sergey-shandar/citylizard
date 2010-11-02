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
        public static D.CodeAttributeDeclaration AddDeclaration<T>(
            this D.CodeAttributeDeclarationCollection dC,
            params D.CodeAttributeArgument[] aP)
        {
            var d = C.AttributeDeclaration<T>(aP);
            dC.Add(d);
            return d;
        }

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
