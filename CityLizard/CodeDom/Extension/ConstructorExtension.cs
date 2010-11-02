namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using C = Code;

    public static class ConstructorExtension
    {
        public static void AddBaseParameter<T>(
            this D.CodeConstructor c, string name)
        {
            c.Parameters.Add(C.ParameterDeclarationExpression<T>(name));
            c.BaseConstructorArgs.Add(C.VariableReferenceExpression(name));
        }
    }
}
