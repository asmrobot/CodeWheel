using CodeWheel.Dialogs;
using CodeWheel.Model.DB;
using CodeWheel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeWheel.Controls
{
    /// <summary>
    /// CWTextControl.xaml 的交互逻辑
    /// </summary>
    public partial class CWDBControl : UserControl, CWControlBase
    {

        public CWDBControl(object _DefaultVal)
        {
            InitializeComponent();
            this.m_DefaultValue = _DefaultVal;
        }

        private object m_DefaultValue
        {
            get;
            set;
        }

        
        private DatabaseMeta DB
        {
            get;set;
        }


        public object GetValue()
        {
            return DB;
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rendercontrol_Click(object sender, RoutedEventArgs e)
        {
            //打开对话框
            DBSelectDialog dialog = new DBSelectDialog();

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                //获取信息
                DB = dialog.Database;
                if (DB != null)
                {
                    this.controlval.Text = DB.ConnectionString;
                }
            }
        }
    }
}
