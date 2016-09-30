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
    public partial class CWFileControl : UserControl, CWControlBase
    {

        public CWFileControl(object _DefaultVal)
        {
            InitializeComponent();
            this.m_DefaultValue = _DefaultVal;
            if (this.m_DefaultValue != null )
            {
                this.controlval.Text = this.m_DefaultValue.ToString();
            }
        }

        private object m_DefaultValue
        {
            get;
            set;
        }

        

        public object GetValue()
        {
            return this.controlval.Text;
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rendercontrol_Click(object sender, RoutedEventArgs e)
        {
            //打开对话框
            // 在WPF中， OpenFileDialog位于Microsoft.Win32名称空间
            Microsoft.Win32.OpenFileDialog dialog =new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "文件|*.*";
            if (dialog.ShowDialog() == true)
            {
                this.controlval.Text = dialog.FileName;
            }

        }
    }
}
