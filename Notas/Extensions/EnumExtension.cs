using Notas.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Notas.Extensions
{
    public static class EnumExtension
    {
        public static List<string> EnumColors()
        {
            var colors = from EnumColor n in Enum.GetValues(typeof(EnumColor))
                         select new { ID = (int)n, Name = GetEnumDescription(n) };

            return colors.Select(x => x.Name).ToList();
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
                return attributes.First().Description;

            return value.ToString();
        }
    }
}
