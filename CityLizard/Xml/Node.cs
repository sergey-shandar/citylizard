namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;
    using Extension;

    public abstract class Node : INode
    {
        public abstract void ToStringBuilder(
            T.StringBuilder builder, string parentNamespace);

        public override string ToString()
        {
            var builder = new T.StringBuilder();
            builder.AppendNode(this, null);
            return builder.ToString();
        }
    }
}