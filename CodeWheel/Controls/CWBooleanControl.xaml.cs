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
    public partial class CWBooleanControl : UserControl,CWControlBase
    {

        public CWBooleanControl(object _DefaultVal)
        {
            InitializeComponent();
            this.m_DefaultValue = _DefaultVal;
            if (this.m_DefaultValue != null&& this.m_DefaultValue.ToString().ToUpper()=="TRUE")
            {
                this.rendercontrol.IsChecked = true;
            }
        }

        private object m_DefaultValue
        {
            get;
            set;

        }

        public object GetValue()
        {
            if (this.rendercontrol.IsChecked.HasValue && this.rendercontrol.IsChecked.Value)
            {
                return true;
            }
            return false;
        }
    }
}
