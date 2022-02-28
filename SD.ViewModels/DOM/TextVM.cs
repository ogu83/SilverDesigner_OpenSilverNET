using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class TextVM : UIObjectVM
    {
        public TextVM()
        {
            Name = "Text";
            Text = "lorem ipsum lorem ipsum";

            _text = string.Empty;
            _textAlignment = System.Windows.TextAlignment.Left;

            properties.Add(new PropertyWrapper(this, typeof(TextVM).GetProperty("Text")));
        }

        private string _text;
        [XmlAttribute()]
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                NotifyPropertyChanged("Text");
            }
        }

        private TextAlignment _textAlignment;
        [XmlAttribute()]
        public TextAlignment TextAlignment
        {
            get { return _textAlignment; }
            set
            {
                _textAlignment = value;
                NotifyPropertyChanged("TextAlignment");
            }
        }

        public override UIElement AsXAMLUIElement()
        {
            WebBrowser retVal = new WebBrowser
            {
                Width = _width,
                Height = _height,
                Margin = _margin.ToThickness(),
                VerticalAlignment = _verticalAlignment,
                HorizontalAlignment = _horizontalAlignment,
            };
            retVal.NavigateToString(_text);
            return retVal;
        }

        public override VMBase Clone()
        {
            TextVM retVal = new TextVM
            {
                _name = this._name,
                _width = this._width,
                _height = this._height,
                _margin = this._margin.Clone() as MarginVM,
                _horizontalAlignment = this._horizontalAlignment,
                _verticalAlignment = this._verticalAlignment,
                _zIndex = this._zIndex,
                _textAlignment = this._textAlignment,
                _text = this._text
            };

            return retVal;
        }
    }
}