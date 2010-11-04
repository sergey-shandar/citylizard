namespace CityLizard.Xml
{
    using IO = System.IO;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public interface INode
    {
        void ToTextWriter(IO.TextWriter writer, string parentNamespace);
    }
}