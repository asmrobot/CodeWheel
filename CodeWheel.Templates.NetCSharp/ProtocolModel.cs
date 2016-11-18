using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    /// <summary>
    /// JSON协议模型
    /// </summary>
    public class ProtocolModel
    {
        /// <summary>
        /// 包/命名空间名
        /// </summary>
        public string package { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string prefix { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string descript { get; set; }

        /// <summary>
        /// 命令集合
        /// </summary>
        public List<ProtocolCommandModel> commands { get; set; }
    }
}
