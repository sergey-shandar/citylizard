namespace CityLizard.PInvoke
{
    using T = System.Text;
    using C = System.Collections.Generic;

    static class StringBuilderExtensions
    {
        public static void AppendConcat(
            this T.StringBuilder builder, C.IEnumerable<string> strings)
        {
            foreach (var v in strings)
            {
                builder.Append(v);
            }
        }

        public static void AppendLineConcat(
            this T.StringBuilder builder, C.IEnumerable<string> strings)
        {
            foreach (var v in strings)
            {
                builder.AppendLine(v);
            }
        }
    }
}
