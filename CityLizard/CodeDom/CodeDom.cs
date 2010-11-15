namespace CityLizard.CodeDom
{
    using S = System;
    using D = System.CodeDom;

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
                    D.MemberAttributes Attributes = default(D.MemberAttributes)): 
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
    }
}
