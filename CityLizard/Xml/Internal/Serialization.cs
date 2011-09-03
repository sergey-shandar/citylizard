namespace CityLizard.Xml.Internal
{
    using S = System.Xml.Serialization;
    using G = System.Collections.Generic;

    public sealed class Serialization
    {
        /// <summary>
        /// A reference on a class instance.
        /// </summary>
        public sealed class Reference
        {
            /// <summary>
            /// A class id.
            /// </summary>
            [S.XmlAttribute()]
            public int Class;

            /// <summary>
            /// An instance id.
            /// </summary>
            [S.XmlAttribute()]
            public int Instance;
        }

        /// <summary>
        /// A type field.
        /// </summary>
        public sealed class Field
        {
            /// <summary>
            /// A name of the field.
            /// </summary>
            [S.XmlAttribute()]
            public string Name;

            /// <summary>
            /// An object.
            /// </summary>
            public Object Object;
        }

        /// <summary>
        /// A class instance.
        /// </summary>
        public sealed class Instance
        {
            /// <summary>
            /// Class fields.
            /// </summary>
            public G.List<Field> Fields;

            /// <summary>
            /// Elements of a list.
            /// </summary>
            public G.List<Object> Elements;
        }

        /// <summary>
        /// An object.
        /// </summary>
        public sealed class Object
        {
            /// <summary>
            /// Value.
            /// </summary>
            [S.XmlAttribute()]
            public string Value;

            /// <summary>
            /// Structure fields.
            /// </summary>
            public G.List<Field> Fields;

            /// <summary>
            /// Reference on a class instance.
            /// </summary>
            public Reference Reference;
        }

        /// <summary>
        /// A class.
        /// </summary>
        public sealed class Class
        {
            /// <summary>
            /// The type name.
            /// </summary>
            [S.XmlAttribute()]
            public string Name;

            /// <summary>
            /// A list of class instances.
            /// </summary>
            public G.List<Instance> Instances;
        }

        /// <summary>
        /// A type name of the main object.
        /// </summary>
        [S.XmlAttribute()]
        public string TypeName;

        /// <summary>
        /// The main object.
        /// </summary>
        public Object Main;

        /// <summary>
        /// A list of classes.
        /// </summary>
        public G.List<Class> Classes;
    }
}
