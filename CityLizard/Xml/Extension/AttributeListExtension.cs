namespace CityLizard.Xml.Extension
{
    using C = System.Collections.Generic;

    /// <summary>
    /// Extension methods for XML attributes.
    /// </summary>
    public static class AttributeListExtension
    {
        /// <summary>
        /// Adds an attribute to the list.
        /// </summary>
        /// <param name="this_">The list of attributes.</param>
        /// <param name="name">The attribute name.</param>
        /// <param name="value">The attribute value.</param>
        public static void Add(
            this C.IList<IAttribute> this_, string name, string value)
        {
            this_.Add(new Attribute(null, name, value));
        }
    }
}
