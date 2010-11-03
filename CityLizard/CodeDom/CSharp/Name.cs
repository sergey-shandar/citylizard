namespace CityLizard.CodeDom.CSharp
{
    public static class Name
    {
        public static string Cast(string n)
        {
            var newName = n.Replace('.', '_').Replace('-', '_');
            if (char.IsDigit(newName[0]))
            {
                newName = "_" + newName;
            }
            return (Keyword.Set.Contains(newName) ? "@" : "") + newName;
        }
    }
}
