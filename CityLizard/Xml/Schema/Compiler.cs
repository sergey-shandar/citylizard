namespace CityLizard.Xml.Schema
{
    using X = System.Xml;
    using XS = System.Xml.Schema;
    using C = System.Collections.Generic;

    using D = CodeDom.CodeDom;

    using Extension;

    public class Compiler: D
    {
        T.Unit U;
        ElementSet Done;
        C.IEnumerable<XS.XmlSchemaElement> ToDo;

        private bool AddElementSet()
        {
            var newToDo = new ElementSet();
            foreach (var x in this.ToDo)
            {
                this.Done.Add(x);
                // SetType(newToDo, u, x);
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
            this.ToDo = schema.GlobalElementsTyped();
            this.Done = new ElementSet();
            while (AddElementSet()) { }
            //
            return this.U;
        }
    }
}
