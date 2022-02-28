using System;
using System.Windows.Input;

namespace SilverDesigner
{
    public class RelayCommand : ICommand
    {
        private readonly Action handler;
        private bool isEnabled;

        public RelayCommand(Action handler)
        {
            this.handler = handler;
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    if (CanExecuteChanged != null)
                        CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            handler();
        }
    }

}
