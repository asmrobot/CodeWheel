using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    /// <summary>
    /// 协议中命令字段
    /// </summary>
    public class ProtocolCommandFieldModel
    {
        public string name { get; set; }

        public string type { get; set; }

        public string descript { get; set; }

        public string GetTypeString()
        {
            switch (this.type.ToUpper())
            {
                case "BYTE":
                    return "byte";
                case "INT":
                    return "Int32";
                case "STRING":
                    return "String";
                case "BYTES":
                    return "byte[]";
                default:
                    return "unknow";
            }
        }

        /// <summary>
        /// 得到reader操作方法
        /// </summary>
        /// <returns></returns>
        public string GetReaderOperationString()
        {
            switch (this.type.ToUpper())
            {
                case "BYTE":
                    return "ReadByte";
                case "INT":
                    return "ReadInt";
                case "STRING":
                    return "ReadString";
                case "BYTES":
                    return "ReadBytes";
                default:
                    return "unknow";
            }
        }

        /// <summary>
        /// 得到writer操作方法
        /// </summary>
        /// <returns></returns>
        public string GetWriterOperationString()
        {
            switch (this.type.ToUpper())
            {
                case "BYTE":
                    return "WriteByte";
                case "INT":
                    return "WriteInt";
                case "STRING":
                    return "WriteString";
                case "BYTES":
                    return "WriteBytes";
                default:
                    return "unknow";
            }
        }
    }
}
