using CodeWheel.Controls;
using CodeWheel.Dialogs;
using CodeWheel.Infrastructure;
using CodeWheel.Infrastructure.DB;
using CodeWheel.Utils;
using CodeWheel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace CodeWheel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel ViewModel
        {
            get;set;
        }

        public MainWindow()
        {
            InitializeComponent();
            
            this.ViewModel = new MainWindowViewModel();
            this.DataContext = this.ViewModel;
        }

        #region 模板参数界面
        /// <summary>
        /// 生成收集变量控件
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private UserControl CreateElement(VarInfoAttribute info)
        {
            UserControl control = null;
            switch (info.VarType)
            {
                case VarType.SingleString:
                    control = new CWSingleTextControl(info.VarDefault);
                    break;
                case VarType.MultiString:
                    control = new CWMutilTextControl(info.VarDefault);
                    break;
            }

            control.Name = info.VarName;
            this.RegisterName(control.Name, control);
            return control;
        }

        /// <summary>
        /// 清空元素
        /// </summary>
        private void ClearElement()
        {
            //清空原模板变量
            foreach (var item in this.varPanel.Children)
            {
                UserControl uc = item as UserControl;
                if (uc != null && !string.IsNullOrEmpty(uc.Name))
                {
                    this.UnregisterName(uc.Name);
                }
            }
            this.varPanel.Children.Clear();
            this.varPanel.RowDefinitions.Clear();
        }
        #endregion

        #region Events
        

        /// <summary>
        /// 选择代码保存目录 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoiceDir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != string.Empty)
            {
                this.ViewModel.SaveDir = fbd.SelectedPath;
            }


            //// 打开选择文件对话框,在WPF中， OpenFileDialog位于Microsoft.Win32名称空间
            //Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            //dialog.Filter = "文件|*.*";
            //if (dialog.ShowDialog() == true)
            //{
            //    string filePath = dialog.FileName;
            //}

        }

        /// <summary>
        /// 打开代码保存目录 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDir_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", this.ViewModel.SaveDir);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ViewModel.TemplateSelectIndex >= 0)
            {
                TemplateBase template = this.ViewModel.Templates[this.ViewModel.TemplateSelectIndex];
                if (template == null || template.Vars == null || template.Vars.Count <= 0)
                {
                    return;
                }
                ClearElement();

                VarInfoAttribute varInfo = null;
                string val = string.Empty;
                //添加新模板变量
                for (int i = 0; i < template.Vars.Count; i++)
                {
                    //读取以往的状态
                    varInfo = template.Vars[i];
                    val=ApplicationGlobal.Instance.States.GetValue($"{template.Name}.{varInfo.VarName}");
                    if (!string.IsNullOrEmpty(val))
                    {
                        varInfo.VarDefault = val;
                    }

                    RowDefinition def = new RowDefinition();
                    def.Height = new GridLength(30, GridUnitType.Auto);
                    this.varPanel.RowDefinitions.Add(def);

                    TextBlock title = new TextBlock();
                    title.Text = template.Vars[i].VarTitle;
                    Grid.SetColumn(title, 0);
                    Grid.SetRow(title, i);
                    title.VerticalAlignment = VerticalAlignment.Center;
                    this.varPanel.Children.Add(title);

                    UserControl element = CreateElement(varInfo);
                    Grid.SetColumn(element, 2);
                    Grid.SetRow(element, i);
                    this.varPanel.Children.Add(element);
                }
            }
        }

        /// <summary>
        /// 选择数据库连接字符串
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChoiceDB_Click(object sender, RoutedEventArgs e)
        {
            //打开对话框
            DBSelectDialog dialog = new DBSelectDialog(this.ViewModel.DBTypeSelectIndex);

            bool? result = dialog.ShowDialog();
            if (result != null && result.Value == true)
            {
                this.ViewModel.ConnectionString = dialog.ConnectionString;

            }
        }

        /// <summary>
        /// 连接字符串 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectDB_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ViewModel.TestConnectionion())
            {
                MessageBox.Show("选择的数据库连接不上，请重新填写");
                return;
            }

            DatabaseMeta dbMeta = null;
            switch (this.ViewModel.DBTypeSelectIndex)
            {
                case 0:
                    dbMeta = DatabaseMeta.CreateByMysql(this.ViewModel.ConnectionString);
                    break;
                case 1:
                    dbMeta = DatabaseMeta.CreateBySqlserver(this.ViewModel.ConnectionString);
                    break;
                case 2:
                    dbMeta = DatabaseMeta.CreateBySqlite(this.ViewModel.ConnectionString);
                    break;
            }

            foreach (var item in dbMeta.Tables)
            {
                this.ViewModel.Tables.Add(new TableSelectModel() { IsSelected = true, TableName = item.TableName,Meta=item });
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateCode_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.TemplateSelectIndex >= 0)
            {
                TemplateBase template = this.ViewModel.Templates[this.ViewModel.TemplateSelectIndex];
                if (template == null || template.Vars == null)
                {
                    return;
                }

                //必须保存文件
                if (template.MustChoiceSaveDir)
                {
                    if (string.IsNullOrEmpty(this.ViewModel.SaveDir))
                    {
                        MessageBox.Show("保存路径不能为空");
                        return;
                    }

                    if (!System.IO.Directory.Exists(this.ViewModel.SaveDir))
                    {
                        MessageBox.Show("保存路径不存在");
                        return;
                    }
                }

                //必须选择表
                if (template.MustChoiceTables)
                {
                    if (this.ViewModel.GetSelectedTables().Count <= 0)
                    {
                        MessageBox.Show("必须要选择数据表");
                        return;
                    }
                }

                UIVOBase vo = Activator.CreateInstance(template.ViewModelType) as UIVOBase;
                if (vo == null)
                {
                    MessageBox.Show("生成失败");
                    return;
                }

                string key = string.Empty;
                string val = string.Empty;
                //页面值
                for (int i = 0; i < template.Vars.Count; i++)
                {
                    CWControlBase uc = this.FindName(template.Vars[i].VarName) as CWControlBase;
                    if (uc == null)
                    {
                        template.Vars[i].VarData = null;
                        continue;
                    }
                    PropertyInfo property = template.ViewModelType.GetProperty(template.Vars[i].VarName);
                    if (property == null)
                    {
                        continue;
                    }
                    val = uc.GetValue()==null?string.Empty :uc.GetValue().ToString();
                    key = $"{template.Name}.{template.Vars[i].VarName}";
                    ApplicationGlobal.Instance.States.SetValue(key, val);

                    property.SetValue(vo, uc.GetValue(), null);
                }

                

                string msg = string.Empty;
                if (template.CreateFiles(ref msg, this.ViewModel.SaveDir, this.ViewModel.GetSelectedTables(), ApplicationGlobal.Instance.TemplateProvider.GenerateFile, vo))
                {
                    MessageBox.Show("生成成功");
                }
                else
                {
                    MessageBox.Show(msg);
                }
            }
        }

        /// <summary>
        /// 生成数据库文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateDBDocument_Click(object sender, RoutedEventArgs e)
        {
            if(WordHelper.ExportDBWordFile(System.IO.Path.Combine(this.ViewModel.SaveDir, "db_struct_"+DateTime.Now.ToString("yyyymmdd_HHmmss")+".docx"), this.ViewModel.GetSelectedTables()))
            {
                MessageBox.Show("导出成功");
            }
            else
            {
                MessageBox.Show("导出失败");
            }
        }
        #endregion
    }
}
