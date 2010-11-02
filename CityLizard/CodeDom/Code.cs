namespace CityLizard.CodeDom
{
    using D = System.CodeDom;

    public static class Code
    {
        #region AttributeArgument

        public static D.CodeAttributeArgument AttributeArgument(
            D.CodeExpression e)
        {
            return new D.CodeAttributeArgument(e);
        }

        #endregion

        #region AttributeDeclaration

        public static D.CodeAttributeDeclaration AttributeDeclaration<T>(
            params D.CodeAttributeArgument[] aP)
        {
            return new D.CodeAttributeDeclaration(TypeReference<T>(), aP);
        }

        #endregion

        #region MethodInvokeExpression

        public static D.CodeMethodInvokeExpression MethodInvokeExpression(
            D.CodeExpression e, string name, params D.CodeExpression[] eP)
        {
            return new D.CodeMethodInvokeExpression(e, name, eP);
        }

        #endregion

        #region Namespace

        public static D.CodeNamespace Namespace(string name)
        {
            return new D.CodeNamespace(name);
        }

        #endregion

        #region ObjectCreateExpression

        public static D.CodeObjectCreateExpression ObjectCreateExpression<T>(
            params D.CodeExpression[] eP)
        {
            return new D.CodeObjectCreateExpression(typeof(T), eP);
        }

        #endregion

        #region ParameterDeclarationExpression

        public static D.CodeParameterDeclarationExpression
            ParameterDeclarationExpression<T>(string name)
        {
            return new D.CodeParameterDeclarationExpression(typeof(T), name);
        }

        #endregion

        #region PrimitiveExpression

        public static D.CodePrimitiveExpression PrimitiveExpression<T>(T v)
        {
            return new D.CodePrimitiveExpression(v);
        }

        #endregion

        #region TypeReference

        public static D.CodeTypeReference TypeReference<T>()
        {
            return new D.CodeTypeReference(typeof(T));
        }

        #endregion

        #region VariableDeclarationStatement

        public static D.CodeVariableDeclarationStatement
            VariableDeclarationStatement<T>(
                string name, params D.CodeExpression[] eP)
        {
            return new D.CodeVariableDeclarationStatement(
                TypeReference<T>(), name, ObjectCreateExpression<T>(eP));
        }

        #endregion

        #region VariableReferenceExpression

        public static D.CodeVariableReferenceExpression
            VariableReferenceExpression(string name)
        {
            return new D.CodeVariableReferenceExpression(name);
        }

        #endregion
    }
}
