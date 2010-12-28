namespace CityLizard.Xml.Extension
{
    /// <summary>
    /// Extension methods for not empty elements.
    /// </summary>
    public static class NotEmptyExtension
    {
        /// <summary>
        /// Adds the comment text to the not empty element.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="this_">The element.</param>
        /// <param name="text">The comment text.</param>
        /// <returns>The element.</returns>
        public static T AddComment<T>(this T this_, string text)
            where T: Linked.Element.NotEmpty
        {
            ((Linked.Element.NotEmpty)this_).AddComment(text);
            return this_;
        }
    }
}
