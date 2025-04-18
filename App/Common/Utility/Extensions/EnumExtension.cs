using System.ComponentModel;
using System.Reflection;

namespace App.Common.Utility.Extensions
{
    public static class EnumExtension
    {
        public static string Description(this Enum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;

            return enumValue.ToString();
        }


        public static T ToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str, true);
        }

        public static bool IsEnum<T>(this string str) where T : struct
        {
            return Enum.TryParse<T>(str, true, out _);
        }

        public static bool IsEnum<T>(this int str) where T : struct
        {
            return Enum.TryParse<T>(str.ToString(), true, out _);
        }

        public static bool IsEnum<T>(this long str) where T : struct
        {
            return Enum.TryParse<T>(str.ToString(), true, out _);
        }

        public static bool IsEnum<T>(this byte str) where T : struct
        {
            return Enum.TryParse<T>(str.ToString(), true, out _);
        }

        public static T ToEnum<T>(this int val)
        {
            return (T)Enum.ToObject(typeof(T) , val);
        }

        public static T ToEnum<T>(this long val)
        {
            return (T)Enum.ToObject(typeof(T), val);
        }

        public static T ToEnum<T>(this byte val)
        {
            return (T)Enum.ToObject(typeof(T) , val);
        }

        public static byte EnumValue<T>(this T type)
        {
            return Convert.ToByte(Enum.Parse(typeof(T), type.ToString(), true));
        }

        public static IEnumerable<T> GetValues<T>() {
            return Enum.GetValuesAsUnderlyingType(typeof(T)).Cast<T>();
        }
    }
}
