using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Infrastructure
{
    public class TypeConverter
    {
        public static Int32 StringToInt(string str, Int32 defVal)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defVal;
            }
            Int32 defV = defVal;
            if (Int32.TryParse(str, out defV))
            {
                return defV;
            }
            return defVal;
        }



        public static Int32 ObjectToInt(object obj, Int32 defVal)
        {
            if (obj == null)
            {
                return defVal;
            }
            Int32 defV = defVal;
            if (Int32.TryParse(obj.ToString(), out defV))
            {
                return defV;
            }
            return defVal;
        }


        public static Boolean ObjectToBool(object obj, bool defVal)
        {
            if (obj == null)
            {
                return defVal;
            }
            bool defV = defVal;
            if (Boolean.TryParse(obj.ToString(), out defV))
            {
                return defV;
            }
            return defVal;
        }
    }
}
