using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class DocumentVM : ContainerVM
    {
        public event SelectedChangeEventHandler AnObjectSelected;
        public event ObjectChangedEventHandler AnObjectPropertyChanged;

        public static DocumentVM Instance { get; set; }

        public DocumentVM()
        {
            Name = "NewDocument";
            Orientation = Orientation.Horizontal;
            this.SelectedChildChanged += DocumentVM_SelectedChildChanged;

            _resources = new ResourcesVM();

            Instance = this;
        }

        public override VMBase Clone()
        {
            DocumentVM retVal = new DocumentVM
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
                _orientation = this._orientation,
                _fileName = this._fileName,
                _resources = this._resources
            };

            foreach (PageVM p in _children)
                retVal.AddPage(p.Clone() as PageVM);

            return retVal;
        }

        #region Methods
        private void arrangeResolution(Orientation or)
        {
            switch (or)
            {
                case Orientation.Horizontal:
                    Width = 1024;
                    Height = 768;
                    break;
                case Orientation.Vertical:
                    Width = 768;
                    Height = 1024;
                    break;
                default:
                    break;
            }
        }
        public void InitFirstPage()
        {
            AddNewPage();
        }
        public void AddNewPage()
        {
            var page = new PageVM()
            {
                Width = this.Width,
                Height = this.Height,
            };

            AddPage(page);
        }
        public void AddPage(PageVM page)
        {
            page.SelectedChildChanged += SelectedChild_SelectedChildChanged;
            page.AnObjectPropertyChanged += DocumentVM_AnObjectPropertyChanged;
            AddChild(page);
            //SelectedChild = page;
        }
        #endregion
        #region events and handlers
        private void DocumentVM_AnObjectPropertyChanged(ContainerVM container, UIObjectVM obj, string propertyName)
        {
            if (AnObjectPropertyChanged != null)
            {
                if (container == null)
                    container = this;

                AnObjectPropertyChanged(container, obj, propertyName);
            }
        }
        private void DocumentVM_SelectedChildChanged(UIObjectVM sender, UIObjectVM selected)
        {
            this.SelectedAnyChild = selected;

            if (AnObjectPropertyChanged != null)
                AnObjectPropertyChanged(this, selected, "ObjectSelected");
        }
        private void SelectedChild_SelectedChildChanged(UIObjectVM sender, UIObjectVM selected)
        {
            if (AnObjectSelected != null)
                AnObjectSelected(sender, selected);

            this.SelectedAnyChild = selected;
        }
        #endregion
        #region Properties
        private string _fileName;
        [XmlIgnore()]
        [JsonIgnore()]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        private Orientation _orientation;
        [XmlAttribute()]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                NotifyPropertyChanged("Orientation");
                arrangeResolution(_orientation);
            }
        }

        private ResourcesVM _resources;
        public ResourcesVM Resources
        {
            get { return _resources; }
            set
            {
                _resources = value;
                NotifyPropertyChanged("Resources");
            }
        }
        #endregion
        #region Json
        public static byte[] SerializeToJsonBytes(DocumentVM document)
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            jsonSerializer.Formatting = Formatting.Indented;
            jsonSerializer.TypeNameHandling = TypeNameHandling.All;
            jsonSerializer.TypeNameAssemblyFormat = FormatterAssemblyStyle.Full;
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    var jsonWriter = new JsonTextWriter(sw);
                    jsonSerializer.Serialize(jsonWriter, document);
                    jsonWriter.Flush();
                    byte[] myBytes = ms.ToArray();
                    return myBytes;
                }
            }
        }
        public static DocumentVM DeserializeFromJsonBytes(byte[] jsonBytes)
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            jsonSerializer.Formatting = Formatting.Indented;
            jsonSerializer.TypeNameHandling = TypeNameHandling.All;
            jsonSerializer.TypeNameAssemblyFormat = FormatterAssemblyStyle.Full;
            byte[] myBytes = jsonBytes;
            using (var ms = new MemoryStream(myBytes))
            {
                using (var sr = new StreamReader(ms))
                {
                    var jsonReader = new JsonTextReader(sr);
                    var retVal = jsonSerializer.Deserialize<DocumentVM>(jsonReader);
                    return retVal;
                }
            }
        }
        public static string SerializeToJsonString(DocumentVM document)
        {
            var myBytes = SerializeToJsonBytes(document);
            string retVal = Encoding.UTF8.GetString(myBytes, 0, myBytes.Length);
            return retVal;
        }
        public static DocumentVM DeserializeFromJsonString(string jsonString)
        {
            var jsonSerializer = new JsonSerializer();
            byte[] myBytes = Encoding.UTF8.GetBytes(jsonString);
            return DeserializeFromJsonBytes(myBytes);
        }

        public string ToJsonString() { return SerializeToJsonString(this); }
        public byte[] ToJsonBytes() { return SerializeToJsonBytes(this); }
        #endregion
    }
}