using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWheel.Utils
{
    /// <summary>
    /// 界面状态提供者
    /// </summary>
    public class StateProvider
    {
        private const string FILE_NAME = "state.bak";
        private readonly string filePath = string.Empty;
        //配置
        private Dictionary<string, string> dict = new Dictionary<string, string>();
        public StateProvider()
        {
            filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FILE_NAME);
        }

        /// <summary>
        /// 得到某个配置项的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置某个配置项的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string key, string value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
            SaveFile();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            if (!File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite);
                fs.Close();
                return;
            }

            using (StreamReader reader = new StreamReader(this.filePath))
            {
                string content=reader.ReadToEnd();
                FormatDictionary(content);
            }
        }

        /// <summary>
        /// 格式化字典
        /// </summary>
        /// <param name="content"></param>
        private void FormatDictionary(string content)
        {
            string[] keyvalue = null;
            string key = string.Empty;
            string value = string.Empty;
            string[] lines = content.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                keyvalue = line.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (keyvalue.Length != 2)
                {
                    continue;
                }
                key = keyvalue[0].Trim();
                value = Coding.Unescape(keyvalue[1].Trim(),System.Text.UTF8Encoding.UTF8);
                dict.Add(key, value);
            }
        }

        

        /// <summary>
        /// 保存至文件
        /// </summary>
        private void SaveFile()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in this.dict)
            {
                builder.Append(item.Key);
                builder.Append("=");
                builder.Append(Coding.Escape(item.Value,System.Text.UTF8Encoding.UTF8));
                builder.Append("\r\n");
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Truncate, FileAccess.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(builder.ToString());
                }
                
            }
        }
    }
}
