namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using S = System;

    using System.Linq;

    public static class ParameterDeclarationExpressionCollectionExtension
    {
        public static D.CodeParameterDeclarationExpression Add<T>(
            this D.CodeParameterDeclarationExpressionCollection this_,
            string name)
        {
            var r = new D.CodeParameterDeclarationExpression(
                new D.CodeTypeReference(typeof(T)),
                name);
            this_.Add(r);
            return r;
        }
    }
}
