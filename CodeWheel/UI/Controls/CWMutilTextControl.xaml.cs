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
    public partial class CWMutilTextControl : UserControl,CWControlBase
    {

        public CWMutilTextControl(object _DefaultVal)
        {
            InitializeComponent();
            this.m_DefaultValue = _DefaultVal;
            if (this.m_DefaultValue != null)
            {
                this.rendercontrol.Text = this.m_DefaultValue.ToString();
            }
        }

        private object m_DefaultValue
        {
            get;
            set;

        }

        public object GetValue()
        {
            return this.rendercontrol.Text;
        }
    }
}
