using SD.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverDesigner
{
    public partial class ToolbarMenu : UserControl
    {
        private ToolbarMenuVM _myVM { get { return DataContext as ToolbarMenuVM; } }

        public ToolbarMenu()
        {
            InitializeComponent();
        }

        private void closeAllPopups()
        {
            fileMenu.IsOpen = false;
            editMenu.IsOpen = false;
            viewMenu.IsOpen = false;
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            fileMenu.IsOpen = !fileMenu.IsOpen;
            editMenu.IsOpen = false;
            viewMenu.IsOpen = false;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            editMenu.IsOpen = !editMenu.IsOpen;
            fileMenu.IsOpen = false;
            viewMenu.IsOpen = false;
        }
        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            viewMenu.IsOpen = !viewMenu.IsOpen;
            fileMenu.IsOpen = false;
            editMenu.IsOpen = false;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            fileMenu.IsOpen = false;
            _myVM.NewCommand();
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            fileMenu.IsOpen = false;

            //OpenFileDialog dialog = new OpenFileDialog() { Multiselect = false };
            //dialog.Filter = "Silver Designer Projects | *.sdp";
            //if (dialog.ShowDialog().GetValueOrDefault())
            //    _myVM.OpenCommand(dialog.File.FullName);

            var res = FileUploaderDialog.UploadFiles().GetAwaiter().GetResult();
            if (res.Count > 0)
            {
                string fileName = res[0].name;
                string fileContent = res[0].text;

                //MessageBox.Show(fileName);
                _myVM.OpenCommand(new Tuple<string,string>(fileName, fileContent));
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            fileMenu.IsOpen = false;

            //SaveFileDialog dialog = new SaveFileDialog();
            //dialog.Filter = "Silver Designer Projects | *.sdp";

            //if (DOM.DocumentVM.Instance != null)
            //    dialog.DefaultFileName = DOM.DocumentVM.Instance.Name;
            //else
            //    dialog.DefaultFileName = "Document";

            //if (dialog.ShowDialog().GetValueOrDefault())
            //    _myVM.SaveCommand(dialog.SafeFileName);

            FileDownloaderDialog dialog = new FileDownloaderDialog();
            dialog.Accept += async (s1, e1) =>
            {
                await FileSaver.SaveTextToFile("MyText", dialog.FileName);
                _myVM.SaveCommand(dialog.FileName);
            };

            dialog.ShowDialog();
        }

        private void btnAddPage_Click(object sender, RoutedEventArgs e)
        {
            editMenu.IsOpen = false;
            _myVM.AddPageCommand();
        }

        private void btnHTMLEditor_Click(object sender, RoutedEventArgs e)
        {
            editMenu.IsOpen = false;
            _myVM.ShowHTMLEditorCommand();
        }

        private void btnPageView_Checked(object sender, RoutedEventArgs e)
        {
            if (viewMenu == null)
                return;

            viewMenu.IsOpen = false;
            _myVM.PageViewCommand();
        }
        private void btnDocumentView_Checked(object sender, RoutedEventArgs e)
        {
            viewMenu.IsOpen = false;
            _myVM.DocumentViewCommand();
        }

        private void btnResources_Click(object sender, RoutedEventArgs e)
        {
            closeAllPopups();
            _myVM.ResourcesViewCommand();
        }
    }
}
