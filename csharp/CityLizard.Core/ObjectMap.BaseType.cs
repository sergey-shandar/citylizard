using System.IO;

namespace CityLizard.ObjectMap
{
    abstract class BaseType
    {
        public readonly TypeCategory Category;

        protected BaseType(TypeCategory category)
        {
            Category = category;
        }

        public abstract void Serialize(Stream serialize, object value);
    }
}
