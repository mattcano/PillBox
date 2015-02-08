using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Core.Helpers
{
    public class StringHelper
    {
        static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        static string BreakUpEnum(string enumValue)
        {
            return enumValue.Replace('_', ' ').ToLower();
        }

        public static string GetEnumString(string enumValue)
        {
            return UppercaseWords(BreakUpEnum(enumValue));
        }
    }
}
