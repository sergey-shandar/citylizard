namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// Represents an XML element.
    /// </summary>
    public interface IElement : IElementBase
    {
        /// <summary>
        /// Gets all childern nodes.
        /// </summary>
        C.IEnumerable<INode> NodeList { get; }
    }
}