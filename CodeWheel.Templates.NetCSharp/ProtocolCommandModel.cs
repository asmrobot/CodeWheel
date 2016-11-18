using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    /// <summary>
    /// 协议中的命令
    /// </summary>
    public class ProtocolCommandModel
    {
        public string name { get; set; }

        public string descript { get; set; }

        public byte command { get; set; }


        public List<ProtocolCommandFieldModel> fields { get; set; }

        public string GetMethodParameterString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < fields.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(",");
                }
                builder.Append(fields[i].GetTypeString() + " " + fields[i].name.ToLower());
            }
            return builder.ToString();
        }

    }
}
