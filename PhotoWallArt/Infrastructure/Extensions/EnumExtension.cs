using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class EnumExtension
    {

        public static string GetDescription<TEnum>(this TEnum enumValue) 
            where TEnum : Enum
        {
            var type = typeof(TEnum);
            var name = Enum.GetName(type, enumValue)!;
            var field = type.GetField(name, BindingFlags.Static | BindingFlags.Public);
            if (field == null)
            {
                return string.Empty;
            }
            return field.GetCustomAttribute<DescriptionAttribute>()!
                    .Description ?? name;
        }
    }
}
