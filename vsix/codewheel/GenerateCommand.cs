using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace codewheel
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("2f608a92-9f60-4219-b357-13158e34bbba");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }



        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new GenerateCommand(package, commandService);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void  Execute(object sender, EventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            
            string selectFile = await GetSelectedFile();
            if (selectFile.EndsWith(".zt"))
            {
                await Log($"是zt模板");
            }
            else
            {
                await Log($"非zt模板");
            }
            await  Log($"测试日志输出,{selectFile}");
            //string basePath=Path.GetDirectoryName(selectFile);
            //string baseFile=Path.GetFileNameWithoutExtension(selectFile);
            //for (int i = 0; i < 5; i++)
            //{
            //    using (var file = File.CreateText(Path.Combine(basePath, $"{baseFile}_{i}.cs")))
            //    {
            //        file.WriteLine($"{i}");
            //    }
            //}
            
            //// Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.package,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        /// <summary>
        /// 获取当前选中文件
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetSelectedFile()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var dte = (await ServiceProvider.GetServiceAsync(typeof(DTE))) as DTE;

            var selectedItems = dte.SelectedItems;
            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (SelectedItem item in selectedItems)
                {
                    if (item.ProjectItem != null)
                    {
                        return item.ProjectItem.FileNames[0];
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task Log(string message)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            
            // 获取输出窗口服务
            var outputWindow = await ServiceProvider.GetServiceAsync(typeof(SVsOutputWindow)) as IVsOutputWindow;

            // 创建一个新的输出窗格
            Guid outputPaneGuid = CommandSet;

            // 获取输出窗格
            outputWindow.GetPane(ref outputPaneGuid, out IVsOutputWindowPane outputPane);
            if (outputPane == null)
            {
                outputWindow.CreatePane(ref outputPaneGuid, "ZTImage", 1, 1);
                outputWindow.GetPane(ref outputPaneGuid, out outputPane);
            }

            //激活
            outputPane.Activate();
            
            // 写入内容
            outputPane.OutputString($"{DateTime.Now}: {message}\n");
        }



    }
}
