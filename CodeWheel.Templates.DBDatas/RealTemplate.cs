using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeWheel.Templates.DBDatas
{
    /// <summary>
    /// 数据库实体类生成
    /// </summary>
    public class RealTemplate : ITemplate
    {
        public const string KEY = "数据库访问层";
        public bool CreateFiles(out string msg,RunTemplateDelegate method, object parameters)
        {
            msg = string.Empty;
            DataEntity entity = parameters as DataEntity;
            
            if (string.IsNullOrWhiteSpace(entity.SavePath))
            {
                msg = "保存路径不能为空";
                return false;
            }
            
            if (entity.Database == null)
            {
                msg = "选择的数据库不存在";
                return false;
            }
            
            for (int i = 0; i < entity.Database.Tables.Count; i++)
            {
                entity.CurrentTable = entity.Database.Tables[i];
                string file = Path.Combine(entity.SavePath, entity.CurrentTable.UpperCamelName+entity.ClassFix + ".cs");
                
                if (!method(file, KEY, typeof(DataEntity), entity))
                {
                    continue;
                }
            }

            return true;
        }

        public TemplateInfo GetTemplateInfo()
        {
            TemplateInfo info = new TemplateInfo();
            Assembly asm = System.Reflection.Assembly.GetAssembly(this.GetType());
            Stream stream = asm.GetManifestResourceStream(string.Concat(asm.GetName().Name, ".TemplateFile.cshtml"));
            StreamReader reader = new StreamReader(stream);
            info.TemplateContent = reader.ReadToEnd();
            stream.Close();
            info.Name = KEY;
            info.ViewModelType = typeof(DataEntity);
            return info;
        }
    }
}
