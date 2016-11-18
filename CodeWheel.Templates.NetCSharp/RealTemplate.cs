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



            StringBuilder builder = new StringBuilder();
            builder.AppendLine("namespace "+entity.ProtocolModel.package);
            builder.AppendLine("{");
            builder.AppendLine("public enum CommandEnum : byte");
            builder.AppendLine("{");
           
            
            for (int i = 0; i < entity.ProtocolModel.commands.Count; i++)
            {
                entity.CurrentCommand = entity.ProtocolModel.commands[i];
                builder.Append(entity.CurrentCommand.name+"="+entity.CurrentCommand.command.ToString());

                if (i < entity.ProtocolModel.commands.Count - 1)
                {
                    builder.Append(",");
                }
                builder.Append("//"+entity.CurrentCommand.descript+"\r\n");

                string file = Path.Combine(entity.SavePath, entity.CurrentCommand.name + ".cs");                
                if (!method(file, KEY, typeof(DataEntity), entity))
                {
                    continue;
                }
            }
            builder.AppendLine("}");
            builder.AppendLine("}");

            string commandenumPath = Path.Combine(entity.SavePath, "CommandEnum.cs");
            File.WriteAllText(commandenumPath, builder.ToString(), System.Text.Encoding.UTF8);



            //{
            //    sendmessage = 1,
            //    sendwx = 2,
            //}

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
