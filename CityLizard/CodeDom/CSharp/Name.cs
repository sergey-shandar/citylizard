namespace CityLizard.CodeDom.CSharp
{
    /// <summary>
    /// C# identifier.
    /// </summary>
    public static class Name
    {
        /// <summary>
        /// Convert string to valid C# name.
        /// </summary>
        /// <param name="n">Text.</param>
        /// <returns>Valid C# name.</returns>
        public static string Cast(string n)
        {
            var newName = n.Replace('.', '_').Replace('-', '_');
            if (char.IsDigit(newName[0]))
            {
                newName = "_" + newName;
            }
            return 
                (Keyword.Set.Contains(newName) ? "@" : string.Empty) + newName;
        }
    }
}
