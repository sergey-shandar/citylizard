//------------------------------------------------------------------------------
// <copyright file="CodeDom.cs" company="CityLizard">
//     Copyright (c) CityLizard. All rights reserved.
// </copyright>
// <author>Sergey Shandar</author>
// <summary>
//     Build utilities.
// </summary>
//------------------------------------------------------------------------------
namespace CityLizard.CodeDom
{
    using System.Linq;

    using C = System.Collections.Generic;
    using D = System.CodeDom;
    using S = System;

    /// <summary>
    /// Code DOM.
    /// </summary>
    public partial class Code
    {
        public T.Unit Unit()
        {
            return new T.Unit();
        }

        public T.Namespace Namespace(string Name)
        {
            return new T.Namespace(Name);
        }

        public T.Type Type(
            string Name,
            bool IsPartial = false,
            bool IsStruct = false,
            D.MemberAttributes Attributes = default(D.MemberAttributes))
        {
            return new T.Type(Name, IsPartial, IsStruct, Attributes);
        }

        public T.TypeRef TypeRef<S>()
        {
            return new T.TypeRef(typeof(S));
        }

        public T.TypeRef TypeRef(string Name)
        {
            return new T.TypeRef(Name);
        }

        public T.Method Method(
            string Name,
            D.MemberAttributes Attributes = default(D.MemberAttributes),
            T.TypeRef Return = null)
        {
            return new T.Method(Name, Attributes, Return);
        }

        public T.Method Method(
            T.TypeRef Return, 
            D.MemberAttributes Attributes = default(D.MemberAttributes))
        {
            return new T.Method(Return, Attributes);
        }

        public T.Constructor Constructor(
            D.MemberAttributes Attributes = default(D.MemberAttributes))
        {
            return new T.Constructor(Attributes);
        }

        public T.Get Get()
        {
            return new T.Get();
        }

        public T.Property Property(
            string Name,
            T.TypeRef Type,
            D.MemberAttributes Attributes = default(D.MemberAttributes))
        {
            return new T.Property(Name, Type, Attributes);
        }

        public T.Parameter Parameter(
            T.TypeRef TypeRef, string Name, T.Primitive Value = null)
        {
            return new T.Parameter(TypeRef, Name, Value);
        }

        public T.VariableRef VariableRef(string Name)
        {
            return new T.VariableRef(Name);
        }

        public T.Return Return(T.New New)
        {
            return new T.Return(New);
        }

        public T.Return Return(T.This This)
        {
            return new T.Return(This);
        }

        public T.New New(T.TypeRef TypeRef)
        {
            return new T.New(TypeRef);
        }

        public T.This This()
        {
            return new T.This();
        }

        public T.Primitive Primitive(object Value)
        {
            return new T.Primitive(Value);
        }

        public T.Invoke Invoke(string Name)
        {
            return new T.Invoke(Name);
        }

        public T.Attribute Attribute<X>(
            params object[] Arguments)
        {
            return new T.Attribute(
                TypeRef<X>(), Arguments.Select(x => Primitive(x)).ToArray());
        }

        public static class T
        {
            public class Unit : D.CodeCompileUnit
            {
                public void Add(Namespace Namespace)
                {
                    this.Namespaces.Add(Namespace);
                }

                public Unit this[Namespace Namespace]
                {
                    get
                    {
                        this.Add(Namespace);
                        return this;
                    }
                }

                public void Add(Attribute Attribute)
                {
                    this.AssemblyCustomAttributes.Add(Attribute);
                }

                public Unit this[Attribute Attribute]
                {
                    get
                    {
                        this.Add(Attribute);
                        return this;
                    }
                }
            }

            public class Attribute : D.CodeAttributeDeclaration
            {
                public Attribute(
                    TypeRef TypeRef, 
                    params Primitive[] Arguments):
                    base(
                        TypeRef, 
                        Arguments.Select(
                            x => new D.CodeAttributeArgument(x)).ToArray())
                {
                }
            }

            public class Namespace : D.CodeNamespace
            {
                public Namespace(string Name): base(Name)
                {
                }

                public void Add(Type Type)
                {
                    this.Types.Add(Type);
                }

                public Namespace this[Type Type]
                {
                    get
                    {
                        this.Add(Type);
                        return this;
                    }
                }
            }

            public class Type : D.CodeTypeDeclaration
            {
                public Type(
                    string Name,
                    bool IsPartial = false,
                    bool IsStruct = false,
                    D.MemberAttributes Attributes = 
                        default(D.MemberAttributes)):
                    base(Name)
                {
                    this.IsPartial = IsPartial;
                    this.IsStruct = IsStruct;
                    this.Attributes = Attributes;
                }

