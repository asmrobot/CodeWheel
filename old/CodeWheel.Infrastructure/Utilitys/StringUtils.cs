using System;

namespace CodeWheel.Infrastructure.Utilitys
{
    public static class StringUtils
    {
        private static string ToCamel(string val, bool firstUpper)
        {
            string name = string.Empty;
            bool flag = firstUpper;
            string[] sections = val.Split("_".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sections.Length; i++)
            {
                if (i == 0 && !firstUpper)
                {
                    name = sections[i];
                }
                else
                {
                    if (sections[i].Length <= 2)
                    {
                        name += sections[i].ToUpper();
                    }
                    else
                    {
                        name += sections[i][0].ToString().ToUpper() + sections[i].Substring(1);
                    }
                }
            }
            
            return name;
        }

        public static string ToLowerCamel(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }
            if (val.IndexOf('_') <= -1)
            {
                return val;
            }
            return ToCamel(val, false);
        }

        public static string ToUpperCamel(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }
            return ToCamel(val, true);
        }
    }
}
