namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;

    public static class ConstructorExtension
    {
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