                public void Add(Type Type)
                {
                    this.Members.Add(Type);
                }

                public Type this[Type Type]
                {
                    get
                    {
                        this.Add(Type);
                        return this;
                    }
                }

                public void Add(TypeRef TypeRef)
                {
                    this.BaseTypes.Add(TypeRef);
                }

                public Type this[TypeRef TypeRef]
                {
                    get
                    {
                        this.Add(TypeRef);
                        return this;
                    }
                }

                public void Add(Method Method)
                {
                    this.Members.Add(Method);
                }

                public Type this[Method Method]
                {
                    get
                    {
                        this.Add(Method);
                        return this;
                    }
                }

                public void Add(Constructor Constructor)
                {
                    this.Members.Add(Constructor);
                }

                public Type this[Constructor Constructor]
                {
                    get
                    {
                        this.Add(Constructor);
                        return this;
                    }
                }

                public void Add(Property Property)
                {
                    this.Members.Add(Property);
                }

                public Type this[Property Property]
                {
                    get
                    {
                        this.Add(Property);
                        return this;
                    }
                }
            }

            public class TypeRef : D.CodeTypeReference
            {
                public TypeRef(S.Type Type): base(Type)
                {
                }

                public TypeRef(string Name): base(Name)
                {
                }
            }

            public class Method : D.CodeMemberMethod
            {
                public Method(
                    string Name, 
                    D.MemberAttributes Attributes = default(D.MemberAttributes),
                    TypeRef Return = null)
                {
                    this.Name = Name;
                    this.Attributes = Attributes;
                    this.ReturnType = Return;
                }

                public Method(
                    TypeRef Return, 
                    D.MemberAttributes Attributes = default(D.MemberAttributes))
                {
                    this.Name = "implicit operator " + Return.BaseType;
                    this.Attributes = Attributes | D.MemberAttributes.Static;
                    this.ReturnType = new D.CodeTypeReference(" ");
                }

                public void Add(Parameter Parameter)
                {
                    this.Parameters.Add(Parameter);
                }

                public Method this[Parameter Parameter]
                {
                    get
                    {
                        this.Add(Parameter);
                        return this;
                    }
                }

                public void Add(C.IEnumerable<Parameter> ParameterList)
                {
                    foreach (var p in ParameterList)
                    {
                        this.Add(p);
                    }
                }

                public Method this[C.IEnumerable<Parameter> ParameterList]
                {
                    get
                    {
                        this.Add(ParameterList);
                        return this;
                    }
                }

                public void Add(Return Return)
                {
                    this.Statements.Add(Return);
                }

                public Method this[Return Return]
                {
                    get
                    {
                        this.Add(Return);
                        return this;
                    }
                }

                public void Add(Invoke Invoke)
                {
                    this.Statements.Add(Invoke);
                }

                public Method this[Invoke Invoke]
                {
                    get
                    {
                        this.Add(Invoke);
                        return this;
                    }
                }
            }

            public class Constructor : D.CodeConstructor
            {
                public Constructor(
                    D.MemberAttributes Attributes = default(D.MemberAttributes))
                {
                    this.Attributes = Attributes;
                }

                public void Add(Parameter Parameter)
                {
                    this.Parameters.Add(Parameter);
                }

                public Constructor this[Parameter Parameter]
                {
                    get
                    {
                        this.Add(Parameter);
                        return this;
                    }
                }

                public void Add(C.IEnumerable<Parameter> ParameterList)
                {
                    foreach (var p in ParameterList)
                    {
                        this.Parameters.Add(p);
                    }
                }

                public Constructor this[C.IEnumerable<Parameter> ParameterList]
                {
                    get
                    {
                        this.Add(ParameterList);
                        return this;
                    }
                }

                public void Add(VariableRef VariableRef)
                {
                    this.BaseConstructorArgs.Add(VariableRef);
                }

                public Constructor this[VariableRef VariableRef]
                {
                    get
                    {
                        this.Add(VariableRef);
                        return this;
                    }
                }

                public void Add(Primitive Primitive)
                {
                    this.BaseConstructorArgs.Add(Primitive);
                }

                public Constructor this[Primitive Primitive]
                {
                    get
                    {
                        this.Add(Primitive);
                        return this;
                    }
                }

                public void Add(Invoke Invoke)
                {
                    this.Statements.Add(Invoke);
                }

                public Constructor this[Invoke Invoke]
                {
                    get
                    {
                        this.Add(Invoke);
                        return this;
                    }
                }

