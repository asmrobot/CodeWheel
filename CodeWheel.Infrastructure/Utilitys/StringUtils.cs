using System;

namespace CodeWheel.Infrastructure.Utilitys
{
    public static class StringUtils
    {
        private static string ToCamel(string val, bool firstUpper)
        {
            string str = string.Empty;
            bool flag = firstUpper;
            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] == '_')
                {
                    flag = true;
                }
                else if (flag)
                {
                    char ch = val[i];
                    str = str + ch.ToString().ToUpper();
                    flag = false;
                }
                else
                {
                    str = str + val[i].ToString();
                }
            }
            return str;
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
