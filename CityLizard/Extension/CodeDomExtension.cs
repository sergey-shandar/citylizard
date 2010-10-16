namespace CityLizard.Extension
{
    using D = System.CodeDom;
    using S = System;

    using System.Linq;

    public static class CodeDomExtension
    {
        public static T AddStatement<T>(
            this D.CodeStatementCollection statementList, T t)
            where T: D.CodeStatement
        {
            statementList.Add(t);
            return t;
        }

        public static T AddExpression<T>(
            this D.CodeStatementCollection statementList, T t)
            where T : D.CodeExpression
        {
            statementList.Add(t);
            return t;
        }

        public static D.CodeVariableDeclarationStatement AddVariableNew<T>(
            this D.CodeStatementCollection statementList,
            string name,
            params D.CodeExpression[] p)
        {
            var t = typeof(T);
            return statementList.AddStatement(
                new D.CodeVariableDeclarationStatement(
                    new D.CodeTypeReference(t),
                    name,
                    new D.CodeObjectCreateExpression(t, p)));
        }

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

        public static D.CodeMethodInvokeExpression AddMethodInvoke(
            this D.CodeStatementCollection statementList,
            D.CodeExpression e,
            string name,
            params D.CodeExpression[] paramVarList)
        {
            return statementList.AddExpression(new D.CodeMethodInvokeExpression(
                e, name, paramVarList));
        }
    }
}
