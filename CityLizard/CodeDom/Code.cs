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
    }
}
