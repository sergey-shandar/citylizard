namespace CityLizard.CodeDom.Extension
{
    using D = System.CodeDom;
    using C = Code;

    /// <summary>
    /// Statement collection extension methods.
    /// </summary>
    public static class StatementCollectionExtension
    {
        /// <summary>
        /// Add a statement.
        /// </summary>
        /// <typeparam name="S">Statement type.</typeparam>
        /// <param name="sC">Collection.</param>
        /// <param name="s">Statement.</param>
        /// <returns>Statement.</returns>
        public static S AddStatement<S>(this D.CodeStatementCollection sC, S s)
            where S: D.CodeStatement
        {
            sC.Add(s);
            return s;
        }

        /// <summary>
        /// Add an expression.
        /// </summary>
        /// <typeparam name="E">Expression type.</typeparam>
        /// <param name="sC">Collection.</param>
        /// <param name="e">Expression.</param>
        /// <returns>Expression.</returns>
        public static E AddExpression<E>(this D.CodeStatementCollection sC, E e)
            where E: D.CodeExpression
        {
            sC.Add(e);
            return e;
        }

        /// <summary>
        /// Add a new variable declaration.
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="sC">Collection.</param>
        /// <param name="name">Name of the variable.</param>
        /// <param name="eP">Constructor parameters.</param>
        /// <returns>Created variable declaration.</returns>
        public static D.CodeVariableDeclarationStatement AddVariable<T>(
            this D.CodeStatementCollection sC,
            string name,
            params D.CodeExpression[] eP)
        {
            return sC.AddStatement(C.VariableDeclarationStatement<T>(name, eP));
        }

        /// <summary>
        /// Add a new method invokation.
        /// </summary>
        /// <param name="sC">Collection.</param>
        /// <param name="e">Target.</param>
        /// <param name="name">Method name.</param>
        /// <param name="eP">Method parameters.</param>
        /// <returns>Created method invokation.</returns>
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
