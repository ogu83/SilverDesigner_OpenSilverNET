using System;
using System.Windows;
using System.Xml.Serialization;

namespace SilverDesigner.DOM
{
    public class MarginVM : VMBase
    {
        public static MarginVM FromThickness(Thickness t)
        {
            MarginVM m = new MarginVM
            {
                _bottom = t.Bottom,
                _top = t.Top,
                _left = t.Left,
                _right = t.Right
            };
            return m;
        }

        private double _left;
        [XmlAttribute()]
        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                NotifyPropertyChanged("Left");
            }
        }

        private double _top;
        [XmlAttribute()]
        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                NotifyPropertyChanged("Top");
            }
        }

        private double _right;
        [XmlAttribute()]
        public double Right
        {
            get { return _right; }
            set
            {
                _right = value;
                NotifyPropertyChanged("Right");
            }
        }

        private double _bottom;
        [XmlAttribute()]
        public double Bottom
        {
            get { return _bottom; }
            set
            {
                _bottom = value;
                NotifyPropertyChanged("Bottom");
            }
        }

        public Thickness ToThickness()
        {
            return new Thickness(_left, _top, _right, _bottom);
        }

        public override string ToString()
        {
            return Math.Round(_left, 0) + ";" + Math.Round(_top, 0) + ";" + Math.Round(_right, 0) + ";" + Math.Round(_bottom, 0);
        }

        public static MarginVM FromString(string str)
        {
            var values = str.Split(';');
            MarginVM retVal = new MarginVM();
            try { retVal.Left = values[0].ToDouble(); }
            catch (Exception) { }
            try { retVal.Top = values[1].ToDouble(); }
            catch (Exception) { }
            try { retVal.Right = values[2].ToDouble(); }
            catch (Exception) { }
            try { retVal.Bottom = values[3].ToDouble(); }
            catch (Exception) { }
            return retVal;
        }

        public override VMBase Clone()
        {
            return new MarginVM
            {
                _left = this._left,
                _top = this._top,
                _right = this._right,
                _bottom = this._bottom
            };
        }

        public static MarginVM operator -(MarginVM x, MarginVM y)
        {
            return new MarginVM
            {
                _left = x._left - y._left,
                _top = x._top - y._top,
                _right = x._right - y._right,
                _bottom = x._bottom - y._bottom,
            };
        }
        public static MarginVM operator +(MarginVM x, MarginVM y)
        {
            return new MarginVM
            {
                _left = x._left + y._left,
                _top = x._top + y._top,
                _right = x._right + y._right,
                _bottom = x._bottom + y._bottom,
            };
        }
    }
}