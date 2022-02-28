using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class PageVM : ContainerVM
    {
        public PageVM()
        {
            Name = "Page";

            _pagePosition = new PagePositionVM();
            _backgroundColor = Colors.White;

            properties.Add(new PropertyWrapper(this, typeof(PageVM).GetProperty("PagePosition")));
        }

        private PagePositionVM _pagePosition;
        public PagePositionVM PagePosition
        {
            get { return _pagePosition; }
            set
            {
                _pagePosition = value;
                NotifyPropertyChanged("PagePosition");
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public MarginVM PageMargin
        {
            get
            {
                int c = _pagePosition.ColumnIndex;
                int r = _pagePosition.RowIndex;
                return _margin + new MarginVM { Left = c * _width + 5, Top = r * _height + 5, Right = 5, Bottom = 5 };
            }
        }

        public UIElement AsXAMLUIElementInOverView()
        {
            Border retVal = new Border
            {
                Name = XAMLBorderName,
                Width = _width,
                Height = _height,
                Margin = PageMargin.ToThickness(),
                Padding = new Thickness(0),
                VerticalAlignment = _verticalAlignment,
                HorizontalAlignment = _horizontalAlignment,
                CornerRadius = new System.Windows.CornerRadius(_cornerRadius),
                Background = new SolidColorBrush(_backgroundColor),
                BorderBrush = new SolidColorBrush(_borderColor),
                BorderThickness = new Thickness(_borderThickness),
                Child = new TextBlock
                {
                    FontSize = 36,
                    Foreground = new SolidColorBrush(Colors.Brown),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Text = _name
                }
            };
            Canvas.SetZIndex(retVal, _zIndex);
            return retVal;
        }

        public override VMBase Clone()
        {
            PageVM retVal = new PageVM
            {
                _name = this._name,
                _width = this._width,
                _height = this._height,
                _margin = this._margin.Clone() as MarginVM,
                _horizontalAlignment = this._horizontalAlignment,
                _verticalAlignment = this._verticalAlignment,
                _horizontalScrollEnabled = this._horizontalScrollEnabled,
                _verticalScrollEnabled = this._verticalScrollEnabled,
                _contentHeight = this._contentHeight,
                _contentWidth = this._contentWidth,
                _zIndex = this._zIndex,
                _padding = this._padding,
                _borderColor = this._borderColor,
                _borderThickness = this._borderThickness,
                _cornerRadius = this._cornerRadius,
                _backgroundColor = this._backgroundColor,
                _pagePosition = this._pagePosition.Clone() as PagePositionVM,
            };

            foreach (UIObjectVM c in _children)
                retVal.AddChild(c.Clone() as UIObjectVM);

            return retVal;
        }
    }

    public class PagePositionVM : VMBase
    {
        private int _columnIndex;
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set
            {
                _columnIndex = value;
                NotifyPropertyChanged("ColumnIndex");
            }
        }

        private int _rowIndex;
        public int RowIndex
        {
            get { return _rowIndex; }
            set
            {
                _rowIndex = value;
                NotifyPropertyChanged("RowIndex");
            }
        }

        public override VMBase Clone()
        {
            return new PagePositionVM
            {
                _columnIndex = this._columnIndex,
                _rowIndex = this._rowIndex
            };
        }

        public override string ToString()
        {
            return (_rowIndex + 1) + ";" + (_columnIndex + 1);
        }

        public static PagePositionVM FromString(string str)
        {
            var values = str.Split(';');
            PagePositionVM retVal = new PagePositionVM();
            try { retVal.RowIndex = values[0].ToInt() - 1; }
            catch (Exception) { }
            try { retVal.ColumnIndex = values[1].ToInt() - 1; }
            catch (Exception) { }
            return retVal;
        }
    }
}