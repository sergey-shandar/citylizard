namespace CityLizard.Xml
{
    using T = System.Text;
    using S = System;
    using C = System.Collections.Generic;

    using System.Linq;

    /// <summary>
    /// Provides text manipulation methods that are used by several classes.
    /// </summary>
    public abstract class CharacterData : Node, ICharacterData
    {
        /// <summary>
        /// Initializes the text.
        /// </summary>
        /// <param name="value">The text.</param>
        protected CharacterData(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// The text.
        /// </summary>
        public string Value { get; private set; }
    }
}