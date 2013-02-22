namespace CityLizard.Xml
{
    /// <summary>
    /// The XML character data is all text that is not markup.
    /// </summary>
    public interface ICharacterData
    {
        /// <summary>
        /// The value of the node.
        /// </summary>
        string Value { get; }
    }
}
