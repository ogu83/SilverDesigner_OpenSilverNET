using SD.UIElements;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace SilverDesigner
{
    public class IDEVM : VMBase
    {
        public IDEVM()
        {
            ToolBoxMenu = new ToolBoxMenuVM();
            ToolBoxMenu.OnToolSelected += ToolBoxMenu_OnToolSelected;

            ToolbarMenu = new ToolbarMenuVM();
            ToolbarMenu.OnNewCommand += ToolbarMenu_OnNewCommand;
            ToolbarMenu.OnSaveCommand += ToolbarMenu_OnSaveCommand;
            ToolbarMenu.OnOpenCommand += ToolbarMenu_OnOpenCommand;
            ToolbarMenu.OnAddPageCommand += ToolbarMenu_OnAddPageCommand;
            ToolbarMenu.OnHtmlEditorCommand += ToolbarMenu_OnHtmlEditorCommand;
            ToolbarMenu.OnDocumentViewCommand += ToolbarMenu_OnDocumentViewCommand;
            ToolbarMenu.OnPageViewCommand += ToolbarMenu_OnPageViewCommand;
            ToolbarMenu.OnResourcesViewCommand += ToolbarMenu_OnResourcesViewCommand;

            EditArea = new EditAreaVM();

            CreateDocument();
        }

        public delegate void ShowNewDocumentDialogEventHandler(IDEVM sender, DOM.DocumentVM document);
        public event ShowNewDocumentDialogEventHandler ShowNewDocumentDialog;

        public event EventHandler ShowResourcesDialog;

        private void ToolbarMenu_OnResourcesViewCommand(object sender, EventArgs e)
        {
            if (ShowResourcesDialog != null)
                ShowResourcesDialog(this, null);
        }

        private void ToolbarMenu_OnPageViewCommand(object sender, EventArgs e)
        {
            EditArea.ShowView(EditAreaVM.ViewEnum.Page);
        }
        private void ToolbarMenu_OnDocumentViewCommand(object sender, EventArgs e)
        {
            EditArea.ShowView(EditAreaVM.ViewEnum.Document);
        }

        private void ToolbarMenu_OnHtmlEditorCommand(object sender, EventArgs e)
        {
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri("http://" + hostName + "/HTMLEditor.html"), "_blank", "toolbar=no,location=no,status=no,menubar=no,resizable=yes");
        }
        private void ToolbarMenu_OnOpenCommand(object sender, EventArgs e)
        {
            try
            {
                var file = sender as Tuple<string, string>;
                string mydata = file.Item2;
                var myDocument = DOM.DocumentVM.DeserializeFromJsonString(mydata);
                var myDocumentClone = myDocument.Clone() as DOM.DocumentVM;
                EditArea.Document = myDocumentClone;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "File Open Failed", MessageBoxButton.OK);
            }
        }
        private void ToolbarMenu_OnSaveCommand(object sender, EventArgs e)
        {
            string mydata = EditArea.Document.ToJsonString();
            //File.WriteAllText(sender as string, mydata);
            FileSaver.SaveTextToFile(mydata, sender as string).GetAwaiter().GetResult();
            MessageBox.Show("Success", "File Save", MessageBoxButton.OK);
        }
        private void ToolbarMenu_OnAddPageCommand(object sender, EventArgs e)
        {
            EditArea.Document.AddNewPage();
        }
        private void ToolbarMenu_OnNewCommand(object sender, EventArgs e)
        {
            var myDocument = new DOM.DocumentVM();
            if (ShowNewDocumentDialog != null)
                ShowNewDocumentDialog(this, myDocument);
        }

        public void CreateDocument(DOM.DocumentVM doc = null)
        {
            if (doc == null) doc = new DOM.DocumentVM();

            EditArea.Document = doc;
            EditArea.Document.InitFirstPage();
        }
        private void ToolBoxMenu_OnToolSelected(ToolBoxMenuVM sender, ToolBoxMenuVM.ToolEnum selectedTool)
        {
            EditArea.SetTool(selectedTool);
        }

        #region Properties
        private EditAreaVM _editArea;
        public EditAreaVM EditArea
        {
            get { return _editArea; }
            set
            {
                _editArea = value;
                NotifyPropertyChanged("EditArea");
            }
        }

        private ToolBoxMenuVM _toolBoxMenu;
        public ToolBoxMenuVM ToolBoxMenu
        {
            get { return _toolBoxMenu; }
            set
            {
                _toolBoxMenu = value;
                NotifyPropertyChanged("ToolBoxMenu");
            }
        }

        private ToolbarMenuVM _toolbarMenuVM;
        public ToolbarMenuVM ToolbarMenu
        {
            get { return _toolbarMenuVM; }
            set
            {
                _toolbarMenuVM = value;
                NotifyPropertyChanged("ToolbarMenu");
            }
        }
        #endregion
    }
}
