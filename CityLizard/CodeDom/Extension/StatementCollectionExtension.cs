namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;

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
            var t = typeof(T);
            return sC.AddStatement(
                new D.CodeVariableDeclarationStatement(
                    new D.CodeTypeReference(t),
                    name,
                    new D.CodeObjectCreateExpression(t, eP)));
        }

        public static D.CodeMethodInvokeExpression AddMethodInvoke(
            this D.CodeStatementCollection sC,
            D.CodeExpression e,
            string name,
            params D.CodeExpression[] eP)
        {
            return sC.AddExpression(new D.CodeMethodInvokeExpression(
                e, name, eP));
        }
    }
}
