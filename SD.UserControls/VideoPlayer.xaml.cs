using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverDesigner
{
    public partial class VideoPlayer : UserControl
    {
        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(VideoPlayer), new PropertyMetadata(new PropertyChangedCallback(UrlChanged)));
        private static void UrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var myObj = d as VideoPlayer;
            if (myObj == null) return;

            if (string.IsNullOrEmpty(e.NewValue as string))
                return;

            try { myObj.mediaElement.Source = new Uri(e.NewValue as string); }
            catch (Exception ex) { }
        }


        public VideoPlayer()
        {
            InitializeComponent();
        }

        public VideoPlayer(string url)
        {
            InitializeComponent();
            Url = url;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }
        private void btnRewind_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }
    }
}