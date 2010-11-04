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
        /// The text.
        /// </summary>
        public readonly string Value;

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
        string ICharacterData.Value
        {
            get { return this.Value; }
        }
    }
}