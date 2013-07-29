namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using C = System.Collections.Generic;
    using CD = System.CodeDom;
    using S = System;
    using A = System.CodeDom.MemberAttributes;

    using CS = CodeDom.CSharp;
    using D = CodeDom.Code;
    using E = Xml.Linked.Element;
    using F = Fsm;

    using Extension;
    using System.Linq;

    public class Compiler: D
    {
        T.Unit U;
        ElementSet Done;
        C.IEnumerable<XS.XmlSchemaElement> ToDo;

        private class Attributes
        {
            public readonly C.List<T.Parameter> Parameters = 
                new C.List<T.Parameter>();
            public readonly C.List<T.Invoke> Invokes = new C.List<T.Invoke>();
            public C.IEnumerable<XS.XmlSchemaAttribute> A;
        }

        private bool AddAttributes(Attributes attributes, bool required)
        {
            var result = false;
            foreach (
                var a in
                attributes.A.Where(
                    x =>
                        x.QualifiedName.Namespace == "" &&
                        (x.Use == XS.XmlSchemaUse.Required) == required))
            {
                var name = a.QualifiedName.Name;
                var p = Parameter(
                    TypeRef<string>(),
                    CS.Name.Cast(name),
                    required ? null : Primitive(null));
                attributes.Parameters.Add(p);
                attributes.Invokes.Add(
                    Invoke(
                        required ?
                            "AddRequiredAttribute" : "AddOptionalAttribute")
                        [Primitive(name)]
                        [p.Ref()]);
                result = true;
            }
            return result;
        }

        private static readonly C.IEqualityComparer<C.HashSet<int>> comparer =
            C.HashSet<int>.CreateSetComparer();

        private static Fsm.Name Start = new Fsm.Name { 0 };

        private static string Name(C.HashSet<int> s)
        {
            var r = "";
            foreach (var i in s)
            {
                r += "_" + i;
            }
            return r;
        }

        private static string Name(
            C.HashSet<int> self, string selfName, C.HashSet<int> key)
        {
            return comparer.Equals(key, self) ? selfName : Name(key);
        }

        private void AddProperty(
            T.Type type,
            T.TypeRef result,
            T.Parameter parameter,
            T.Get get)            
        {
            type.Add(
                Property("Item", result, A.Public)
                    [parameter]
                    [get]);
        }

        private void AddProperty(
            T.Type type, 
            T.TypeRef result, 
            T.TypeRef parameter, 
            string parameterName)
        {
            var p = Parameter(parameter, parameterName);
            this.AddProperty(
                type,
                result,
                p,
                Get()
                    [Invoke("Add")[p.Ref()]]
                    [Return(This())]);
        }

        /// <summary>
        /// A -X-> A: A.P |= X[1; +Inf)
        /// A -X-> B: B.P |= A.P + X
        /// A -X-> ... B -Y-> A: A |=
        ///
        /// </summary>
        /// <param name="newToDo"></param>
        /// <param name="element"></param>
        /// <param name="isRoot"></param>
        private void SetType(
            ElementSet newToDo,
            XS.XmlSchemaElement element,
            bool isRoot = false)
        {
            C.Dictionary<Fsm.Name, Fsm.Dfa<System.Xml.XmlQualifiedName>.State> dictionaryX = null;
        }

        private bool AddElementSet()
        {
            var newToDo = new ElementSet();
            foreach (var x in this.ToDo)
            {
                this.Done.Add(x);
                this.SetType(newToDo, x);
            }
            newToDo.ExceptWith(this.Done);
            this.ToDo = newToDo;
            return newToDo.Count != 0;
        }

        public T.Unit Load(X.XmlReader reader)
        {
            var schema = new XS.XmlSchemaSet();
            schema.Add(null, reader);
            schema.Compile();
            //
            this.U = Unit();
            //
            this.ToDo = schema.GlobalElementsTyped();
            this.Done = new ElementSet();
            while (this.AddElementSet()) { }
            //
            return this.U;
        }

        public T.Unit Load(string fileName)
        {
            using(var xmlReader = new X.XmlTextReader(fileName))
            {
                return this.Load(xmlReader);
            }
        }
    }
}
