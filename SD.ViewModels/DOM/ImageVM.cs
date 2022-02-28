using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class ImageVM : UIObjectVM
    {
        public ImageVM()
        {
            Name = "Image";

            _url = string.Empty;

            properties.Add(new PropertyWrapper(this, typeof(ImageVM).GetProperty("Url")));
            properties.Add(new PropertyWrapper(this, typeof(ImageVM).GetProperty("ResourceName")));
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

        private string defaultImagePath { get { return "http://" + hostName + "/Images/ImageObjectIcon.png"; } }

        public override UIElement AsXAMLUIElement()
        {
            Image retVal = new Image
            {
                Name = base.Name,
                VerticalAlignment = base.VerticalAlignment,
                HorizontalAlignment = base.HorizontalAlignment,
                Margin = base.Margin.ToThickness(),
                Width = base.Width,
                Height = base.Height,
                Stretch = System.Windows.Media.Stretch.Fill
            };

            Canvas.SetZIndex(retVal, _zIndex);

            if (_selectedResource != null)
                try { retVal.Source = _selectedResource.ImageDataSource; }
                catch (Exception) { retVal.Source = new BitmapImage(new Uri(defaultImagePath)); }
            else
                try { retVal.Source = new BitmapImage(new Uri(_url)); }
                catch (Exception) { retVal.Source = new BitmapImage(new Uri(defaultImagePath)); }

            return retVal;
        }

        public override VMBase Clone()
        {
            ImageVM retVal = new ImageVM
            {
                _name = this._name,
                _width = this._width,
                _height = this._height,
                _margin = this._margin.Clone() as MarginVM,
                _horizontalAlignment = this._horizontalAlignment,
                _verticalAlignment = this._verticalAlignment,
                _zIndex = this._zIndex,
                _url = this._url,
                ResourceName = this.ResourceName
            };

            return retVal;
        }

        [XmlIgnore]
        [JsonIgnore]
        public IEnumerable<ResourceBase> ImageResources
        {
            get { return DocumentVM.Instance.Resources.Resources.Where(x => x.GetType() == typeof(ImageResource)); }
        }

        private ImageResource _selectedResource;
        [XmlIgnore]
        [JsonIgnore]
        public ImageResource SelectedResource
        {
            get { return _selectedResource; }
            set
            {
                _selectedResource = value;
                NotifyPropertyChanged("SelectedResource");
                NotifyPropertyChanged("ResourceName");
            }
        }

        public string ResourceName
        {
            get
            {
                if (_selectedResource != null)
                    return _selectedResource.Name;
                else
                    return string.Empty;
            }
            set
            {
                try { SelectedResource = (ImageResource)ImageResources.SingleOrDefault(r => r.Name == value); }
                catch (Exception) { }
            }
        }
    }
}