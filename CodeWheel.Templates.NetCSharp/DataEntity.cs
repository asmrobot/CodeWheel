using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    public class DataEntity
    {
        /// <summary>
        /// 读取的JSON文件
        /// </summary>
        [VarInfoAttribute("JSON文件", "D:\\Codegen", VarType.V_File)]
        public string JsonFile { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        [VarInfoAttribute("保存路径", "D:\\Codegen", VarType.V_Path)]
        public string SavePath { get; set; }

        
        /// <summary>
        /// 引用空间
        /// </summary>
        [VarInfoAttribute("导入命名空间", "using System;", VarType.V_String)]
        public string ImportNameSpace { get; set; }

        /// <summary>
        /// 当前命令
        /// </summary>
        public ProtocolCommandModel CurrentCommand { get; set; }


        /// <summary>
        /// 协义定义
        /// </summary>
        public ProtocolModel ProtocolModel { get; set; }


        


        
        
    }
}
