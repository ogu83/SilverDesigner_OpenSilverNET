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
    public class ToolBoxMenuVM : VMBase
    {
        public enum ToolEnum { Hand, WhiteArrow, BlackArrow, Container, Image, Text, Video }
        public delegate void OnToolSelectedEventHandler(ToolBoxMenuVM sender, ToolEnum selectedTool);
        public event OnToolSelectedEventHandler OnToolSelected;

        private ToolEnum _selectedTool;
        public ToolEnum SelectedTool
        {
            get { return _selectedTool; }
            set
            {
                _selectedTool = value;
                if (OnToolSelected != null)
                    OnToolSelected(this, _selectedTool);
                NotifyPropertyChanged("SelectedTool");
            }
        }
    }
}
