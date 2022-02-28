using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SilverDesigner
{
    public class EditAreaVM : VMBase
    {
        public EditAreaVM()
        {
            Document = new DOM.DocumentVM();
        }

        public enum ViewEnum { Page, Document };

        public event EventHandler DocumentChanged;
        private DOM.DocumentVM _document;
        public DOM.DocumentVM Document
        {
            get { return _document; }
            set
            {
                if (_document != null)
                {
                    _document.AnObjectSelected -= _document_AnObjectSelected;
                    _document.AnObjectPropertyChanged -= _document_AnObjectPropertyChanged;
                }

                _document = value;

                _document.AnObjectSelected += _document_AnObjectSelected;
                _document.AnObjectPropertyChanged += _document_AnObjectPropertyChanged;

                if (DocumentChanged != null)
                    DocumentChanged(this, null);

                if (AnObjectPropertyChanged != null)
                    AnObjectPropertyChanged(_document, _document.SelectedChild, "newDocument");

                NotifyPropertyChanged("Document");
            }
        }

        public delegate void OnShowViewEventHandler(EditAreaVM sender, ViewEnum view);
        public event OnShowViewEventHandler OnShowView;

        public event SelectedChangeEventHandler AnObjectSelected;
        public event ObjectChangedEventHandler AnObjectPropertyChanged;
        private void _document_AnObjectSelected(DOM.UIObjectVM sender, DOM.UIObjectVM selected)
        {
            if (AnObjectSelected != null)
                AnObjectSelected(sender, selected);
        }
        private void _document_AnObjectPropertyChanged(DOM.ContainerVM container, DOM.UIObjectVM obj, string propertyName)
        {
            if (AnObjectPropertyChanged != null)
                AnObjectPropertyChanged(container, obj, propertyName);
        }

        public List<int> ZoomFactors
        {
            get { return (new int[] { 5, 12, 25, 50, 75, 100, 125, 150, 200, 400, 600, 800, 1200, 1600, 2400, 3200, 4000 }).ToList(); }
        }
        private int _zoomFactor = 100;
        public int ZoomFactor
        {
            get { return _zoomFactor; }
            set
            {
                _zoomFactor = value;
                NotifyPropertyChanged("ZoomFactor");
                NotifyPropertyChanged("ScaledWidth");
                NotifyPropertyChanged("ScaledHeight");
            }
        }
        public double ScaledWidth { get { return _document.Width * _zoomFactor * .01; } }
        public double ScaledHeight { get { return _document.Height * _zoomFactor * .01; } }

        private Cursor _mouseCursor;
        public Cursor MouseCursor
        {
            get { return _mouseCursor; }
            set
            {
                _mouseCursor = value;
                NotifyPropertyChanged("MouseCursor");
            }
        }

        private Point _mousePosition;
        public Point MousePosition
        {
            get { return _mousePosition; }
            set
            {
                _mousePosition = value;
                NotifyPropertyChanged("MousePosition");
            }
        }

        public ToolBoxMenuVM.ToolEnum SelectedDrawTool { get; private set; }
        public void SetTool(ToolBoxMenuVM.ToolEnum selectedTool)
        {
            SelectedDrawTool = selectedTool;
            switch (selectedTool)
            {
                case ToolBoxMenuVM.ToolEnum.Hand:
                    MouseCursor = Cursors.Hand;
                    break;
                case ToolBoxMenuVM.ToolEnum.WhiteArrow:
                    MouseCursor = Cursors.Arrow;
                    break;
                case ToolBoxMenuVM.ToolEnum.BlackArrow:
                    MouseCursor = Cursors.Stylus;
                    break;
                case ToolBoxMenuVM.ToolEnum.Container:
                case ToolBoxMenuVM.ToolEnum.Image:
                case ToolBoxMenuVM.ToolEnum.Video:
                case ToolBoxMenuVM.ToolEnum.Text:
                    MouseCursor = Cursors.Stylus;
                    break;
                default:
                    MouseCursor = Cursors.Arrow;
                    break;
            }
        }

        public void ShowView(ViewEnum view)
        {
            if (OnShowView != null)
                OnShowView(this, view);
        }
    }
}