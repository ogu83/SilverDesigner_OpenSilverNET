using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class ContainerVM : UIObjectVM
    {
        public ContainerVM()
        {
            _children = new ObservableCollection<UIObjectVM>();

            _backgroundColor = Colors.Transparent;
            _borderColor = Colors.Transparent;
            _padding = new MarginVM();

            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("Padding")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("BorderThickness")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("BorderColor")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("CornerRadius")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("BackgroundColor")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("VerticalScrollEnabled")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("HorizontalScrollEnabled")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("ContentWidth")));
            properties.Add(new PropertyWrapper(this, typeof(ContainerVM).GetProperty("ContentHeight")));
        }

        [XmlIgnore]
        [JsonIgnore]
        public string XAMLBorderName { get { return "_brd_" + Name; } }
        [XmlIgnore]
        [JsonIgnore]
        public string XAMLScrollViewerName { get { return "_scr_" + Name; } }
        [XmlIgnore]
        [JsonIgnore]
        public string XAMLGridName { get { return "_grd_" + Name; } }

        public override UIElement AsXAMLUIElement()
        {
            Border retVal = new Border
            {
                Name = XAMLBorderName,
                Width = _width,
                Height = _height,
                Margin = _margin.ToThickness(),
                Padding = new Thickness(0),
                VerticalAlignment = _verticalAlignment,
                HorizontalAlignment = _horizontalAlignment,
                CornerRadius = new System.Windows.CornerRadius(_cornerRadius),
                Background = new SolidColorBrush(_backgroundColor),
                BorderBrush = new SolidColorBrush(_borderColor),
                BorderThickness = new Thickness(_borderThickness),
                Child = new ScrollViewer
                {
                    Name = XAMLScrollViewerName,
                    VerticalScrollBarVisibility = VerticalScrollEnabled.ToScrollBarVisibility(),
                    HorizontalScrollBarVisibility = HorizontalScrollEnabled.ToScrollBarVisibility(),
                    Margin = _padding.ToThickness(),
                    Padding = new Thickness(0),
                    BorderBrush = null,
                    BorderThickness = new Thickness(0),
                    Content = new Grid()
                    {
                        Name = XAMLGridName,
                        Width = _contentWidth,
                        Height = _contentHeight,
                        VerticalAlignment = System.Windows.VerticalAlignment.Top,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                        Background = new SolidColorBrush(_backgroundColor)
                    }
                }
            };
            Canvas.SetZIndex(retVal, _zIndex);
            return retVal;
        }

        public override VMBase Clone()
        {
            ContainerVM retVal = new ContainerVM
            {
                _name = this._name,
                _width = this._width,
                _height = this._height,
                _margin = this._margin.Clone() as MarginVM,
                _horizontalAlignment = this._horizontalAlignment,
                _verticalAlignment = this._verticalAlignment,
                _zIndex = this._zIndex,
                _padding = this._padding,
                _borderThickness = this._borderThickness,
                _borderColor = this._borderColor,
                _cornerRadius = this._cornerRadius,
                _backgroundColor = this._backgroundColor,
                _horizontalScrollEnabled = this._horizontalScrollEnabled,
                _verticalScrollEnabled = this._verticalScrollEnabled,
                _contentHeight = this._contentHeight,
                _contentWidth = this._contentWidth,
            };

            foreach (UIObjectVM c in _children)
                retVal.AddChild(c.Clone() as UIObjectVM);

            return retVal;
        }

        #region properties
        public override MarginVM MarginToDocument { get { return base.MarginToDocument + _padding; } }

        protected MarginVM _padding;
        [XmlAttribute()]
        public MarginVM Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                NotifyPropertyChanged("Padding");
            }
        }

        protected double _borderThickness;
        [XmlAttribute()]
        public double BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                _borderThickness = value;
                NotifyPropertyChanged("BorderThickness");
            }
        }

        protected Color _borderColor;
        [XmlAttribute()]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                NotifyPropertyChanged("BorderColor");
            }
        }

        protected int _cornerRadius;
        [XmlAttribute()]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value;
                NotifyPropertyChanged("CornerRadius");
            }
        }

        protected Color _backgroundColor;
        [XmlAttribute()]
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                NotifyPropertyChanged("BackgroundColor");
            }
        }

        protected bool _verticalScrollEnabled;
        [XmlAttribute()]
        public bool VerticalScrollEnabled
        {
            get { return _verticalScrollEnabled; }
            set
            {
                _verticalScrollEnabled = value;
                NotifyPropertyChanged("VerticalScrollEnabled");

                if (!_verticalScrollEnabled)
                    ContentHeight = Height;
            }
        }

        protected bool _horizontalScrollEnabled;
        [XmlAttribute()]
        public bool HorizontalScrollEnabled
        {
            get { return _horizontalScrollEnabled; }
            set
            {
                _horizontalScrollEnabled = value;
                NotifyPropertyChanged("HorizontalScrollEnabled");

                if (!_horizontalScrollEnabled)
                    ContentWidth = Width;
            }
        }

        protected double _contentWidth;
        [XmlAttribute()]
        public double ContentWidth
        {
            get { return _contentWidth; }
            set
            {
                _contentWidth = value;
                NotifyPropertyChanged("ContentWidth");
            }
        }

        protected double _contentHeight;
        [XmlAttribute()]
        public double ContentHeight
        {
            get { return _contentHeight; }
            set
            {
                _contentHeight = value;
                NotifyPropertyChanged("ContentHeight");
            }
        }
        #endregion
        #region child issues
        protected ObservableCollection<UIObjectVM> _children;
        public ObservableCollection<UIObjectVM> Children
        {
            get { return _children; }
            set
            {
                _children = value;
                NotifyPropertyChanged("Children");
            }
        }

        protected UIObjectVM _selectedChild;
        [XmlIgnore]
        [JsonIgnore]
        public UIObjectVM SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                NotifyPropertyChanged("SelectedChild");

                SelectedAnyChild = _selectedChild;

                if (SelectedChildChanged != null)
                    SelectedChildChanged(this, _selectedChild);
            }
        }

        protected UIObjectVM _selectedAnyChild;
        [XmlIgnore()]
        [JsonIgnore()]
        public UIObjectVM SelectedAnyChild
        {
            get { return _selectedAnyChild; }
            set
            {
                _selectedAnyChild = value;
                NotifyPropertyChanged("SelectedAnyChild");

                if (SelectedAnyChildChanged != null)
                    SelectedAnyChildChanged(this, _selectedAnyChild);
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<UIObjectVM> ChildContainers { get { return Children.Where(c => c as ContainerVM != null); } }
        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<UIObjectVM> ChildElements { get { return Children.Where(c => c as ContainerVM == null); } }
        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<UIObjectVM> EveryChildren
        {
            get
            {
                var retVal = _children.AsEnumerable<UIObjectVM>();

                foreach (ContainerVM con in ChildContainers)
                    retVal = retVal.Union(con.EveryChildren);

                return retVal;
            }
        }

        public void FixAllObjectNames()
        {
            foreach (var c in Children)
                fixObjNaming(c);
        }
        private void fixObjNaming(UIObjectVM obj)
        {
            int n = 0;
            while (DocumentVM.Instance.CheckChildExist(obj.Name))
            {
                if (!obj.Name.Contains('_'))
                    obj.Name += '_' + (n++).ToString();
                else
                    obj.Name = (obj.Name.Split("_".ToCharArray())[0]) + "_" + (n++).ToString();
            }
        }

        public bool CheckChildExist(string name, UIObjectVM except)
        {
            bool retVal = _children.Count(c => c.Name == name && c != except) > 0;
            if (!retVal)
            {
                foreach (ContainerVM container in ChildContainers)
                {
                    bool inRetVal = container.CheckChildExist(name, except);
                    if (inRetVal)
                        return inRetVal;
                }
            }
            return retVal;
        }
        public bool CheckChildExist(string name)
        {
            bool retVal = _children.Count(c => c.Name == name) > 0;
            if (!retVal)
            {
                foreach (ContainerVM container in ChildContainers)
                {
                    bool inRetVal = container.CheckChildExist(name);
                    if (inRetVal)
                        return inRetVal;
                }
            }
            return retVal;
        }
        public bool CheckChildExist(UIObjectVM obj)
        {
            return _children.Contains(obj);
        }

        public void AddChild(UIObjectVM obj)
        {
            obj.Parent = this;

            fixObjNaming(obj);

            obj.Selected += obj_Selected;
            obj.PropertyChanged += obj_PropertyChanged;
            obj.NameChanged += obj_NameChanged;
            obj.Deleted += obj_Deleted;
            var con = obj as ContainerVM;
            if (con != null)
            {
                con.SelectedAnyChildChanged += con_SelectedAnyChildChanged;
                con.AnObjectPropertyChanged += con_AnObjectPropertyChanged;
            }

            Children.Add(obj);
            SelectedChild = obj;

            if (AnObjectPropertyChanged != null)
                AnObjectPropertyChanged(this, obj, "added");
        }

        public void RemoveChild(string name)
        {
            var objList = Children.Where(c => c.Name == name).ToList();
            RemoveChild(objList[0]);
        }
        public void RemoveChild(UIObjectVM obj)
        {
            obj.Selected -= obj_Selected;
            obj.PropertyChanged -= obj_PropertyChanged;
            obj.NameChanged -= obj_NameChanged;
            obj.Deleted -= obj_Deleted;

            var con = obj as ContainerVM;
            if (con != null)
            {
                con.SelectedAnyChildChanged -= con_SelectedAnyChildChanged;
                con.AnObjectPropertyChanged -= con_AnObjectPropertyChanged;
            }

            if (SelectedChild == obj)
                SelectedChild = null;

            Children.Remove(obj);
        }

        public void UpdateChildName(UIObjectVM obj, string name)
        {
            if (!Children.Contains(obj))
                return;

            obj.Name = name;
            fixObjNaming(obj);
        }
        #endregion
        #region event and handlers
        public event SelectedChangeEventHandler SelectedChildChanged;
        public event SelectedChangeEventHandler SelectedAnyChildChanged;
        public event ObjectChangedEventHandler AnObjectPropertyChanged;
        private void obj_Deleted(object sender, RoutedEventArgs e)
        {
            var myObj = sender as UIObjectVM;
            if (this as DocumentVM != null && myObj as PageVM != null)
            {
                if (Children.Count == 1)
                {
                    MessageBox.Show("Document should have an least 1 page", "Page not deleted", MessageBoxButton.OK);
                    return;
                }
            }

            RemoveChild(myObj);

            if (AnObjectPropertyChanged != null)
                AnObjectPropertyChanged(this, sender as UIObjectVM, "deleted");
        }
        private void obj_NameChanged(ContainerVM container, UIObjectVM obj, string propertyName)
        {
            if (CheckChildExist(propertyName, obj))
                obj.CancelNameChange = true;
        }
        private void obj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (AnObjectPropertyChanged != null)
                AnObjectPropertyChanged(this, sender as UIObjectVM, e.PropertyName);
        }
        private void obj_Selected(UIObjectVM obj)
        {
            SelectedChild = obj;
            SelectedAnyChild = obj;
        }
        private void con_SelectedAnyChildChanged(UIObjectVM sender, UIObjectVM selected)
        {
            SelectedAnyChild = selected;
        }
        private void con_AnObjectPropertyChanged(ContainerVM container, UIObjectVM obj, string propertyName)
        {
            if (AnObjectPropertyChanged != null)
                AnObjectPropertyChanged(container, obj, propertyName);
        }
        #endregion
    }
}

