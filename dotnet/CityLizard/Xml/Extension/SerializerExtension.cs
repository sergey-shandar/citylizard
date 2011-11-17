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
    /// Object-oriented serialization keeps top-level objects
    /// (dynamic serializer).
    ///     Advantages: can save unknown structures.
    ///     Disadvantages: no XML schema.
    /// </summary>
    public static class SerializerExtension
    {
        private static void SystemSerialize<T>(this X.XmlWriter writer, T value)
        {
            new XS.XmlSerializer(typeof(T)).Serialize(writer, value);
            //new RS.DataContractSerializer(typeof(T)).WriteObject(writer, value);
        }

        private static T SystemDeserialize<T>(this X.XmlReader reader)
        {
            return (T)(new XS.XmlSerializer(typeof(T)).Deserialize(reader));
            //return
            //    (T)(new RS.DataContractSerializer(typeof(T)).ReadObject(reader));
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

        private sealed class Serializer: CC.DictionaryCache<S.Type, SerializerClass>
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
                if (object_ == null)
                {
                    o.Null = new I.Serialization.Null();
                }
                else
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

            private readonly G.List<I.Serialization.Instance> Instances;

            public readonly S.Action<object, I.Serialization.Instance> Set;

            public DeserializerClass(
                Deserializer d, I.Serialization.Class class_):
                base(class_.Instances.Count)
            {
                this.Type = S.Type.GetType(class_.Name);
                this.Instances = class_.Instances;
                
                if (IsList(this.Type))
                {
                    this.Set = (o, i) =>
                    {
                        var list = (SC.IList)o;
                        var itemType = this.Type.GetGenericArguments()[0];
                        foreach (var element in i.Elements)
                        {
                            list.Add(d.GetObject(itemType, element));
                        }
                    };
                }
                else
                {
                    this.Set = (o, i) => d.SetFields(o, i.Fields);
                }
            }

            protected override object Create(int i)
            {
                return S.Activator.CreateInstance(this.Type);
            }

            protected override void Initialize(int i, object data)
            {
                this.Set(data, this.Instances[i]);
            }
        }

        private sealed class Deserializer: CC.ArrayCache<DeserializerClass>
        {
            public void SetFields(
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

            public object GetObject(
                S.Type type, I.Serialization.Object object_)
            {
                if (object_.Null != null)
                {
                    return null;
                }
                else if (object_.Value != null)
                {
                    return S.Convert.ChangeType(object_.Value, type);
                }
                else if (object_.Reference != null)
                {
                    var r = object_.Reference;
                    return this[r.Class][r.Instance];
                }
                else
                {
                    var o = S.Activator.CreateInstance(type);
                    this.SetFields(o, object_.Fields);
                    return o;
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
                return new DeserializerClass(this, this.Serialization.Classes[i]);
            }
        }

        public static object Deserialize(this X.XmlReader reader)
        {
            return
                new Deserializer(reader.SystemDeserialize<I.Serialization>()).
                Deserialize();
        }
    }
}
