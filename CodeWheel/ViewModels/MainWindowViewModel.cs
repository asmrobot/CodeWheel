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
                var tinfo = Provider.Templates[i].GetTemplateInfo();
                if (tinfo != null)
                {
                    tinfo.Vars = Provider.GetVarInfo(tinfo.ViewModelType);
                    this.m_TemplateList.Add(tinfo);
                }
            }
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
