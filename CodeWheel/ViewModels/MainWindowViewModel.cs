using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using CodeWheel.Model;
using CodeWheel.Utils;

namespace CodeWheel.ViewModels
{
    public class MainWindowViewModel:NotificationObject
    {

        public MainWindowViewModel(RazorProvider Provider)
        {

            this.m_TemplateList = new ObservableCollection<TemplateInfo>();
            
            //加载模板
            Provider.LoadTemplate();

            for (int i = 0; i < Provider.Templates.Count; i++)
            {
                var tinfo=Provider.Templates[i].GetBaseInfo();
                if (tinfo != null)
                {
                    this.m_TemplateList.Add(tinfo);
                }
            }





            //this.m_TemplateList.Add(new TemplateInfo()
            //{
            //    TemplateName = "实体生成",
            //    VarList = new List<VarInfo>() {
            //        new VarInfo() { VarTitle="命名空间",VarName="modelnamespace",VarType=VarType.V_String,VarDefault="using System;"},
            //        new VarInfo() { VarTitle="行号",VarName="modelrowno",VarType=VarType.V_Int,VarDefault="3"},
            //        new VarInfo() { VarTitle="引入包",VarName="modelimport",VarType=VarType.V_String,VarDefault="using System;"},
            //        new VarInfo() { VarTitle="是否生成文件",VarName="modelgenricfile",VarType=VarType.V_Boolean,VarDefault="true"},
            //        new VarInfo() { VarTitle="生成日期",VarName="modelgenericdt",VarType=VarType.V_DateTime,VarDefault="2016-12-23"},
            //        new VarInfo() { VarTitle="生成文件",VarName="modelgenericgetfile",VarType=VarType.V_File,VarDefault="d:/ss.txt"},
            //        new VarInfo() { VarTitle="生成路径",VarName="modelgenericgetpath",VarType=VarType.V_Path,VarDefault="d:/ss/ww"},
            //        new VarInfo() { VarTitle="数据库",VarName="modelgetdb",VarType=VarType.V_DB,VarDefault="d:/ss/ww"},
            //    }
            //});


        }

        private ObservableCollection<TemplateInfo> m_TemplateList;

        /// <summary>
        /// 模板列表
        /// </summary>
        public ObservableCollection<TemplateInfo> TemplateList
        {
            get
            {
                return m_TemplateList;
            }
            set
            {
                if (m_TemplateList != value)
                {
                    m_TemplateList = value;
                    this.RaisePropertyChanged("TemplateList");
                }
            }
        }


        private int m_TemplateSelectIndex =-1;
        /// <summary>
        /// 选择模板索引
        /// </summary>
        public int TemplateSelectIndex
        {
            get
            {
                return m_TemplateSelectIndex ;
            }

            set
            {
                if (m_TemplateSelectIndex  != value)
                {
                    m_TemplateSelectIndex  = value;
                    this.RaisePropertyChanged("TemplateSelectIndex");
                }
            }

        }


        private string m_TemplateVarSettingPage = "Dialogs/EmptyPage.xaml";
        /// <summary>
        /// 模板变量设置界面
        /// </summary>
        public string TemplateVarSettingPage
        {
            get
            {
                return m_TemplateVarSettingPage;
            }

            set
            {
                if (m_TemplateVarSettingPage != value)
                {
                    m_TemplateVarSettingPage = value;
                    this.RaisePropertyChanged("TemplateVarSettingPage");
                }
            }
        }

        
        
    }
}
