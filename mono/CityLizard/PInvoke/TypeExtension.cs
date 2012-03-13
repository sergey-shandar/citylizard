﻿namespace CityLizard.PInvoke
{
    using S = System;
    using C = System.Collections.Generic;
    using R = System.Reflection;
    using I = System.Runtime.InteropServices;

    using System.Linq;

    struct EnumValue
    {
        public string Name;        
        public object Value;
    }

    static class TypeExtension
    {
        public static S.Type GetEnumValueType(this S.Type type)
        {
            return type.GetField("value__").FieldType;
        }

        public static C.IEnumerable<EnumValue> GetEnumItems(this S.Type type)
        {
            var valueType = type.GetEnumValueType();
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

        public static T GetCustomAttribute<T>(
            this R.ICustomAttributeProvider p, bool inherit)
            where T: S.Attribute
        {
            return 
                (T)p.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
        }

        public static bool IsPreserveSig(this R.MethodInfo method)
        {
            var f = method.GetMethodImplementationFlags();
            return (f & R.MethodImplAttributes.PreserveSig) != 0;
        }

        public static I.CallingConvention GetCallingConvention(
            this R.MethodInfo method)
        {
            var attribute = 
                method.GetCustomAttribute<I.DllImportAttribute>(true);
            return attribute != null ?
                attribute.CallingConvention :
                I.CallingConvention.StdCall;
        }
    }
}
