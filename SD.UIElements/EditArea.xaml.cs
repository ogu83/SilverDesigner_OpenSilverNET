using SilverDesigner.DOM;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SilverDesigner
{
    public partial class EditArea : UserControl
    {
        private EditAreaVM _myVM { get { return DataContext as EditAreaVM; } }
        private Point _lastPos;
        private bool _mouseLeftDown;
        private double _horizonalOffset;
        private double _verticalOffset;
        private Rectangle _addRect;
        private Rectangle _selRect;
        private double _objLeft, _objTop;

        public EditArea()
        {
            InitializeComponent();

            this.MouseMove += EditArea_MouseMove;
            this.DataContextChanged += EditArea_DataContextChanged;
        }

        private void EditArea_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_myVM != null)
            {
                _myVM.AnObjectSelected += _myVM_AnObjectSelected;
                _myVM.AnObjectPropertyChanged += _myVM_AnObjectPropertyChanged;
                _myVM.DocumentChanged += _myVM_DocumentChanged;
                _myVM.OnShowView += _myVM_OnShowView;
            }
        }

        private void _myVM_OnShowView(EditAreaVM sender, EditAreaVM.ViewEnum view)
        {
            switch (view)
            {
                case EditAreaVM.ViewEnum.Page:
                    invalidateDocumentView(_myVM.Document);
                    break;
                case EditAreaVM.ViewEnum.Document:
                    invalidateDocumentOverview();
                    break;
                default:
                    break;
            }
        }

        private void _myVM_AnObjectPropertyChanged(ContainerVM container, UIObjectVM obj, string propertyName)
        {
            invalidateDocumentView(container);
        }
        private void _myVM_AnObjectSelected(UIObjectVM sender, UIObjectVM selected)
        {
            invalidateSelection(selected);
        }
        private void _myVM_DocumentChanged(object sender, EventArgs e)
        {
            invalidateDocumentView(_myVM.Document);
        }

        #region Mouse Actions
        private void EditArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_mouseLeftDown)
                return;

            _mouseLeftDown = false;

            //if (sender != this.documentView)
            //    return;

            if (_addRect == null)
                return;

            var con = _myVM.Document.SelectedAnyChild as ContainerVM;
            var parentCon = _myVM.Document.SelectedAnyChild.Parent as ContainerVM;

            switch (_myVM.SelectedDrawTool)
            {
                case ToolBoxMenuVM.ToolEnum.Text:
                    var myText = new TextVM()
                    {
                        Name = "Text",
                        Height = _addRect.Height,
                        Width = _addRect.Width,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = MarginVM.FromThickness(_addRect.Margin)
                    };


                    if (con != null)
                    {
                        myText.Margin -= (con.MarginToDocument);
                        con.AddChild(myText);
                    }
                    else
                    {
                        myText.Margin -= (parentCon.MarginToDocument);
                        parentCon.AddChild(myText);
                    }

                    break;
                case ToolBoxMenuVM.ToolEnum.Image:
                    var myImage = new ImageVM()
                    {
                        Name = "Image",
                        Height = _addRect.Height,
                        Width = _addRect.Width,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = MarginVM.FromThickness(_addRect.Margin)
                    };


                    if (con != null)
                    {
                        myImage.Margin -= (con.MarginToDocument);
                        con.AddChild(myImage);
                    }
                    else
                    {
                        myImage.Margin -= (parentCon.MarginToDocument);
                        parentCon.AddChild(myImage);
                    }

                    break;
                case ToolBoxMenuVM.ToolEnum.Video:
                    var myVideo = new VideoVM()
                    {
                        Name = "Video",
                        Height = _addRect.Height,
                        Width = _addRect.Width,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = MarginVM.FromThickness(_addRect.Margin)
                    };


                    if (con != null)
                    {
                        myVideo.Margin -= (con.MarginToDocument);
                        con.AddChild(myVideo);
                    }
                    else
                    {
                        myVideo.Margin -= (parentCon.MarginToDocument);
                        parentCon.AddChild(myVideo);
                    }

                    break;
                case ToolBoxMenuVM.ToolEnum.Container:
                    var myContainer = new ContainerVM()
                    {
                        Name = "Container",
                        Height = _addRect.Height,
                        Width = _addRect.Width,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = MarginVM.FromThickness(_addRect.Margin),
                        BackgroundColor = Color.FromArgb(44, 255, 255, 255),
                        BorderColor = Colors.DarkGray,
                        BorderThickness = 1
                    };

                    if (con != null)
                    {
                        myContainer.Margin -= (con.MarginToDocument);
                        con.AddChild(myContainer);
                    }
                    else
                    {
                        myContainer.Margin -= (parentCon.MarginToDocument);
                        parentCon.AddChild(myContainer);
                    }

                    break;
                default:
                    break;
            }
            documentView.Children.Remove(_addRect);
            _addRect = null;
        }
        private void EditArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_mouseLeftDown)
                return;

            _mouseLeftDown = true;
            _lastPos = e.GetPosition(documentView);
            if (_mouseLeftDown)
            {
                switch (_myVM.SelectedDrawTool)
                {
                    case ToolBoxMenuVM.ToolEnum.Hand: //PAN
                        _horizonalOffset = scrollViewer.HorizontalOffset;
                        _verticalOffset = scrollViewer.VerticalOffset;
                        break;
                    case ToolBoxMenuVM.ToolEnum.WhiteArrow: //SELECTION
                    case ToolBoxMenuVM.ToolEnum.BlackArrow:
                        if (_myVM.Document.SelectedChild != null)
                        {
                            UIObjectVM myObj = (_myVM.Document.SelectedChild as PageVM)
                                                .EveryChildren.LastOrDefault(c => c.AbsoluteBounds.Contains(_lastPos));
                            if (myObj != null)
                            {
                                myObj.SelectCommand();
                                _objLeft = myObj.Margin.Left;
                                _objTop = myObj.Margin.Top;
                            }
                            else
                                (_myVM.Document.SelectedChild as PageVM).SelectCommand();
                        }
                        break;
                    case ToolBoxMenuVM.ToolEnum.Text: //OBJECT CREATION
                    case ToolBoxMenuVM.ToolEnum.Image:
                    case ToolBoxMenuVM.ToolEnum.Video:
                    case ToolBoxMenuVM.ToolEnum.Container:
                        _addRect = new Rectangle()
                        {
                            Margin = new Thickness(_lastPos.X, _lastPos.Y, 0, 0),
                            Width = 0,
                            Height = 0,
                            Fill = new SolidColorBrush(Color.FromArgb(10, 44, 44, 44)),
                            Stroke = new SolidColorBrush(Colors.Blue),
                            StrokeThickness = 1,
                        };
                        documentView.Children.Add(_addRect);
                        _addRect.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                        _addRect.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        break;
                    default:
                        break;
                }
            }
        }
        private void EditArea_MouseMove(object sender, MouseEventArgs e)
        {
            Point curPoss = e.GetPosition(documentView);
            _myVM.MousePosition = curPoss;

            if (_mouseLeftDown)
            {
                switch (_myVM.SelectedDrawTool)
                {
                    case ToolBoxMenuVM.ToolEnum.Hand: //PAN
                        scrollViewer.ScrollToHorizontalOffset(_horizonalOffset -
                            (curPoss.X - _lastPos.X) * _myVM.ZoomFactor * .005);
                        scrollViewer.ScrollToVerticalOffset(_verticalOffset -
                            (curPoss.Y - _lastPos.Y) * _myVM.ZoomFactor * .005);
                        break;
                    case ToolBoxMenuVM.ToolEnum.BlackArrow: //OBJECT MOVE
                        if (_myVM.Document.SelectedAnyChild != null)
                            _myVM.Document.SelectedAnyChild.Margin = new MarginVM
                            {
                                Left = _objLeft + (curPoss.X - _lastPos.X) * _myVM.ZoomFactor * .01,
                                Top = _objTop + (curPoss.Y - _lastPos.Y) * _myVM.ZoomFactor * .01,
                                Right = _myVM.Document.SelectedAnyChild.Margin.Right,
                                Bottom = _myVM.Document.SelectedAnyChild.Margin.Bottom
                            };
                        break;
                    case ToolBoxMenuVM.ToolEnum.Text: //OBJECT CREATION
                    case ToolBoxMenuVM.ToolEnum.Image:
                    case ToolBoxMenuVM.ToolEnum.Video:
                    case ToolBoxMenuVM.ToolEnum.Container:
                        _addRect.Width = Math.Max(curPoss.X - _lastPos.X, 0);
                        _addRect.Height = Math.Max(curPoss.Y - _lastPos.Y, 0);
                        break;
                    default:
                        break;
                }
            }
        }
        private void EditArea_MouseLeave(object sender, MouseEventArgs e)
        {
            _mouseLeftDown = false;
        }

        private void EditArea_Drop(object sender, DragEventArgs e)
        {
            //var pos = e.GetPosition(sender as UIElement);
            //var selectedFiles = e.Data.GetData(DataFormats.FileDrop) as FileInfo[];
            //FileInfo f = selectedFiles[0];
            //if (f != null)
            //{
            //    _myVM.Document.Resources.AddResourceCommand(f.Name, ResourceBase.Types.Image, File.ReadAllBytes(f.FullName));
            //    var s = _myVM.Document.SelectedAnyChild;
            //    if (s as ContainerVM != null)
            //    {
            //        (s as ContainerVM).AddChild(new ImageVM()
            //        {
            //            Width = 100,
            //            Height = 100,
            //            Margin = new MarginVM { Left = pos.X, Top = pos.Y },
            //            ResourceName = f.Name,
            //        });
            //    }
            //    else
            //    {
            //        (s.Parent as ContainerVM).AddChild(new ImageVM()
            //        {
            //            Width = 100,
            //            Height = 100,
            //            Margin = new MarginVM { Left = pos.X, Top = pos.Y },
            //            ResourceName = f.Name
            //        });
            //    }
            //}
        }
        #endregion
        #region Invalidation & ReArrange
        private void invalidateDocumentOverview()
        {
            documentView.Children.Clear();
            foreach (PageVM page in _myVM.Document.Children)
            {
                var xamlPage = page.AsXAMLUIElementInOverView() as Border;
                documentView.Height = Math.Max(documentView.Height, xamlPage.Margin.Top + xamlPage.Height);
                documentView.Width = Math.Max(documentView.Width, xamlPage.Margin.Left + xamlPage.Width);
                documentView.Children.Add(xamlPage);
            }
        }

        private void invalidateDocumentView(ContainerVM container)
        {
            documentView.Height = _myVM.Document.Height;
            documentView.Width = _myVM.Document.Width;

            if (container as DocumentVM != null)
            {
                documentView.Children.Clear();
                resetObjects(_myVM.Document.SelectedChild as PageVM);
                documentView.InvalidateArrange();
            }
            else
                resetObjects(container);

            invalidateSelection();
        }

        private void resetObjects(ContainerVM container)
        {
            if (container == null)
                return;

            var parentContainer = this.FindName(container.Parent.XAMLGridName) as Grid;
            if (parentContainer == null)
                parentContainer = documentView;

            var xamlContainer = this.FindName(container.XAMLBorderName) as Border;
            if (xamlContainer == null)
            {
                xamlContainer = container.AsXAMLUIElement() as Border;
                parentContainer.Children.Add(xamlContainer);
            }

            var xamlInnerGrid = xamlContainer.FindName(container.XAMLGridName) as Grid;

            xamlInnerGrid.MouseMove -= EditArea_MouseMove;
            xamlInnerGrid.MouseLeftButtonUp -= EditArea_MouseLeftButtonUp;
            xamlInnerGrid.MouseLeftButtonDown -= EditArea_MouseLeftButtonDown;
            //xamlInnerGrid.Drop -= EditArea_Drop;

            xamlInnerGrid.MouseMove += EditArea_MouseMove;
            xamlInnerGrid.MouseLeftButtonUp += EditArea_MouseLeftButtonUp;
            xamlInnerGrid.MouseLeftButtonDown += EditArea_MouseLeftButtonDown;
            //xamlInnerGrid.Drop += EditArea_Drop;

            xamlInnerGrid.AllowDrop = true;
            xamlInnerGrid.Children.Clear();
            foreach (UIObjectVM e in container.ChildElements)
            {
                var myUIelem = e.AsXAMLUIElement();
                xamlInnerGrid.Children.Add(myUIelem);
            }

            foreach (ContainerVM c in container.ChildContainers)
                resetObjects(c);
        }

        private void invalidateSelection()
        {
            if (_myVM != null)
                if (_myVM.Document != null)
                    if (_myVM.Document.SelectedAnyChild != null)
                        invalidateSelection(_myVM.Document.SelectedAnyChild);
        }
        private void invalidateSelection(UIObjectVM obj)
        {
            documentView.Children.Remove(_selRect);

            if (obj == null)
                return;

            _selRect = new Rectangle
            {
                Name = "_selRect",
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 10, 5 },
                Width = obj.Width + ((obj as PageVM == null) ? 6 : -6),
                Height = obj.Height + ((obj as PageVM == null) ? 6 : -6),
                VerticalAlignment = obj.VerticalAlignment,
                HorizontalAlignment = obj.HorizontalAlignment,
                Margin = (obj.MarginToDocument + new MarginVM
                {
                    Left = ((obj as PageVM == null) ? -3 : 3),
                    Top = ((obj as PageVM == null) ? -3 : 3),
                }).ToThickness(),
                IsHitTestVisible = false
            };
            documentView.Children.Add(_selRect);
            Canvas.SetZIndex(_selRect, 99999);
        }
        #endregion
    }
}