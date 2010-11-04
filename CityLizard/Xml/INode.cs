namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public interface INode
    {
        void ToTextWriter(T.StringBuilder builder, string parentNamespace);
    }
}