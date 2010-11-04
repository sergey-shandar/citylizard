namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// Provides text manipulation methods that are used by several classes.
    /// </summary>
    public interface ICharacterData : INode
    {
        /// <summary>
        /// The text.
        /// </summary>
        string Value { get; }
    }
}