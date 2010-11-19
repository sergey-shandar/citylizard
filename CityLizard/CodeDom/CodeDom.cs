namespace CityLizard.CodeDom
{
    using S = System;
    using D = System.CodeDom;
    using C = System.Collections.Generic;

    public partial class CodeDom
    {
        public static class T
        {
            public class Unit : D.CodeCompileUnit
            {
                public void Append(Namespace Namespace)
                {
                    this.Namespaces.Add(Namespace);
                }

                public Unit this[Namespace Namespace]
                {
                    get
                    {
                        this.Append(Namespace);
                        return this;
                    }
                }
            }

            public class Namespace : D.CodeNamespace
            {
                public Namespace(string Name): base(Name)
                {
                }

                public void Append(Type Type)
                {
                    this.Types.Add(Type);
                }

                public Namespace this[Type Type]
                {
                    get
                    {
                        this.Append(Type);
                        return this;
                    }
                }
            }

            public class Type : D.CodeTypeDeclaration
            {
                public Type(
                    string Name,
                    bool IsPartial = false,
                    D.MemberAttributes Attributes = 
                        default(D.MemberAttributes)):
                    base(Name)
                {
                    this.IsPartial = IsPartial;
                    this.Attributes = Attributes;
                }

                public void Append(Type Type)
                {
                    this.Members.Add(Type);
                }

                public Type this[Type Type]
                {
                    get
                    {
                        this.Append(Type);
                        return this;
                    }
                }

                public void Append(TypeRef TypeRef)
                {
                    this.BaseTypes.Add(TypeRef);
                }

                public Type this[TypeRef TypeRef]
                {
                    get
                    {
                        this.Append(TypeRef);
                        return this;
                    }
                }

                public void Append(Method Method)
                {
                    this.Members.Add(Method);
                }

                public Type this[Method Method]
                {
                    get
                    {
                        this.Append(Method);
                        return this;
                    }
                }

                public void Append(Constructor Constructor)
                {
                    this.Members.Add(Constructor);
                }

                public Type this[Constructor Constructor]
                {
                    get
                    {
                        this.Append(Constructor);
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

                public void Append(Parameter Parameter)
                {
                    this.Parameters.Add(Parameter);
                }

                public Method this[Parameter Parameter]
                {
                    get
                    {
                        this.Append(Parameter);
                        return this;
                    }
                }

                public void Append(C.IEnumerable<Parameter> ParameterList)
                {
                    foreach (var p in ParameterList)
                    {
                        this.Append(p);
                    }
                }

                public Method this[C.IEnumerable<Parameter> ParameterList]
                {
                    get
                    {
                        this.Append(ParameterList);
                        return this;
                    }
                }

                public void Append(Return Return)
                {
                    this.Statements.Add(Return);
                }

                public Method this[Return Return]
                {
                    get
                    {
                        this.Append(Return);
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

                public void Append(Parameter Parameter)
                {
                    this.Parameters.Add(Parameter);
                }

                public Constructor this[Parameter Parameter]
                {
                    get
                    {
                        this.Append(Parameter);
                        return this;
                    }
                }

                public void Append(C.IEnumerable<Parameter> ParameterList)
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
                        this.Append(ParameterList);
                        return this;
                    }
                }

                public void Append(VariableRef VariableRef)
                {
                    this.BaseConstructorArgs.Add(VariableRef);
                }

                public Constructor this[VariableRef VariableRef]
                {
                    get
                    {
                        this.Append(VariableRef);
                        return this;
                    }
                }

                public void Append(Invoke Invoke)
                {
                    this.Statements.Add(Invoke);
                }

                public Constructor this[Invoke Invoke]
                {
                    get
                    {
                        this.Append(Invoke);
                        return this;
                    }
                }

                public void Append(C.IEnumerable<Invoke> InvokeList)
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
                        this.Append(InvokeList);
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

                public Parameter(S.Type Type, string Name, Primitive Value = null):
                    base(Type, ToName(Name, Value))
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
            }

            public class New : D.CodeObjectCreateExpression
            {
                public New(TypeRef TypeRef): base(TypeRef)
                {
                }

                public void Append(VariableRef VariableRef)
                {
                    this.Parameters.Add(VariableRef);
                }

                public New this[VariableRef VariableRef]
                {
                    get
                    {
                        this.Append(VariableRef);
                        return this;
                    }
                }

                public void Append(This This)
                {
                    this.Parameters.Add(This);
                }

                public New this[This This]
                {
                    get
                    {
                        this.Append(This);
                        return this;
                    }
                }

                public void Append(C.IEnumerable<VariableRef> VariableRefList)
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
                        this.Append(VariableRefList);
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

                public void Append(VariableRef VariableRef)
                {
                    this.Parameters.Add(VariableRef);
                }

                public Invoke this[VariableRef VariableRef]
                {
                    get
                    {
                        this.Append(VariableRef);
                        return this;
                    }
                }

                public void Append(Primitive Primitive)
                {
                    this.Parameters.Add(Primitive);
                }

                public Invoke this[Primitive Primitive]
                {
                    get
                    {
                        this.Append(Primitive);
                        return this;
                    }
                }
            }
        }

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
            D.MemberAttributes Attributes = default(D.MemberAttributes))
        {
            return new T.Type(Name, IsPartial, Attributes);
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

        public T.Constructor Constructor(
            D.MemberAttributes Attributes = default(D.MemberAttributes))
        {
            return new T.Constructor(Attributes);
        }

        public T.Parameter Parameter<U>(string Name, T.Primitive Value = null)
        {
            return new T.Parameter(typeof(U), Name, Value);
        }

        public T.VariableRef VariableRef(string Name)
        {
            return new T.VariableRef(Name);
        }

        public T.Return Return(T.New New)
        {
            return new T.Return(New);
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
    }
}
