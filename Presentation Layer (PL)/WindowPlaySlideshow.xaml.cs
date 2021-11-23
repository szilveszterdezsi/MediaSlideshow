/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-10-11
/// Modified: n/a
/// ---------------------------

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DL;

namespace PL
{
    /// <summary>
    /// Presentation dialog (full screen) class used for playing a Slideshow.
    /// </summary>
    public partial class WindowPlaySlideshow : Window
    {
        private BindingList<Media> content;
        private DispatcherTimer timer;
        private int index;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="content">List of Media to run in Slideshow.</param>
        public WindowPlaySlideshow(BindingList<Media> content)
        {
            InitializeComponent();
            this.content = content;
            index = 0;
            timer = new DispatcherTimer();
            timer.Tick += ImageMediaEnded;
        }

        /// <summary>
        /// Starts the slideshow as well as skips to the next media item.
        /// </summary>
        public void NextSlide()
        {
            if (content != null && index < content.Count && content.Count > 0)
            {
                Media item = content[index++];
                if (item.Type == MediaType.Image)
                {
                    meAudioVideoMedia.Visibility = Visibility.Hidden;
                    iPhotoMedia.Visibility = Visibility.Visible;
                    iPhotoMedia.Source = new BitmapImage(new Uri(item.Path));
                    timer.Interval = TimeSpan.FromSeconds(item.Duration);
                    timer.Start();
                }
                else if (item.Type == MediaType.Video || item.Type == MediaType.Audio)
                {
                    iPhotoMedia.Visibility = Visibility.Hidden;
                    meAudioVideoMedia.Visibility = Visibility.Visible;
                    meAudioVideoMedia.Source = new Uri(item.Path);
                    meAudioVideoMedia.Play();
                }
                else
                {
                    // do nothing
                }
            }
            else
            {
                Close(); 
            }

        }

        /// <summary>
        /// Detects when the ImageMedia timer runs out and skips to the next media item.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void ImageMediaEnded(object sender, EventArgs e)
        {
            timer.Stop();
            NextSlide();
        }

        /// <summary>
        /// Detects when a VideoMedia or AudioMedia item has ended and skips to the next media item.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void AudioVideoMedia_MediaEnded(object sender, RoutedEventArgs e)
        {
            NextSlide();
        }

        /// <summary>
        /// Detects when the "Esc"-key is pressed and closes the slideshow.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">ExecutedRoutedEventArgs.</param>
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Detects when the "Space"-key is pressand skips to the next media item.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">ExecutedRoutedEventArgs.</param>
        private void SpaceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (timer.IsEnabled)
                timer.Stop();
            NextSlide();
        }
    }
}
