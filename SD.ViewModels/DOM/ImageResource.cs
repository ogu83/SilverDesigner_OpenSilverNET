using Newtonsoft.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class ImageResource : ResourceBase
    {
        public ImageResource() { }
        public ImageResource(string name, byte[] data)
        {
            _name = name;
            Data = data;
        }
        
        [JsonIgnore]
        [XmlIgnore]
        public ImageSource ImageDataSource
        {
            get
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(DataStream);
                return bitmap;
            }
        }
    }
}