namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// Provides name manipulation methods that are used by several classes.
    /// </summary>
    public interface IName : INode
    {
        /// <summary>
        /// The XML namespace.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// The XML name.
        /// </summary>
        string Name { get; }
    }
}