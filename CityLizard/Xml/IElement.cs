namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    public interface IElement : IName
    {
        C.IEnumerable<INode> NodeList { get; }
    }
}