                public void Add(C.IEnumerable<Invoke> InvokeList)
                {
                    foreach (var i in InvokeList)
                    {
                        this.Statements.Add(i);
                    }
                }

                public Constructor this[C.IEnumerable<Invoke> InvokeList]
                {
                    get
                    {
                        this.Add(InvokeList);
                        return this;
                    }
                }
            }

            public class Get: D.CodeStatementCollection
            {
                public C.IEnumerable<D.CodeStatement> Typed
                {
                    get
                    {
                        return this.Cast<D.CodeStatement>();
                    }
                }

                public void Add(Return Return)
                {
                    base.Add(Return);
                }

                public Get this[Return Return]
                {
                    get
                    {
                        this.Add(Return);
                        return this;
                    }
                }

                public void Add(Invoke Invoke)
                {
                    base.Add(Invoke);
                }

                public Get this[Invoke Invoke]
                {
                    get
                    {
                        this.Add(Invoke);
                        return this;
                    }
                }
            }

            public class Property : D.CodeMemberProperty
            {
                public Property(
                    string Name, 
                    TypeRef Type,
                    D.MemberAttributes Attributes = default(D.MemberAttributes))
                {
                    this.Name = Name;
                    this.Type = Type;
                    this.Attributes = Attributes;
                }

                public void Add(Parameter Parameter)
                {
                    this.Parameters.Add(Parameter);
                }

                public Property this[Parameter Parameter]
                {
                    get
                    {
                        this.Add(Parameter);
                        return this;
                    }
                }

                public void Add(Get Get)
                {
                    this.HasGet = true;
                    foreach(var i in Get.Typed)
                    {
                        this.GetStatements.Add(i);
                    }
                }

                public Property this[Get Get]
                {
                    get
                    {
                        this.Add(Get);
                        return this;
                    }
                }
            }

            public class Parameter : D.CodeParameterDeclarationExpression
            {
                public readonly new string Name;

                public readonly Primitive Value;

                private static string ToName(string Name, Primitive Value)
                {
                    return
                        Name +
                            (Value == null ?
                                "" :
                                " = " +
                                (Value.Value == null ?
                                    "null" : Value.Value.ToString()));
                }

                public Parameter(
                    TypeRef TypeRef, string Name, Primitive Value = null) :
                    base(TypeRef, ToName(Name, Value))
                {
                    this.Name = Name;
                    this.Value = Value;
                }

                public VariableRef Ref()
                {
                    return new VariableRef(this.Name);
                }
            }

            public class VariableRef : D.CodeVariableReferenceExpression
            {
                public VariableRef(string Name): base(Name)
                {
                }
            }

            public class Return : D.CodeMethodReturnStatement
            {
                public Return(New New): base(New)
                {
                }

                public Return(This This): base(This)
                {
                }
            }

            public class New : D.CodeObjectCreateExpression
            {
                public New(TypeRef TypeRef): base(TypeRef)
                {
                }

                public void Add(VariableRef VariableRef)
                {
                    this.Parameters.Add(VariableRef);
                }

                public New this[VariableRef VariableRef]
                {
                    get
                    {
                        this.Add(VariableRef);
                        return this;
                    }
                }

                public void Add(This This)
                {
                    this.Parameters.Add(This);
                }

                public New this[This This]
                {
                    get
                    {
                        this.Add(This);
                        return this;
                    }
                }

                public void Add(C.IEnumerable<VariableRef> VariableRefList)
                {
                    foreach (var r in VariableRefList)
                    {
                        this.Parameters.Add(r);
                    }
                }

                public New this[C.IEnumerable<VariableRef> VariableRefList]
                {
                    get
                    {
                        this.Add(VariableRefList);
                        return this;
                    }
                }
            }

            public class This : D.CodeThisReferenceExpression
            {
                public This()
                {
                }
            }

            public class Primitive : D.CodePrimitiveExpression
            {
                public Primitive(object Value): base(Value)
                {
                }
            }

            public class Invoke : D.CodeMethodInvokeExpression
            {
                public Invoke(string Name): base(new T.This(), Name)
                {
                }

                public void Add(VariableRef VariableRef)
                {
                    this.Parameters.Add(VariableRef);
                }

                public Invoke this[VariableRef VariableRef]
                {
                    get
                    {
                        this.Add(VariableRef);
                        return this;
                    }
                }

                public void Add(Primitive Primitive)
                {
                    this.Parameters.Add(Primitive);
                }

                public Invoke this[Primitive Primitive]
                {
                    get
                    {
                        this.Add(Primitive);
                        return this;
                    }
                }
            }
        }
    }
}
