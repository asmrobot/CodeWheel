using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model
{
    public class TypeConverter
    {

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
