using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeWheel.Model
{
    /// <summary>
    /// 支持的变量类型枚举
    /// </summary>
    public enum VarType
    {
        V_Int,//int,
        V_String,//string,
        V_Boolean,//boolean,复选框
        V_DateTime,//datetime,选择日期
        V_DB,//db,选择数据库
        V_Path,//path,选择路径
        V_File//file,选择文件
    }

}
