using System.Windows;
using System.Windows.Controls;

namespace SilverDesigner
{
    public partial class ToolboxMenu : UserControl
    {
        private ToolBoxMenuVM _myVM { get { return DataContext as ToolBoxMenuVM; } }

        public ToolboxMenu()
        {
            InitializeComponent();
        }

        private void btnHand_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.Hand;
        }
        private void btnBlackArrow_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.BlackArrow;
        }
        private void btnWhiteArrow_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.WhiteArrow;
        }

        private void btnContainer_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.Container;
        }
        private void btnImage_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.Image;
        }
        private void btnText_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.Text;
        }
        private void btnVideo_Checked(object sender, RoutedEventArgs e)
        {
            _myVM.SelectedTool = ToolBoxMenuVM.ToolEnum.Video;
        }
    }
}
