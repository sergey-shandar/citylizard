using System.Linq;

namespace CityLizard.Xml.Extension
{
    using S = System;
    using X = System.Xml;
    using XS = System.Xml.Serialization;
    using C = CityLizard.Xml;
    using L = System.Xml.Linq;
    using SC = System.Collections;
    using G = System.Collections.Generic;
    using CC = CityLizard.Collections;
    using RS = System.Runtime.Serialization;
    using I = Internal;
    using D = System.Diagnostics;

    using U = CityLizard.Xml.Untyped;

    /// <summary>
    /// 1. Object-oriented serialization keeps top-level objects
    ///    (dynamic serializer).
    ///     Advantages: can save unknown structures.
    ///     Disadvantages: no XML schema.
    /// 2. Type defined structure (static serializer).
    ///     Advantages:
    ///         - can generate XML schema.
    ///         - can generate more readable XML structure.
    ///         - can generate faster serializer/deserializer.
    ///     Disadvantages:
    ///         - .NET does not have unions.
    /// Requirements:
    /// <list type="ordered">
    ///     <item>value types is keeped as values.</item>
    /// </list>
    /// Proposed dynamic structure:
    /// <code>
    /// [root]
    ///     [object type="..."]
    ///         [ref type="..." object="..."/]
    ///     [/object]
    ///     [type id="..." name="..."]
    ///         [object id="..."]
    ///             [item id="..."]
    ///                 [value]...[/value]
    ///             [/item]
    ///             ...
    ///         [/object]
    ///         ...
    ///     [/type]
    ///     [type id="..." name="..."]
    ///         [object id="..."]
    ///             [item id="..."]
    ///                 [ref type="..." object="..."/]
    ///             [/item]
    ///             [item id="..."]
    ///                 [null /]
    ///             [/item]
    ///             ...
    ///         [/object]
    ///         ...
    ///     [/type]
    ///     [type id="..." name="..."]
    ///         [object id="..."]
    ///             [field name="..."]
    ///                 [ref type="..." object="..."/]
    ///             [/field]
    ///             [field name="..."]
    ///                 [value]...[/value]
    ///             [/field]
    ///             [field name="..."]
    ///                 [struct]
    ///                     [field name="..."]
    ///                         [value]...[/value]
    ///                     [/field]
    ///                     ...
    ///                 [/struct]
    ///             [/field]
    ///             ...
    ///         [/object]
    ///         ...
    ///     [/type]
    ///     ...
    /// [/root]
    /// </code>
    /// </summary>
    public static class SerializerExtension
    {
        private static void SystemSerialize<T>(this X.XmlWriter writer, T value)
        {
            new XS.XmlSerializer(typeof(T)).Serialize(writer, value);
        }

        private static T SystemDeserialize<T>(this X.XmlReader reader)
        {
            return (T)(new XS.XmlSerializer(typeof(T)).Deserialize(reader));
        }

        private static bool IsList(S.Type sType)
        {
            return
                sType.IsGenericType &&
                sType.GetGenericTypeDefinition() == typeof(G.List<>);
        }

        private static readonly G.HashSet<S.Type> SimpleSet =
            new G.HashSet<S.Type>()
            {
                typeof(bool),
                typeof(byte),
                typeof(sbyte),
                typeof(char),
                typeof(decimal),
                typeof(double),
                typeof(float),
                typeof(int),
                typeof(uint),
                typeof(long),
                typeof(ulong),
                typeof(short),
                typeof(ushort),
                typeof(string),
                //
                typeof(S.DateTime),
            };

        private static bool IsSimple(S.Type sType)
        {
            return SimpleSet.Contains(sType);
        }

        private static string TypeName(this object object_)
        {
            return object_.GetType().AssemblyQualifiedName;
        }

        private static int AddElement<T>(this G.List<T> list, T value)
        {
            var i = list.Count;
            list.Add(value);
            return i;
        }

        private sealed class SerializerClass
        {
            private readonly G.List<I.Serialization.Instance> Instances;

            public readonly int Id;

            private readonly S.Action<I.Serialization.Instance, object> Set;

            private readonly RS.ObjectIDGenerator Generator =
                new RS.ObjectIDGenerator();

