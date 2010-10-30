namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using S = System;

    using System.Linq;

    public static class Extension
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

        public static void AddBaseParameter<T>(
            this D.CodeConstructor c, string name)
        {
            c.Parameters.Add(
                new D.CodeParameterDeclarationExpression(typeof(T), name));
            c.BaseConstructorArgs.Add(
                new D.CodeVariableReferenceExpression(name));
        }
    }
}
