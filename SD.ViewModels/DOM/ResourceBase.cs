using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public abstract class ResourceBase : VMBase
    {
        public enum Types { None, Image, Video }

        protected byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set
            {
                _data = value;
                arrangeDataStream();
                NotifyPropertyChanged("ImageDataSource");
            }
        }

        private void arrangeDataStream()
        {
            if (DataStream != null)
            {
                DataStream.Dispose();
                DataStream = null;
            }
            DataStream = new MemoryStream(_data);
        }

        [XmlIgnore]
        [JsonIgnore]
        public MemoryStream DataStream { get; private set; }

        protected string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
    }
}
