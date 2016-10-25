using CodeWheel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model
{
    

    /// <summary>
    /// 变量信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class VarInfoAttribute:Attribute
    {
        public VarInfoAttribute()
        {

        }
        public VarInfoAttribute(string title,string def,VarType type)
        {
            this.VarTitle = title;
            this.VarDefault = def;
            this.VarType = type;
            this.VarData = null;

        }
        /// <summary>
        /// 变量标题
        /// </summary>
        public string VarTitle { get; set; }

        /// <summary>
        /// 变量名
        /// </summary>
        public string VarName { get; set; }
        
        /// <summary>
        /// 变量类型
        /// </summary>
        public VarType VarType { get; set; }

        /// <summary>
        /// 变量数据
        /// </summary>
        public object VarData { get; set; }

        /// <summary>
        /// 变量默认值
        /// </summary>
        public string VarDefault { get; set; }
    }
}
