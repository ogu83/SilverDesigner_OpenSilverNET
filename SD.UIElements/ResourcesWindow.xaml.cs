using SD.UIElements;
using SilverDesigner.DOM;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SilverDesigner
{
    public partial class ResourcesWindow : ChildWindow
    {
        private ResourcesVM _myVM { get { return DataContext as ResourcesVM; } }

        public ResourcesWindow()
        {
            InitializeComponent();
        }
        public ResourcesWindow(ResourcesVM dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog fd = new OpenFileDialog();
            //if (fd.ShowDialog().GetValueOrDefault())
            //{
            //    byte[] data = File.ReadAllBytes(fd.File.FullName);
            //    _myVM.AddResourceCommand(fd.File.Name, ResourceBase.Types.Image, data);
            //}

            var res = FileUploaderDialog.UploadFiles().GetAwaiter().GetResult();
            if (res.Count > 0)
            {
                string fileName = res[0].name;
                string fileContent = res[0].text;
                byte[] data = Convert.FromBase64String(fileContent);
                _myVM.AddResourceCommand(fileName, ResourceBase.Types.Image, data);
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            _myVM.RemoveResourceCommand();
        }
    }
}

