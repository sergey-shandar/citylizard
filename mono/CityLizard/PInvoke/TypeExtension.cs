namespace CityLizard.PInvoke
{
    using S = System;
    using C = System.Collections.Generic;

    struct EnumValue
    {
        public string Name;        
        public object Value;
    }

    static class TypeExtension
    {
        public static C.IEnumerable<EnumValue> GetEnumItems(this S.Type type)
        {
            var valueType = type.GetField("value__").FieldType;
            var names = type.GetEnumNames();
            var values = type.GetEnumValues();
            for (int i = 0; i < names.Length; ++i)
            {
                yield return new EnumValue 
                { 
                    Name = names[i], 
                    Value = S.Convert.ChangeType(values.GetValue(i), valueType),
                };
            }
        }
    }
}
