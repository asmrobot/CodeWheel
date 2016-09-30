using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeWheel.ViewModels
{
    public class DelegateCommand:ICommand
    {
        public DelegateCommand()
        {

        }

        public DelegateCommand(Action<object> execute)
        {
            this.ExcuteAction = execute;

        }
        public bool CanExecute(object parameter)
        {
            if (this.CanExcuteFunc == null)
            {
                return true;
            }

            return this.CanExcuteFunc(parameter); 
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (ExcuteAction != null)
            {
                this.ExcuteAction(parameter);
            }
        }


        public Action<object> ExcuteAction { get; set; }


        public Func<object,bool> CanExcuteFunc { get; set; }
    }
}
