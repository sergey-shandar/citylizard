using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLizard.ObjectMap
{
    sealed class OutputObjectMap
    {
        struct Field
        {
            public readonly string Field;

            public readonly Func<TypeInfo> Type;
        }

        /// <summary>
        /// type:
        /// - struct (0)
        ///     - simple  (0)
        ///         - integer (0)
        ///             - signed   (0)
        ///                 - size 2^X
        ///             - unsigned (1)
        ///                 - size 2^X
        ///         - float   (1)
        ///             - signed   (0)
        ///                 - size 2^X
        ///     - complex (1)
        ///         - number of fields (0..64), ptr on array of fields
        /// - class  (1)
        ///     - array   (0)
        ///         - string  (0)
        ///             - (0 0000) length, UTF-8
        ///         - array   (1)
        ///             - (0 0000) typeRef: count, 
        ///     - complex (1)
        ///         - number of fields (0..63), ptr on array of fields
        /// </summary>
        [Flags]
        enum TypeType: byte
        {
            ByRef          = 0x80,
            //
            Complex        = 0x40,
            FieldListCount = 0x3F,
            //
            Float          = 0x20,
            Signed         = 0x10,
            SizePower      = 0x0F,
            //
            Array          = 0x20,
        }
        
        /// <summary>
        /// type:
        /// - struct (0)
        ///     - int (00)
        ///         - signed
        ///         - unsigned
        ///     - float (01)
        ///     - complex (10)
        ///     - reserved (11)
        /// - class (1)
        ///     - string (00)
        ///     - array (01)
        ///     - complex (10)
        ///     - reserved (11)
        /// 
        /// type 2:
        ///     IsByRef: boolean
        ///     IsArray: boolean
        /// </summary>
        class TypeInfo
        {
            public readonly String Name;

            public readonly bool IsByRef;

            public readonly bool IsArray;

            public readonly Func<TypeInfo> ElementType;

            public readonly int Size;

            public readonly Field[] FieldList;

            public TypeInfo(OutputObjectMap map, Type type)
            {
                Name = type.Name;
                IsByRef = type.IsByRef;
                IsArray = type.IsArray;
                ElementType = 
                    type.IsArray ?
                        map.GetLazyTypeInfo(type.GetElementType()) :
                        () => null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>a function to avoid infinitive recursion. The result of the 
        /// function should be serializable.</returns>
        private Func<TypeInfo> GetLazyTypeInfo(Type type)
        {
            // TODO:
            return null;
        }

        private readonly Dictionary<Type, ulong> TypeMap =
            new Dictionary<Type, ulong>();

        private readonly Dictionary<Object, ulong> ObjectMap =
            new Dictionary<object, ulong>();

        private readonly IOutputObjectMapStore Store;

        public OutputObjectMap(IOutputObjectMapStore store)
        {
            Store = store;
        }

        public ulong Write(Object value)
        {
            if (value == null)
            {
                return 0;
            }
            // search the object in a map
            {
                ulong id;
                if (ObjectMap.TryGetValue(value, out id))
                {
                    return id;
                }
            }
            // create an object store.
            var objectStore = Store.New();
            // TODO: serialize the object to the store.            
            return objectStore.Id;
        }
    }
}
