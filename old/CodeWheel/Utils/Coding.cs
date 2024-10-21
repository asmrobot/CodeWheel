using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWheel.Utils
{
    public class Coding
    {
        /// <summary>
        /// 编码escape
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Escape(string code, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder();
            int len = code.Length;
            int i = 0;

            string temp = "*@-_+./";
            while (i < len)
            {
                byte[] b = System.Text.Encoding.BigEndianUnicode.GetBytes(code[i].ToString());
                if (b[0] == 0)
                {
                    if (temp.IndexOf(code[i]) > -1 || Char.IsLetterOrDigit(code[i]))
                    {
                        builder.Append(code[i]);
                    }
                    else
                    {
                        builder.Append("%" + b[1].ToString("X"));
                    }

                }
                else
                {
                    builder.Append("%u" + b[0].ToString("X2") + b[1].ToString("X2"));
                }

                i++;
            }
            return builder.ToString();
        }


        /// <summary>
        /// 解码escape
        /// </summary>
        /// <param name="code"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Unescape(string code, Encoding encoding)
        {
            if (string.IsNullOrEmpty(code))
            {
                return "";
            }
            int len = code.Length;
            int i = 0;
            StringBuilder builder = new StringBuilder();
            while (i < len)
            {
                if (code[i] == '%')
                {
                    if (code[i + 1] == 'u' || code[i + 1] == 'U')
                    {
                        //Unicode
                        code.Substring(i + 2, 4);
                        byte[] t = new byte[] { Convert.ToByte(code.Substring(i + 2, 2), 16), Convert.ToByte(code.Substring(i + 4, 2), 16) };
                        builder.Append(System.Text.UnicodeEncoding.BigEndianUnicode.GetString(t));
                        i += 6;
                    }
                    else if (code[i + 1] == '0')
                    {
                        byte t = Convert.ToByte(code[i + 2].ToString(), 16);
                        builder.Append(System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { t }));
                        i += 3;
                    }
                    else if (code[i + 1] == 'D' || code[i + 1] == 'd'|| code[i + 1] == 'A' || code[i + 1] == 'a')
                    {
                        byte t=Convert.ToByte(code[i + 1].ToString(),16);
                        builder.Append(System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { t }));
                        i += 2;
                    }
                    else
                    {
                        //普通字符
                        builder.Append(Uri.HexUnescape(code, ref i));
                    }
                }
                else
                {
                    builder.Append(code[i]);
                    i++;
                }
            }
            return builder.ToString();
        }

    }
}
