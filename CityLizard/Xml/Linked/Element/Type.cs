namespace CityLizard.Xml.Linked.Element
{
    /// <summary>
    /// The element type.
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// Simple type (simpleType).
        /// </summary>
        Simple,

        /// <summary>
        /// Complex type (complexType), empty.
        /// </summary>
        Empty,

        /// <summary>
        /// Complex type (complexType mixed="false"), not empty, not mixed.
        /// </summary>
        NotMixed,

        /// <summary>
        /// Complex type (complextType mixed="true"), not empty, mixed.
        /// </summary>
        Mixed,
    }
}
