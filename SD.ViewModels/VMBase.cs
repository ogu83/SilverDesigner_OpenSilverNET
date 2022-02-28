using SilverDesigner.DOM;
using System.ComponentModel;
using System.Windows;

namespace SilverDesigner
{
    public abstract class VMBase : INotifyPropertyChanged
    {
        public delegate void SelectedChangeEventHandler(UIObjectVM sender, UIObjectVM selected);
        public delegate void ObjectChangedEventHandler(ContainerVM container, UIObjectVM obj, string propertyName);
        public delegate void SelectedEventHandler(UIObjectVM obj);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        protected string hostName
        {
            get
            {
                string ret = Application.Current.Host.Source.Host;
                if (Application.Current.Host.Source.Port != 80)
                    ret += ":" + Application.Current.Host.Source.Port;

                return ret;
            }
        }

        public virtual VMBase Clone()
        {
            return null;
        }
    }
}