//------------------------------------------------------------------------------
// <copyright file="Code.cs" company="CityLizard">
//     Copyright (c) CityLizard. All rights reserved.
// </copyright>
// <author>Sergey Shandar</author>
// <summary>
//     Build utilities.
// </summary>
//------------------------------------------------------------------------------
namespace CityLizard.CodeDom
{
    using D = System.CodeDom;

    /// <summary>
    /// CodeDom utility.
    /// </summary>
    public static class Code
    {
        #region AttributeArgument

        /// <summary>
        /// Create an attribute argument.
        /// </summary>
        /// <param name="e">Expression.</param>
        /// <returns>Created an attribute argument.</returns>
        public static D.CodeAttributeArgument AttributeArgument(
            D.CodeExpression e)
        {
            return new D.CodeAttributeArgument(e);
        }

        #endregion

        #region AttributeDeclaration

        /// <summary>
        /// Create an attribute declaration.
        /// <code>
        /// [MyAttribute("parameter")]
        /// </code>
        /// </summary>
        /// <typeparam name="T">Attribute type.</typeparam>
        /// <param name="aP">Attribute parameters.</param>
        /// <returns>Created attribute declaration.</returns>
        public static D.CodeAttributeDeclaration AttributeDeclaration<T>(
            params D.CodeAttributeArgument[] aP)
        {
            return new D.CodeAttributeDeclaration(TypeReference<T>(), aP);
        }

        #endregion

        #region MethodInvokeExpression

        /// <summary>
        /// Create a method invokation.
        /// <code>
        /// this.Method("parameter0", "parameter1");
        /// </code>
        /// </summary>
        /// <param name="e">Target.</param>
        /// <param name="name">Method name.</param>
        /// <param name="eP">Method parameters.</param>
        /// <returns>Created method invokation.</returns>
        public static D.CodeMethodInvokeExpression MethodInvokeExpression(
            D.CodeExpression e, string name, params D.CodeExpression[] eP)
        {
            return new D.CodeMethodInvokeExpression(e, name, eP);
        }

        #endregion

        #region Namespace

        /// <summary>
        /// Create a namespace.
        /// </summary>
        /// <param name="name">Namespace name.</param>
        /// <returns>Created namespace.</returns>
        public static D.CodeNamespace Namespace(string name)
        {
            return new D.CodeNamespace(name);
        }

        #endregion

        #region ObjectCreateExpression

        /// <summary>
        /// Create an object creation.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="eP">Constructor parameters.</param>
        /// <returns>Created object creation.</returns>
        public static D.CodeObjectCreateExpression ObjectCreateExpression<T>(
            params D.CodeExpression[] eP)
        {
            return new D.CodeObjectCreateExpression(typeof(T), eP);
        }

        #endregion

        #region ParameterDeclarationExpression

        /// <summary>
        /// Parameter declaration.
        /// </summary>
        /// <typeparam name="T">Parameter type.</typeparam>
        /// <param name="name">Parameter name.</param>
        /// <returns>Created parameter declaration.</returns>
        public static D.CodeParameterDeclarationExpression
            ParameterDeclarationExpression<T>(string name)
        {
            return new D.CodeParameterDeclarationExpression(typeof(T), name);
        }

        #endregion

        #region PrimitiveExpression

        /// <summary>
        /// Create a constant.
        /// </summary>
        /// <typeparam name="T">Type of the constant.</typeparam>
        /// <param name="v">Value.</param>
        /// <returns>Created constant.</returns>
        public static D.CodePrimitiveExpression PrimitiveExpression<T>(T v)
        {
            return new D.CodePrimitiveExpression(v);
        }

        #endregion

        #region TypeReference

        /// <summary>
        /// Create a type reference.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Created type reference.</returns>
        public static D.CodeTypeReference TypeReference<T>()
        {
            return new D.CodeTypeReference(typeof(T));
        }

        #endregion

        #region VariableDeclarationStatement

        /// <summary>
        /// Create a variable declararion.
        /// </summary>
        /// <typeparam name="T">Type of the variable.</typeparam>
        /// <param name="name">Name of the variable.</param>
        /// <param name="eP">Constructor parameters.</param>
        /// <returns>Created declaration.</returns>
        public static D.CodeVariableDeclarationStatement
            VariableDeclarationStatement<T>(
                string name, params D.CodeExpression[] eP)
        {
            return new D.CodeVariableDeclarationStatement(
                TypeReference<T>(), name, ObjectCreateExpression<T>(eP));
        }

        #endregion

        #region VariableReferenceExpression

        /// <summary>
        /// Creates a reference on variable.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        /// <returns>Created reference.</returns>
        public static D.CodeVariableReferenceExpression
            VariableReferenceExpression(string name)
        {
            return new D.CodeVariableReferenceExpression(name);
        }

        #endregion
    }
}