            public SerializerClass(Serializer s, S.Type type)
            {
                var class_ = new I.Serialization.Class()
                {
                    Name = type.AssemblyQualifiedName,
                    Instances = new G.List<I.Serialization.Instance>(),
                };
                this.Id = s.S.Classes.AddElement(class_);
                this.Instances = class_.Instances;

                if (IsList(type))
                {
                    this.Set = 
                        (i, o) =>
                            i.Elements =
                                ((SC.IEnumerable)o).
                                Cast<object>().
                                Select(e => s.AddObject(e)).
                                ToList();
                }
                else
                {
                    this.Set = (i, o) => i.Fields = s.GetFields(o);
                }
            }

            public int this[object key]
            {
                get
                {
                    bool firstTime;
                    var result = this.Generator.GetId(key, out firstTime);
                    if (firstTime)
                    {
                        var instance = new I.Serialization.Instance();
                        this.Instances.Add(instance);
                        D.Debug.Assert(result == this.Instances.Count);
                        //
                        this.Set(instance, key);
                    }
                    return (int)result - 1;
                }
            }
        }

        private sealed class Serializer: CC.Cache<S.Type, SerializerClass>
        {
            public readonly I.Serialization S = new I.Serialization();

            public G.List<I.Serialization.Field> GetFields(object object_)
            {
                return
                    object_.
                    GetType().
                    GetFields().
                    Select(
                        f => new I.Serialization.Field() 
                        { 
                            Name = f.Name,
                            Object = this.AddObject(f.GetValue(object_)),
                        }).
                    ToList();
            }

            public I.Serialization.Object AddObject(object object_)
            {
                var o = new I.Serialization.Object();
                if (object_ != null)
                {
                    var type = object_.GetType();
                    if (IsSimple(type))
                    {
                        o.Value = object_.ToString();
                    }
                    else if (type.IsValueType)
                    {
                        o.Fields = this.GetFields(object_);
                    }
                    else
                    {
                        var class_ = this[type];
                        o.Reference = new I.Serialization.Reference()
                        {
                            Class = class_.Id,
                            Instance = class_[object_],
                        };
                    }
                }
                return o;
            }

            public I.Serialization Serialize(object object_)
            {
                this.S.Classes = new G.List<I.Serialization.Class>();
                this.S.TypeName = object_.TypeName();
                this.S.Main = this.AddObject(object_);
                return this.S;
            }

            protected override SerializerClass Create(S.Type key)
            {
                return new SerializerClass(this, key);
            }
        }

        public static void Serialize(this X.XmlWriter writer, object object_)
        {
            writer.SystemSerialize(new Serializer().Serialize(object_));
        }

        private sealed class DeserializerClass: CC.ArrayCache<object>
        {
            private readonly S.Type Type;

            public DeserializerClass(I.Serialization.Class class_):
                base(class_.Instances.Count)
            {
                this.Type = S.Type.GetType(class_.Name);
            }

            protected override object Create(int i)
            {
                return S.Activator.CreateInstance(this.Type);
            }
        }

        private sealed class Deserializer: CC.ArrayCache<DeserializerClass>
        {
            private void SetFields(
                object o, G.List<I.Serialization.Field> fields)
            {
                foreach(var f in o.GetType().GetFields())
                {
                    var name = f.Name;
                    f.SetValue(
                        o,
                        this.GetObject(
                            f.FieldType,
                            fields.First(x => x.Name == name).Object));
                }
            }

            private object GetObject(
                string typeName, I.Serialization.Object object_)
            {
                return this.GetObject(S.Type.GetType(typeName), object_);
            }

            private object GetObject(
                S.Type type, I.Serialization.Object object_)
            {
                if (object_.Value != null)
                {
                    return S.Convert.ChangeType(object_.Value, type);
                }
                else if (object_.Fields != null)
                {
                    var o = S.Activator.CreateInstance(type);
                    this.SetFields(o, object_.Fields);
                    return o;
                }
                else if (object_.Reference != null)
                {
                    var r = object_.Reference;
                    return
                        this.
                        Serialization.
                        Classes[r.Class].
                        Instances[r.Instance];
                }
                else
                {
                    return null;
                }
            }

            private readonly I.Serialization Serialization;

            public Deserializer(I.Serialization s):
                base(s.Classes.Count)
            {
                this.Serialization = s;
            }

            public object Deserialize()
            {
                return
                    this.GetObject(
                        this.Serialization.TypeName, this.Serialization.Main);
            }

