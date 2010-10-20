namespace CityLizard
{
    using S = System;
    using T = System.Text;
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using D = System.CodeDom;
    using C = System.Collections.Generic;
    using CS = Microsoft.CSharp;

    using System.Linq;
    using Extension;

    /// <summary>
    /// sequence: A => BC
    /// choice: A => B|C
    /// </summary>
    public static class Schema
    {
        public static D.CodeCompileUnit Load(string fileName)
        {
            return Load(new X.XmlTextReader(fileName));
        }

        public class ElementSet : C.HashSet<XS.XmlSchemaElement>
        {
            public class Equality: C.EqualityComparer<XS.XmlSchemaElement>
            {
                public override bool Equals(
                    XS.XmlSchemaElement x, XS.XmlSchemaElement y)
                {
                    return x.QualifiedName == y.QualifiedName;
                }

                public override int GetHashCode(XS.XmlSchemaElement obj)
                {
                    return obj.QualifiedName.GetHashCode();
                }
            }

            public ElementSet(): base(new Equality())
            {
            }
        }

        public static D.CodeCompileUnit Load(X.XmlReader reader)
        {
            var s = new XS.XmlSchemaSet();
            s.Add(null, reader);
            s.Compile();
            var u = new D.CodeCompileUnit();
            var d = new ElementSet();
            var done = new ElementSet();
            foreach (var e in s.GlobalElementsTyped())
            {
                done.Add(e);
                SetType(d, u, e, true);
            }
            d.ExceptWith(done);
            while(true)
            {
                var newD = new ElementSet();
                foreach (var x in d)
                {
                    done.Add(x);
                    SetType(newD, u, x);
                }
                newD.ExceptWith(done);
                if (d.Count == 0)
                {
                    break;
                }
                d = newD;
            }
            var t = new System.IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            return u;
        }

        private static string Name(C.HashSet<int> s)
        {
            var r = "";
            foreach (var i in s)
            {
                r += "_" + i;
            }
            return r;
        }

        static readonly C.IEqualityComparer<C.HashSet<int>> comparer = 
            C.HashSet<int>.CreateSetComparer();

        private static string CSName(
            C.HashSet<int> self, string selfName, C.HashSet<int> key)
        {
            return comparer.Equals(key, self) ? selfName : Name(key);
        }

        private class Type
        {
            public D.CodeTypeDeclaration Declaration;

            public XS.XmlSchemaElement Element;

            public string Ns;

            private static D.CodeTypeReference String = 
                new D.CodeTypeReference(typeof(string));

            public static D.CodeExpression Add(params D.CodeExpression[] e)
            {
                var r = e[0];
                for (var i = 1; i < e.Length; ++i)
                {
                    r = new D.CodeBinaryOperatorExpression(
                        r, D.CodeBinaryOperatorType.Add, e[i]);
                }
                return r;
            }

            public static void Add(
                D.CodeStatementCollection m, params D.CodeExpression[] eList)
            {
                foreach (var e in eList)
                {
                    m.Add(
                        new D.CodeMethodInvokeExpression(
                            new D.CodeMethodReferenceExpression(
                                new D.CodeVariableReferenceExpression("R"),
                                "Append"),
                            e));
                }
            }

            private D.CodeConstructor AddConstructor()
            {
                var c = new D.CodeConstructor()
                {
                    Attributes = D.MemberAttributes.FamilyAndAssembly,
                };
                this.Declaration.Members.Add(c);
                return c;
            }

            public Type(
                D.CodeTypeDeclaration declaration, 
                string stateName, 
                XS.XmlSchemaElement element,
                string ns = null)
            {
                this.Element = element;
                this.Ns = ns;
                this.Declaration = declaration;
                this.Declaration.BaseTypes.Add(typeof(Xml.Element));
                // ctor(Xml.Element.Header H): base(H) {}
                {
                    var c = this.AddConstructor();
                    c.AddBaseParameter<Xml.Element.Header>("H");
                }
                // ctor(Xml.Element Part0, Xml.Element Child): 
                //   base(Part0, Child) {}
                {
                    var c = this.AddConstructor();
                    c.AddBaseParameter<Xml.Element>("Part0");
                    c.AddBaseParameter<Xml.Element>("Child");
                }
                // ctor(Xml.Element Part0): base(Part0) {}
                {
                    var c = this.AddConstructor();
                    c.AddBaseParameter<Xml.Element>("Part0");
                }
                //
                if (this.Element.ElementSchemaType.IsMixed)
                {
                    var returnType = new D.CodeTypeReference(stateName);
                    {
                        var property = new D.CodeMemberProperty()
                        {
                            Name = "Item",
                            Type = returnType,
                            Attributes = D.MemberAttributes.Public,
                        };
                        var pName = "text";
                        property.Parameters.Add(
                            new D.CodeParameterDeclarationExpression(
                                typeof(string),
                                pName));
                        property.GetStatements.Add(
                            new D.CodeMethodInvokeExpression(
                                new D.CodeThisReferenceExpression(),
                                "AddText",
                                new D.CodeVariableReferenceExpression(pName)));
                        property.GetStatements.Add(new D.CodeMethodReturnStatement(
                            new D.CodeThisReferenceExpression()));
                        this.Declaration.Members.Add(property);
                    }
                }
            }

            public D.CodeFieldReferenceExpression Field(string name)
            {
                return new D.CodeFieldReferenceExpression(
                    new D.CodeThisReferenceExpression(), name);
            }

            public static D.CodeObjectCreateExpression Return(
                D.CodeStatementCollection collection, string type)
            {
                var create = new D.CodeObjectCreateExpression(
                    new D.CodeTypeReference(type));
                collection.Add(new D.CodeMethodReturnStatement(create));
                return create;
            }

            public static void Return(
                D.CodeStatementCollection collection, 
                string type, 
                params D.CodeExpression[] a)
            {
                var create = Return(collection, type);
                foreach (var i in a)
                {
                    create.Parameters.Add(i);
                }
            }

            public void Comment(bool empty)
            {
                if (!empty)
                {
                    var returnType = new D.CodeTypeReference(
                        this.Declaration.Name);
                    var method = new D.CodeMemberMethod()
                    {
                        Name = "C",
                        ReturnType = returnType,
                        Attributes = D.MemberAttributes.Public,
                    };
                    var pName = "comment";
                    method.Parameters.Add(
                        new D.CodeParameterDeclarationExpression(
                            typeof(string),
                            pName));
                    method.Statements.Add(
                        new D.CodeMethodInvokeExpression(
                            new D.CodeThisReferenceExpression(),
                            "AddComment",
                            new D.CodeVariableReferenceExpression(pName)));
                    method.Statements.Add(new D.CodeMethodReturnStatement(
                        new D.CodeThisReferenceExpression()));
                    this.Declaration.Members.Add(method);
                }
            }

            public void Property(string returnTypeName, string parameter)
            {
                var returnType = new D.CodeTypeReference(returnTypeName);
                var property = new D.CodeMemberProperty()
                {
                    Name = "Item",
                    Type = returnType,
                    Attributes = D.MemberAttributes.Public,
                };
                var pName = CSharp.Name(parameter);
                property.Parameters.Add(
                    new D.CodeParameterDeclarationExpression(
                        new D.CodeTypeReference(parameter),
                        pName));
                var s = property.GetStatements;
                if (returnTypeName == this.Declaration.Name)
                {
                    s.Add(
                        new D.CodeMethodInvokeExpression(
                            new D.CodeThisReferenceExpression(),
                            "AddElement",
                            new D.CodeVariableReferenceExpression(pName)));
                    s.Add(new D.CodeMethodReturnStatement(
                        new D.CodeThisReferenceExpression()));
                }
                else
                {
                    Return(
                        property.GetStatements,
                        returnTypeName,
                        new D.CodeThisReferenceExpression(),
                        new D.CodeVariableReferenceExpression(pName));
                }
                this.Declaration.Members.Add(property);
            }

            public void SetState(
                C.KeyValuePair<C.HashSet<int>, 
                Dfa<X.XmlQualifiedName>.State> state,
                C.HashSet<int> self,
                string selfName)
            {
                foreach (var t in state.Value)
                {
                    var returnTypeName = CSName(self, selfName, t.Value);
                    this.Property(returnTypeName, CSharp.Name(t.Key.Name));
                }
            }
        }

        public class Attributes
        {
            private readonly D.CodeParameterDeclarationExpressionCollection P;
            private readonly D.CodeStatementCollection S;
            private readonly C.IEnumerable<XS.XmlSchemaAttribute> List;

            public readonly C.HashSet<string> PSet = new C.HashSet<string>();

            public Attributes(
                D.CodeParameterDeclarationExpressionCollection p,
                D.CodeStatementCollection s,
                C.IEnumerable<XS.XmlSchemaAttribute> list)
            {
                this.P = p;
                this.S = s;
                this.List = list;
            }

            public const string H = "H";

            public static readonly D.CodeVariableReferenceExpression HRef = 
                new D.CodeVariableReferenceExpression(H);

            public void Add(bool required)
            {
                var suffix = required ? "" : " = null";
                var methodName = 
                    required ? "AddRequiredAttribute" : "AddOptionalAttribute";
                foreach (var a in this.List)
                {
                    var aName = CSharp.Name(a.QualifiedName.Name);
                    if (
                        required == (a.Use == XS.XmlSchemaUse.Required) &&
                        !this.PSet.Contains(aName))
                    {
                        this.P.Add<string>(aName + suffix);
                        this.S.AddMethodInvoke(
                            HRef,
                            methodName,
                            new D.CodePrimitiveExpression(a.QualifiedName.Name),
                            new D.CodeVariableReferenceExpression(aName));
                        PSet.Add(aName);
                    }
                }
            }
        }

        public static bool Create(
            D.CodeParameterDeclarationExpressionCollection p, 
            D.CodeStatementCollection s, 
            string t, 
            C.IEnumerable<XS.XmlSchemaAttribute> attributeList,
            XS.XmlSchemaElement e,
            bool empty)
        {
            var q = e.QualifiedName;
            s.AddVariable<Xml.Element.Header>(
                Attributes.H, 
                new D.CodePrimitiveExpression(q.Namespace), 
                new D.CodePrimitiveExpression(q.Name),
                new D.CodePrimitiveExpression(empty));
            var r = false;
            if (p != null)
            {
                var attributes = new Attributes(p, s, attributeList);
                attributes.Add(true);
                r = attributes.PSet.Count > 0;
                attributes.Add(false);
            }
            Type.Return(s, t, Attributes.HRef);
            return r;
        }

        public static void SetType(
            ElementSet dictionary,
            D.CodeCompileUnit unit,
            XS.XmlSchemaElement element,
            bool isRoot = false)
        {
            var q = element.QualifiedName;
            var type = element.ElementSchemaType;

            var n = new D.CodeNamespace(CSharp.Namespace(q.Namespace));
            unit.Namespaces.Add(n);

            var common = new D.CodeTypeDeclaration("X")
            {
                IsPartial = true,
                Attributes = 
                    D.MemberAttributes.Static | D.MemberAttributes.Public,
            };
            n.Types.Add(common);

            var csName = CSharp.Name(q.Name);

            var t = new D.CodeTypeDeclaration("T")
            {
                IsPartial = true,
            };
            common.Members.Add(t);

            var c = new D.CodeTypeDeclaration(csName);
            t.Members.Add(c);

            var mixed = type.IsMixed;

            var complexType = type as XS.XmlSchemaComplexType;

            var tt = new Type(
                c, 
                csName, 
                element,
                isRoot ? element.QualifiedName.Namespace: null);

            var self = default(C.HashSet<int>);

            //bool empty = false;

            string suffix = "";
            var attributes =
                D.MemberAttributes.Static | D.MemberAttributes.Public;

            bool attributeRequired = false;

            if (complexType != null)
            {
                var fsm = new Fsm<X.XmlQualifiedName>();
                var set = new C.HashSet<int> { 0 };
                Apply(dictionary, fsm, set, complexType.Particle);
                var dfa = new Dfa<X.XmlQualifiedName>(fsm, set);
                var empty =
                    !type.IsMixed &&
                    dfa.D[new C.HashSet<int> { 0 }].Count == 0;
                tt.Comment(empty);
                //
                //
                foreach (var p in dfa.D)
                {
                    if (p.Value.Last)
                    {
                        self = p.Key;
                        break;
                    }
                }
                //
                foreach (var p in dfa.D)
                {
                    var v = p.Value;
                    if (comparer.Equals(p.Key, self))
                    {
                        tt.SetState(p, self, csName);
                    }
                    else
                    {
                        var csN = Name(p.Key);
                        var s = new D.CodeTypeDeclaration(csN);
                        c.Members.Add(s);

                        var ttt = new Type(s, csN, element);
                        ttt.SetState(p, self, csName);
                        ttt.Comment(empty);
                        //
                        if (v.Last)
                        {
                            var m = new D.CodeMemberMethod()
                            {
                                Attributes =
                                    D.MemberAttributes.Public |
                                    D.MemberAttributes.Static,
                                Name = "implicit operator " + csName,
                                ReturnType = new D.CodeTypeReference(" "),
                            };
                            m.Parameters.Add(
                                new D.CodeParameterDeclarationExpression(
                                    new D.CodeTypeReference(csN),
                                    csN));
                            var csNRef = new D.CodeVariableReferenceExpression(csN);
                            Type.Return(
                                m.Statements,
                                csName,
                                csNRef);
                            s.Members.Add(m);
                        }
                    }
                }
                if (!comparer.Equals(self, new C.HashSet<int> { 0 }))
                {
                    suffix = "._0";
                }
                var tTypeName = "T." + csName + suffix;
                var tType = new D.CodeTypeReference(tTypeName);

                {
                    var method = new D.CodeMemberMethod()
                    {
                        Name = csName + "_",
                        Attributes = attributes,
                        ReturnType = tType,
                    };
                    common.Members.Add(method);
                    attributeRequired = Create(
                        method.Parameters,
                        method.Statements,
                        tTypeName,
                        complexType.AttributeUsesTyped(),
                        element,
                        empty);
                }

                if (!attributeRequired)
                {
                    var p = new D.CodeMemberProperty()
                    {
                        Name = csName,
                        Attributes = attributes,
                        Type = tType,
                    };
                    Create(
                        null,
                        p.GetStatements,
                        tTypeName,
                        complexType.AttributeUsesTyped(),
                        element,
                        empty);
                    common.Members.Add(p);
                }
            }
            else
            {
                throw new S.Exception("simple type");
            }
        }

        public static void ApplyOne(
            C.HashSet<XS.XmlSchemaElement> dictionary,
            Fsm<X.XmlQualifiedName> fsm,
            C.ISet<int> list,
            XS.XmlSchemaParticle p)
        {
            // sequence
            {
                var sequence = p as XS.XmlSchemaSequence;
                if (sequence != null)
                {
                    foreach (var i in sequence.ItemsTyped())
                    {
                        Apply(dictionary, fsm, list, i);
                    }
                    return;
                }
            }

            // choice
            {
                var choice = p as XS.XmlSchemaChoice;
                if (choice != null)
                {
                    var x = new C.HashSet<int>(list);
                    list.Clear();
                    foreach (var i in choice.ItemsTyped())
                    {
                        var xi = new C.HashSet<int>(x);
                        Apply(dictionary, fsm, xi, i);
                        list.UnionWith(xi);
                    }
                    return;
                }
            }

            // element
            {
                var element = p as XS.XmlSchemaElement;
                if (element != null)
                {
                    dictionary.Add(element);
                    //
                    var i = fsm.AddNew(list, element.QualifiedName);
                    list.Clear();
                    list.Add(i);
                    return;
                }
            }

            //            
            throw new S.Exception(
                "unknown XmlSchemaObject type: " + p.ToString());
        }

        public static void Apply(
            C.HashSet<XS.XmlSchemaElement> dictionary,
            Fsm<X.XmlQualifiedName> fsm, 
            C.ISet<int> list,
            XS.XmlSchemaParticle p)
        {
            // group ref
            {
                var groupRef = p as XS.XmlSchemaGroupRef;
                if (groupRef != null)
                {
                    p = groupRef.Particle;
                }                
            }

            if (p == null)
            {
                return;
            }

            var min = (int)p.MinOccurs;
            var max = 
                p.MaxOccurs == decimal.MaxValue ? 
                    int.MaxValue : 
                // else
                    (int)p.MaxOccurs;

            fsm.Loop(list, x => ApplyOne(dictionary, fsm, x, p), min, max);
        }

    }
}
