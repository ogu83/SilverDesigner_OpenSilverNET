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

namespace SilverDesigner.DOM
{
    public class VideoResource : ResourceBase
    {
        public VideoResource() { }
        public VideoResource(string name, byte[] data)
        {
            _name = name;
            Data = data;
        }
    }
}
