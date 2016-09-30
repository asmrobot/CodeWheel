using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    /// <summary>
    /// 数据库实体类生成
    /// </summary>
    public class RealTemplate : ITemplate
    {
        public const string KEY = "网络传输->C#";
        public bool CreateFiles(out string msg,RunTemplateDelegate method, Dictionary<string, object> parameters)
        {
            msg = string.Empty;
            DataEntity entity = new DataEntity();
            if (!parameters.ContainsKey("savepath"))
            {
                msg = "请设置保存路径";
                return false;
            }
            if (parameters["savepath"] == null)
            {
                msg = "保存路径不能为空";
                return false;
            }
            entity.SavePath = parameters["savepath"].ToString();
            if (!parameters.ContainsKey("namespace"))
            {
                msg = "默名空间不能为空";
                return false;
            }

            entity.NameSpace = parameters["namespace"].ToString();

            if (parameters.ContainsKey("importnamespace"))
            {
                entity.ImportNameSpace = parameters["importnamespace"].ToString();
            }

            if (!parameters.ContainsKey("database"))
            {
                msg = "请选择数据库";
                return false;
            }

            entity.Database = parameters["database"] as DatabaseMeta;
            if (entity.Database == null)
            {
                msg = "选择的数据库不存在";
                return false;
            }

            if (parameters.ContainsKey("classpre"))
            {
                entity.ClassPre = parameters["classpre"].ToString();
            }

            for (int i = 0; i < entity.Database.Tables.Count; i++)
            {
                entity.CurrentTable = entity.Database.Tables[i];
                string file = Path.Combine(entity.SavePath, entity.ClassPre+entity.CurrentTable.TableName + ".cs");
                
                if (!method(file, KEY, typeof(DataEntity), entity))
                {
                    continue;
                }
            }

            return true;
        }

        public TemplateInfo GetBaseInfo()
        {
            TemplateInfo info = new TemplateInfo();
            info.Name = KEY;
            info.ModelType = typeof(DataEntity);
            info.TemplateFile = "NetCSharp\\TemplateFile.cshtml";
            info.Vars = new List<VarInfo>();
            info.Vars.Add( new VarInfo("保存路径", "savepath", "d:\\", VarType.V_Path));
            info.Vars.Add(new VarInfo("命名空间", "namespace", "Models", VarType.V_String));
            info.Vars.Add(new VarInfo("导入命名空间", "importnamespace", "using System;", VarType.V_String));
            info.Vars.Add(new VarInfo("数据库", "database", "", VarType.V_DB));
            info.Vars.Add(new VarInfo("类型前缀", "classpre", "T_", VarType.V_String));
            
            return info;
        }
    }
}
