using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Reflection;

namespace CityLizard.Serializer
{
    public sealed class ObjectMap
    {
        public sealed class Serializer
        {
            private readonly Dictionary<Object, int> ObjectMap =
                new Dictionary<object, int>();

            public readonly XElement ObjectMapXml = new XElement("ObjectMap");

            public void Serialize(Object value, XElement element)
            {
                foreach(
                    var field in value.GetType().GetTypeInfo().DeclaredFields)
                {
                    element.Add(
                        new XElement(
                            field.Name,
                            new XAttribute(
                                "ref", Serialize(field.GetValue(value))
                            )
                        )
                    );
                }
            }

            public int Serialize(Object value)
            {
                if (value == null)
                {
                    return 0;
                }
                var id = ObjectMap.Get(value);
                if (id != null)
                {
                    return id.Value;
                }
                //
                var type = value.GetType();
                var element = new XElement(type.Name);
                //
                var count = ObjectMapXml.Elements().Count() + 1;
                ObjectMap.Add(value, count);
                ObjectMapXml.Add(element);
                //
                Serialize(value, element);
                //
                return count;
            }
        }

        public static XDocument Serialize<T>(T value)
        {
            var serializer = new Serializer();
            //
            var fieldList = typeof(T).GetRuntimeFields().Where(f => !f.IsStatic);
            //
            return new XDocument(serializer.ObjectMapXml);
        }

        public static T Deserialize<T>(XDocument document)
        {
            return default(T); // new T();
        }
    }
}
