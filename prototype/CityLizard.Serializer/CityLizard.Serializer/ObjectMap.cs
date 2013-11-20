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
        public interface ISerializer<T>
        {
            void Serializer(T value, XElement element);
            T Deserialize(XElement element);
        }

        public class SerializerDictionary
        {
            public readonly Dictionary<Type, object> SerializerMap = 
                new Dictionary<Type, object>();

            public void Add<T>(ISerializer<T> serializer)
            {
                SerializerMap.Add(typeof(T), serializer);
            }

            public ISerializer<T> Get<T>()
            {
                var optional = SerializerMap.Get(typeof(T));
                if(optional != null)
                {
                    return (ISerializer<T>)optional.Value;
                }
                return null;
            }
        }

        public sealed class Serializer
        {
            private readonly Dictionary<Object, int> ObjectMap =
                new Dictionary<object, int>();

            public readonly XElement ObjectMapXml = new XElement("ObjectMap");

            public void Serialize(Object value, XElement element)
            {
                var type = value.GetType();
                if (type == typeof(int))
                {
                    element.Add(new XAttribute("value", value.ToString()));
                }
                else
                {
                    foreach (var field in type.GetTypeInfo().DeclaredFields)
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
            var document = new XDocument(serializer.ObjectMapXml);
        }

        public static T Deserialize<T>(XDocument document)
        {
            return default(T); // new T();
        }
    }
}
