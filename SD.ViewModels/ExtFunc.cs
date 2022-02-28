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
using System.Windows.Threading;

namespace SilverDesigner
{
    public static class ExtFunc
    {
        #region Delayed Start
        public static void DelayedStart(Action action, TimeSpan interval)
        {
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Interval = interval;
            _timer.Tick += (object sender, EventArgs e) =>
            {
                action.Invoke();
                _timer.Stop();
                _timer = null;
            };
            _timer.Start();
        }
        public static void StartAfter(this Action action, TimeSpan interval)
        {
            DelayedStart(action, interval);
        }
        #endregion

        #region String Functions
        public static bool IsDouble(this string str)
        {
            double retVal;
            return double.TryParse(str, out retVal);
        }
        public static double ToDouble(this string str)
        {
            double retVal;
            double.TryParse(str, out retVal);
            return retVal;
        }
        public static bool IsInt(this string str)
        {
            int retVal;
            return int.TryParse(str, out retVal);
        }
        public static int ToInt(this string str)
        {
            int retVal;
            int.TryParse(str, out retVal);
            return retVal;
        }
        public static byte[] HexToBytes(this string str)
        {
            if (str.Length == 0 || str.Length % 2 != 0)
                return new byte[0];

            byte[] buffer = new byte[str.Length / 2];
            char c;
            for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
            {
                // Convert first half of byte
                c = str[sx];
                buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

                // Convert second half of byte
                c = str[++sx];
                buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
            }

            return buffer;
        }
        public static string ToHex(this byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            byte b;
            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b + 0x37 + 0x20 : b + 0x30);
            }
            return new string(c);
        }

        public static Color ToColor(this string str)
        {
            //#AARRGGBB
            Color retVal = new Color();

            string myVal = str;
            if (myVal.Contains("#"))
                myVal = myVal.Replace("#", "");

            var myBytes = myVal.HexToBytes();

            try { retVal.A = myBytes[0]; }
            catch (Exception) { }
            try { retVal.R = myBytes[1]; }
            catch (Exception) { }
            try { retVal.G = myBytes[2]; }
            catch (Exception) { }
            try { retVal.B = myBytes[3]; }
            catch (Exception) { }

            return retVal;
        }
        #endregion
        #region Bool Functions
        public static Visibility ToVisibility(this bool visible)
        {
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }
        public static bool ToBoolean(this Visibility visible)
        {
            return visible == Visibility.Visible;
        }
        public static int ToInt(this bool val)
        {
            return val ? 1 : 0;
        }
        public static bool ToBoolean(this int val)
        {
            return val != 0;
        }
        public static ScrollBarVisibility ToScrollBarVisibility(this bool visible)
        {
            return visible ? ScrollBarVisibility.Visible : ScrollBarVisibility.Disabled;
        }
        #endregion
    }
}