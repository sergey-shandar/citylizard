namespace CityLizard.Xml
{
    using C = System.Collections.Generic;

    public interface IElementBase: IName
    {
        C.IEnumerable<IAttribute> AttributeList { get; }
    }
}
