using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class VideoVM : UIObjectVM
    {
        public VideoVM()
        {
            Name = "Video";

            _url = string.Empty;

            properties.Add(new PropertyWrapper(this, typeof(VideoVM).GetProperty("Url")));
        }

        private string _url;
        [XmlAttribute()]
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                NotifyPropertyChanged("Url");
            }
        }

        private string defaultImagePath { get { return "http://" + hostName + "/Images/VideoObjectIcon.png"; } }

        public override UIElement AsXAMLUIElement()
        {
            VideoPlayer retVal = new VideoPlayer(_url)
            {
                Name = base.Name,
                VerticalAlignment = base.VerticalAlignment,
                HorizontalAlignment = base.HorizontalAlignment,
                Margin = base.Margin.ToThickness(),
                Width = base.Width,
                Height = base.Height,
            };
            Canvas.SetZIndex(retVal, _zIndex);
            return retVal;
        }

        public override VMBase Clone()
        {
            VideoVM retVal = new VideoVM
            {
                _name = this._name,
                _width = this._width,
                _height = this._height,
                _margin = this._margin.Clone() as MarginVM,
                _horizontalAlignment = this._horizontalAlignment,
                _verticalAlignment = this._verticalAlignment,
                _zIndex = this._zIndex,
                _url = this._url
            };

            return retVal;
        }
    }
}