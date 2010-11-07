namespace CityLizard.Xml
{
    using C = System.Collections.Generic;

    public interface IElementBase: IName
    {
        /// <summary>
        /// {attribute}
        /// </summary>
        C.IEnumerable<IAttribute> AttributeList { get; }

        /// <summary>
        /// {element|text|comment}
        /// </summary>
        C.IEnumerable<INode> ContentList { get; }
    }
}
