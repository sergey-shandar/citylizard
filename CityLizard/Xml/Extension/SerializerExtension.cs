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
    using I = Internal;

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

        private sealed class SerializerInstance
        {
            public readonly int Id;

            public readonly I.Serialization.Instance I;

            public SerializerInstance(int id, I.Serialization.Instance i)
            {
                this.Id = id;
                this.I = i;
            }
        }

        private sealed class SerializerClass:
            CC.Cache<object, SerializerInstance>
        {
            private readonly I.Serialization.Class C;

            public readonly int Id;

            private readonly S.Action<I.Serialization.Instance, object> Set;

            public SerializerClass(Serializer s, S.Type type)
            {
                this.C = new I.Serialization.Class()
                {
                    Name = type.AssemblyQualifiedName,
                };
                this.Id = s.S.Classes.AddElement(this.C);

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

            protected override SerializerInstance Create(object key)
            {
                var i = new I.Serialization.Instance();
                return
                    new SerializerInstance(this.C.Instances.AddElement(i), i);
            }

            protected override void Initialize(object key, SerializerInstance data)
            {
                this.Set(data.I, key);
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
                            Instance = class_[o].Id,
                        };
                    }
                }
                return o;
            }

            public I.Serialization Serialize(object object_)
            {
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

        /*
        private class Serializer : C.Untyped
        {
            private T.X Types;

            private static T.A Id(string value)
            {
                return A("id", value);
            }

            private class Type
            {
                public readonly int Id;
                public T.X Element;

                public ObjectMap ObjectMap;

                public Type(int id)
                {
                    this.Id = id;
                }
            }

            private sealed class ObjectMap : CC.IdCache<object, int>
            {
                protected override int Create(object key, int id)
                {
                    return id;
                }

                protected override void Initialize(object key, int data)
                {
                    var element = this.X.X("object", Id(data.ToString()));
                    this.Type.Element = this.Type.Element[element];
                    this.Set(element, key);
                }

                private readonly S.Action<T.X, object> Set;

                private readonly Serializer X;

                private readonly TypeMap TypeMap;

                private readonly Type Type;

                public ObjectMap(
                    TypeMap typeMap, Type type, S.Action<T.X, object> set)
                {
                    this.TypeMap = typeMap;
                    this.X = typeMap.X;
                    this.Type = type;
                    this.Set = set;
                }
            }

            private T.X FieldList(T.X x, object object_)
            {
                foreach (var field in object_.GetType().GetFields())
                {
                    x = x
                        [this.X("field", Id(field.Name))
                            [this.AddObject(field.GetValue(object_))]
                        ];
                }
                return x;
            }

            private sealed class TypeMap : CC.IdCache<S.Type, Type>
            {
                public readonly Serializer X;

                public TypeMap(Serializer x)
                {
                    this.X = x;
                }

                protected override Type Create(S.Type key, int id)
                {
                    return new Type(id);
                }

                protected override void Initialize(S.Type key, Type data)
                {
                    S.Action<T.X, object> set;
                    if (IsList(key))
                    {
                        set = (e, o) =>
                        {
                            var i = 0;
                            foreach (var item in (SC.IEnumerable)o)
                            {
                                e = e
                                    [this.X.X("item", Id(i.ToString()))
                                        [this.X.AddObject(item)]
                                    ];
                                ++i;
                            }
                        };
                    }
                    else
                    {
                        set = (e, o) => this.X.FieldList(e, o);
                    }
                    data.ObjectMap = new ObjectMap(this, data, set);
                    var element =
                        this.X.X("type",
                            Id(data.Id.ToString()),
                            A("name", key.AssemblyQualifiedName));
                    this.X.Types = this.X.Types[element];
                    data.Element = element;
                }
            }

            private readonly TypeMap TypeMapInstance;

            private T.Element AddObject(object object_)
            {
                if (object_ == null)
                {
                    return E("null");
                }
                else
                {
                    var sType = object_.GetType();
                    if (IsSimple(sType))
                    {
                        return X("value")[object_.ToString()];
                    }
                    else if (sType.IsValueType)
                    {
                        return this.FieldList(X("struct"), sType.GetFields());
                    }
                    else
                    {
                        var type = this.TypeMapInstance[sType];
                        var refType = type.Id;
                        var refObject = type.ObjectMap[object_];
                        return
                            E("ref",
                                A("type", refType.ToString()),
                                A("object", refObject.ToString()));
                    }
                }
            }

            public C.Untyped.T.X Serialize(object object_)
            {
                var typeName = object_.GetType().AssemblyQualifiedName;
                return
                    X("root")
                        [X("object", A("type", typeName))
                            [this.AddObject(object_)]
                        ]
                        [this.Types];
            }

            public Serializer()
            {
                this.TypeMapInstance = new TypeMap(this);
                this.Types = X("types");
            }
        }

        public static void Serialize(
            this X.XmlWriter this_, object object_)
        {
            new Serializer().Serialize(object_).Save(this_);
        }
         * */

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
    }
}
