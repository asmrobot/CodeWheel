using CodeWheel.Model;
using CodeWheel.Model.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CodeWheel.Templates.NetCSharp
{
    /// <summary>
    /// .net数据通信类生成
    /// </summary>
    public class RealTemplate : ITemplate
    {
        public const string KEY = ".net通信类";
        public bool CreateFiles(out string msg,RunTemplateDelegate method, object parameters)
        {
            msg = string.Empty;
            DataEntity entity = parameters as DataEntity;
            
            if (string.IsNullOrWhiteSpace(entity.SavePath))
            {
                msg = "保存路径不能为空";
                return false;
            }

            if (string.IsNullOrEmpty(entity.JsonFile))
            {
                msg = "JSON文件不能为空";
                return false;
            }

            if (!File.Exists(entity.JsonFile))
            {
                msg = "文件不存在";
                return false;
            }

            string json = string.Empty;
            try
            {
                json = File.ReadAllText(entity.JsonFile);
            }
            catch
            {
                msg = "读取JSON文件失败";
                return false;
            }

            try
            {
                entity.ProtocolModel = ZTImage.Json.JsonParser.ToObject<ProtocolModel>(json);
                if (entity.ProtocolModel == null)
                {
                    msg = "解析出来的协议为空";
                    return false;
                }
            }
            catch(Exception e)
            {
                msg = "protocol解析失败"+e.Message;
                return false;
            }

            

            
            
            for (int i = 0; i < entity.ProtocolModel.commands.Count; i++)
            {
                entity.CurrentCommand = entity.ProtocolModel.commands[i];
                string file = Path.Combine(entity.SavePath, "Net_"+entity.CurrentCommand.name + ".cs");
                
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