            protected override DeserializerClass Create(int i)
            {
                return new DeserializerClass(this.Serialization.Classes[i]);
            }
        }

        public static object Deserialize(this X.XmlReader reader)
        {
            return
                new Deserializer(reader.SystemDeserialize<I.Serialization>()).
                Deserialize();
        }

        /*
        private class Deserializer
        {
            private L.XElement Types;

            private class ObjectDictionary : CC.Cache<int, object>
            {
                private readonly Type Type;

                protected L.XElement Element(int key)
                {
                    var id = key.ToString();
                    return this.Type.Element.
                        Elements(U.GlobalName("object")).
                        First(e => e.Attribute(U.GlobalName("id")).Value == id);
                }

                public ObjectDictionary(Type type)
                {
                    this.Type = type;
                }

                protected override object Create(int key)
                {
                    return S.Activator.CreateInstance(this.Type.SType);
                }

                protected override void Initialize(int key, object data)
                {
                    var id = key.ToString();
                    var element = this.Type.Element.
                        Elements(U.GlobalName("object")).
                        First(e => e.Attribute(U.GlobalName("id")).Value == id);
                    this.Type.Action(data, element);
                }
            }

            private class Type
            {
                public readonly L.XElement Element;

                public readonly S.Type SType;

                public readonly ObjectDictionary ObjectMap;

                public readonly S.Action<object, L.XElement> Action;

                public Type(Deserializer d, L.XElement element)
                {
                    this.Element = element;
                    this.SType =
                        S.Type.GetType(
                            element.Attribute(U.GlobalName("name")).Value);
                    if (IsList(this.SType))
                    {
                        this.Action = (o, e) =>
                        {
                            var list = (SC.IList)o;
                            var itemType = this.SType.GetGenericArguments()[0];
                            foreach (var i in e.Elements(U.GlobalName("item")))
                            {
                                list.Add(d.GetObject(itemType, i));
                            }
                        };
                    }
                    else
                    {
                        this.Action = (o, e) => d.SetFields(o, e);
                    }
                    this.ObjectMap = new ObjectDictionary(this);
                }
            }

            private class TypeDictionary : CC.Cache<int, Type>
            {
                private readonly Deserializer D;

                protected override Type Create(int id)
                {
                    var idStr = id.ToString();
                    var element = this.D.Types.
                        Elements(U.GlobalName("type")).
                        First(
                            e =>
                                e.Attribute(U.GlobalName("id")).Value == idStr);
                    return new Type(this.D, element);
                }

                protected override void Initialize(int id, Type type)
                {
                }

                public TypeDictionary(Deserializer d)
                {
                    this.D = d;
                }
            }

            private TypeDictionary TypeMap;

            private void SetFields(object o, L.XElement e)
            {
                foreach (var field in o.GetType().GetFields())
                {
                    var fe =
                        e.
                        Elements(U.GlobalName("field")).
                        First(
                            x =>
                                x.Attribute(U.GlobalName("id")).Value ==
                                field.Name);
                    field.SetValue(
                        o, this.GetObject(field.FieldType, fe));
                }
            }

            private static int Attribute(L.XElement e, string name)
            {
                return int.Parse(e.Attribute(name).Value);
            }

            private object GetObject(S.Type sType, L.XElement e)
            {
                if (e.Element(U.GlobalName("null")) != null)
                {
                    return null;
                }
                else if (IsSimple(sType))
                {
                    return S.Convert.ChangeType(
                        e.Element(U.GlobalName("value")).Value, sType);
                }
                else if (sType.IsValueType)
                {
                    var o = S.Activator.CreateInstance(sType);
                    this.SetFields(o, e);
                    return o;
                }
                else
                {
                    var r = e.Element(U.GlobalName("ref"));
                    return
                        this.
                        TypeMap[Attribute(r, "type")].
                        ObjectMap[Attribute(r, "object")];
                }
            }

            public object Deserialize(L.XElement root)
            {
                this.Types = root.Element(U.GlobalName("types"));
                this.TypeMap = new TypeDictionary(this);
                var object_ = root.Element(U.GlobalName("object"));
                var type =
                    S.Type.GetType(
                        object_.Attribute(U.GlobalName("type")).Value);
                return this.GetObject(type, object_);
            }
        }

        public static object
            Deserialize(this X.XmlReader this_)
        {
            return new Deserializer().Deserialize(L.XElement.Load(this_));
        }
         * */
    }
}
