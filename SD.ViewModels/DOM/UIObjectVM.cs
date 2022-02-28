using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Serialization;
using System.Linq;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace SilverDesigner.DOM
{
    public abstract class UIObjectVM : VMBase
    {
        public UIObjectVM()
        {
            _margin = new MarginVM();

            wireCommands();
            wireProperties();
        }

        #region Event And Handlers
        public event ObjectChangedEventHandler NameChanged;
        public event SelectedEventHandler Selected;
        public event RoutedEventHandler Deleted;
        #endregion
        #region Properties
        private void wireProperties()
        {
            properties = new ObservableCollection<PropertyWrapper>();
            properties.Add(new PropertyWrapper(this, typeof(UIObjectVM).GetProperty("Name")));
            if (this as PageVM == null)
            {
                properties.Add(new PropertyWrapper(this, typeof(UIObjectVM).GetProperty("Margin")));
                properties.Add(new PropertyWrapper(this, typeof(UIObjectVM).GetProperty("Width")));
                properties.Add(new PropertyWrapper(this, typeof(UIObjectVM).GetProperty("Height")));
                properties.Add(new PropertyWrapper(this, typeof(UIObjectVM).GetProperty("ZIndex")));
            }
        }
        protected ObservableCollection<PropertyWrapper> properties;
        [XmlIgnore]
        [JsonIgnore]
        public ObservableCollection<PropertyWrapper> Properties { get { return properties; } }

        [XmlIgnore]
        [JsonIgnore]
        public ContainerVM Parent { get; internal set; }

        [XmlIgnore]
        [JsonIgnore]
        public bool CancelNameChange;
        protected string _name;
        [XmlAttribute()]
        public string Name
        {
            get { return _name; }
            set
            {
                if (NameChanged != null)
                    NameChanged(null, this, value);

                if (!CancelNameChange)
                    _name = value;
                else
                    CancelNameChange = false;

                NotifyPropertyChanged(Name);
            }
        }

        protected MarginVM _margin;
        [XmlElement()]
        public MarginVM Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                NotifyPropertyChanged("Margin");
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public virtual MarginVM MarginToDocument
        {
            get
            {
                if (Parent != null)
                    return _margin + Parent.MarginToDocument;
                else
                    return _margin;
            }
        }

        protected VerticalAlignment _verticalAlignment;
        [XmlAttribute()]
        public VerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set
            {
                _verticalAlignment = value;
                NotifyPropertyChanged("VerticalAlignment");
            }
        }

        protected HorizontalAlignment _horizontalAlignment;
        [XmlAttribute()]
        public HorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set
            {
                _horizontalAlignment = value;
                NotifyPropertyChanged("HorizontalAlignment");
            }
        }

        protected double _width;
        [XmlAttribute()]
        public double Width
        {
            get { return _width; }
            set
            {
                _width = Math.Round(value);
                NotifyPropertyChanged("Width");

                var con = this as ContainerVM;
                if (con != null)
                    if (!con.HorizontalScrollEnabled)
                        con.ContentWidth = _width;
            }
        }

        protected double _height;
        [XmlAttribute()]
        public double Height
        {
            get { return _height; }
            set
            {
                _height = Math.Round(value);
                NotifyPropertyChanged("Height");

                var con = this as ContainerVM;
                if (con != null)
                    if (!con.VerticalScrollEnabled)
                        con.ContentHeight = _height;
            }
        }

        protected int _zIndex;
        [XmlAttribute()]
        public int ZIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                NotifyPropertyChanged("ZIndex");
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        public string IdeIcon
        {
            get
            {
                if (this as VideoVM != null)
                    return "Images/Movies-icon.png";

                if (this as ImageVM != null)
                    return "Images/image-icon.png";

                if (this as TextVM != null)
                    return "Images/text-richtext-icon.png";

                return "Images/page-icon.png";
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        public Rect RelativeBounds { get { return new Rect(_margin.Left, _margin.Top, _width, _height); } }
        [JsonIgnore]
        [XmlIgnore]
        public Rect AbsoluteBounds { get { return new Rect(MarginToDocument.Left, MarginToDocument.Top, _width, _height); } }

        #endregion
        #region commands
        private void wireCommands()
        {
            DeleteCommand = new RelayCommand(Delete);
            DeleteCommand.IsEnabled = true;
        }

        [XmlIgnore]
        [JsonIgnore]
        public RelayCommand DeleteCommand { get; private set; }
        public void Delete()
        {
            if (Deleted != null)
                Deleted(this, null);
        }

        public void SelectCommand()
        {
            if (Selected != null)
                Selected(this);
        }
        #endregion
        #region virtuals
        public virtual UIElement AsXAMLUIElement()
        {
            return null;
        }
        public virtual string AsHTMLElement()
        {
            return null;
        }
        #endregion
    }
}