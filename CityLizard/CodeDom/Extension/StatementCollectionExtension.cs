namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using C = Code;

    public static class StatementCollectionExtension
    {
        public static S AddStatement<S>(this D.CodeStatementCollection sC, S s)
            where S: D.CodeStatement
        {
            sC.Add(s);
            return s;
        }

        public static E AddExpression<E>(this D.CodeStatementCollection sC, E e)
            where E: D.CodeExpression
        {
            sC.Add(e);
            return e;
        }

        public static D.CodeVariableDeclarationStatement AddVariable<T>(
            this D.CodeStatementCollection sC,
            string name,
            params D.CodeExpression[] eP)
        {
            return sC.AddStatement(C.VariableDeclarationStatement<T>(name, eP));
        }

        public static D.CodeMethodInvokeExpression AddMethodInvoke(
            this D.CodeStatementCollection sC,
            D.CodeExpression e,
            string name,
            params D.CodeExpression[] eP)
        {
            return sC.AddExpression(C.MethodInvokeExpression(e, name, eP));
        }
    }
}
