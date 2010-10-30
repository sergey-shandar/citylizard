namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;

    using System.Linq;

    public static class AttributeDeclarationCollection
    {
        public static D.CodeAttributeDeclaration AddDeclaration<T>(
            this D.CodeAttributeDeclarationCollection dC,
            params D.CodeAttributeArgument[] aP)
        {
            var d = new D.CodeAttributeDeclaration(
                new D.CodeTypeReference(typeof(T)), aP);
            dC.Add(d);
            return d;
        }

        public static D.CodeAttributeDeclaration AddDeclarationString<T>(
            this D.CodeAttributeDeclarationCollection dC,
            params string[] aP)
        {
            return dC.AddDeclaration<T>(
                aP.
                    Select(
                        x => new D.CodeAttributeArgument(
                            new D.CodePrimitiveExpression(x))).
                    ToArray());
        }
    }
}
