namespace CityLizard.Xml
{
    /// <summary>
    /// Static functions.
    /// </summary>
    public class Static
    {
        /// <summary>
        /// Represents the content of an XML comment.
        /// </summary>
        /// <param name="text">The content.</param>
        /// <returns>The XML comment.</returns>
        public static Comment Comment(string text)
        {
            return new Comment(text);
        }
    }
}
