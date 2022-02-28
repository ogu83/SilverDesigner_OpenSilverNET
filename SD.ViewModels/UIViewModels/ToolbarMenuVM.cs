using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverDesigner
{
    public class ToolbarMenuVM : VMBase
    {
        public event EventHandler OnNewCommand;
        public event EventHandler OnOpenCommand;
        public event EventHandler OnSaveCommand;
        public event EventHandler OnAddPageCommand;
        public event EventHandler OnHtmlEditorCommand;
        public event EventHandler OnPageViewCommand;
        public event EventHandler OnDocumentViewCommand;
        public event EventHandler OnResourcesViewCommand;

        public void NewCommand()
        {
            if (OnNewCommand != null)
                OnNewCommand(this, null);
        }
        public void OpenCommand(Tuple<string, string> file)
        {
            if (OnOpenCommand != null)
                OnOpenCommand(file, null);
        }
        public void SaveCommand(string fileName)
        {
            if (OnSaveCommand != null)
                OnSaveCommand(fileName, null);
        }

        public void AddPageCommand()
        {
            if (OnAddPageCommand != null)
                OnAddPageCommand(this, null);
        }

        public void ShowHTMLEditorCommand()
        {
            if (OnHtmlEditorCommand != null)
                OnHtmlEditorCommand(this, null);
        }

        public void PageViewCommand()
        {
            if (OnPageViewCommand != null)
                OnPageViewCommand(this, null);
        }

        public void DocumentViewCommand()
        {
            if (OnDocumentViewCommand != null)
                OnDocumentViewCommand(this, null);
        }

        public void ResourcesViewCommand()
        {
            if (OnResourcesViewCommand != null)
                OnResourcesViewCommand(this, null);
        }
    }
}
