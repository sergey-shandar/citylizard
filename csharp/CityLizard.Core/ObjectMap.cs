using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CityLizard.Collections;
using CityLizard.Policy;
using CityLizard.Xml.Linked.Element;
using Type = System.Type;

namespace CityLizard.ObjectMap
{
    public static class Map
    {
        public static XDocument Append(
            this XDocument document, XElement element)
        {
            document.Add(element);
            return document;
        }

        public static XElement Append(this XElement root, XElement element)
        {
            root.Add(element);
            return root;
        }

        public static XElement Append(
            this XElement root, string attributeName, string value)
        {
            root.Add(new XAttribute(attributeName, value));
            return root;
        }

        private class Config<T>
        {
            public readonly Func<object, ulong> IdMap; 
            public readonly XElement Element;
            public readonly T Value;

            public Config(Func<object, ulong> idMap, XElement element, T value)
            {
                IdMap = idMap;
                Element = element;
                Value = value;
            }
        }

        private static void Append<T>(
            this Dictionary<Type, Action<Config<object>>> map,
            Action<Config<T>> serialize)
        {
            map.Add(
                typeof(T),
                c => serialize(new Config<T>(c.IdMap, c.Element, (T)c.Value)));
        }

        private static void AppendValue<T>(
            this Dictionary<Type, Action<Config<object>>> map,
            Func<T, string> toString)
        {
            map.Append<T>(c => c.Element.Append("value", toString(c.Value)));
        }

        private static void AppendRange<T, P>(
            this Dictionary<Type, Action<Config<object>>> map)
            where T: struct, IComparable<T>
            where P: struct, IRange<T>
        {
            map.AppendValue<T>(v => new P().ToId(v));
        }

        private static Func<Type, Action<Config<object>>> CreateTypeMap()
        {
            var map = new Dictionary<Type, Action<Config<object>>>();
            map.AppendRange<byte, I>();
            map.AppendRange<sbyte, I>();
            map.AppendRange<ushort, I>();
            map.AppendRange<short, I>();
            map.AppendRange<uint, I>();
            map.AppendRange<int, I>();
            map.AppendRange<ulong, I>();
            map.AppendRange<long, I>();
            map.AppendRange<bool, I>();
            map.AppendRange<char, I>();
            map.AppendRange<float, I>();
            map.AppendRange<double, I>();
            map.AppendRange<decimal, I>();
            map.AppendValue<string>(v => v);
            return map.Cached(
                type =>
                {
                    if (type.IsEnum)
                    {
                        var underlyingType = Enum.GetUnderlyingType(type);
                        return config =>
                        {
                            /* TODO: enum */
                        };
                    }
                    if (type.IsArray)
                    {
                        return config =>
                        {
                            /* TODO: array */
                        };
                    }
                    var fieldList =
                        type.GetFields().Where(f => !f.IsStatic).ToArray();
                    return config =>
                    {
                        foreach (var field in fieldList)
                        {
                            var value = field.GetValue(config.Value);
                            if (value == null)
                            {
                                config.Element.Append("idref", "0");
                            }
                            else
                            {
                                // TODO: 
                            }
                        }
                    };
                });
        }

        public static XDocument Serialize(object value)
        {
            var document = new XDocument();
            if (value != null)
            {
                // type map
                var typeMap = CreateTypeMap();
                /*
                CachedExtension.Cached<Type, Action<XElement, object>>(
                    (Type type) =>
                    {
                        if (type == typeof (string))
                        {
                            return (element, v) => element.Append("value", v.ToString());
                        }
                        else if (type == typeof (float))
                        {
                            return (element, v) => element.Append("value", ((float)v).ToString("R"));
                        }
                        else if (type == typeof (double))
                        {
                            return (element, v) => element.Append("value", ((double)v).ToString("R"));
                        }
                        return (XElement element, object v) => { };
                    });
                 * */
                // idMap.
                var count = 0UL;
                var queue = new Queue<KeyValuePair<ulong, object>>();
                var idMap = CachedExtension.Cached(
                    (object key) =>
                    {
                        ++count;
                        queue.Enqueue(
                            new KeyValuePair<ulong, object>(count, key));
                        return count;
                    });
                // add value to the id map.
                idMap(value);
                var root = new XElement("root");
                document.Add(root);
                // serialize all objects.
                while (queue.Count > 0)
                {
                    var pair = queue.Dequeue();
                    root.Add(
                        new XElement("object").
                            Append("id", pair.Key.ToString()));
                    var v = pair.Value;
                    var type = v.GetType();
                    var serialize = typeMap(type);
                    serialize(new Config<object>(idMap, root, v));
                }
            }
            return document;
        }
    }
}
