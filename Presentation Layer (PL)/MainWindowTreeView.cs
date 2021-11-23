/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: n/a
/// ---------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DL;

namespace PL
{
    /// <summary>
    /// Partial presentation class that handles TreeView related events and I/O interaction with the user.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// (Lazy)loads local directories based on path. 
        /// </summary>
        /// <param name="path">Root directory path.</param>
        public void LoadMediaRootDirectory(string path)
        {
            localDirectories.Items.Clear();
            DirectoryInfo root = new DirectoryInfo(path);
            localDirectories.Items.Add(CreateTreeItem(root));
        }

        /// <summary>
        /// (Lazy)loads local drives. 
        /// </summary>
        public void LoadLocalDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
            {
                localDirectories.Items.Add(CreateTreeItem(driveInfo));
            }

        }

        /// <summary>
        /// Creates TreeViewItem from DirectoryInfo/DriveInfo.
        /// </summary>
        /// <param name="o">DirectoryInfo/DriveInfo object.</param>
        /// <returns>TreeViewItem created.</returns>
        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            if (o is DriveInfo)
            {
                item.Header = (o as DriveInfo).Name;
                item.Items.Add("Loading...");
            }
            if (o is DirectoryInfo)
            {
                item.Header = (o as DirectoryInfo).Name;
                if (DirectoryReadableAndNotEmpty(o as DirectoryInfo))
                    item.Items.Add("Loading...");
            }
            item.Tag = o;
            return item;
        }

        /// <summary>
        /// Checks if a directory is readable and/or empty.
        /// </summary>
        /// <param name="dirInfo">Directory path.</param>
        /// <returns>True if reabale and/or not empty.</returns>
        private bool DirectoryReadableAndNotEmpty(DirectoryInfo dirInfo)
        {
            try
            {
                if (dirInfo.GetDirectories().Count() > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Detects when a "Local Directory" is selected and loads supported files types in to "Local Files"
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void LocalDirectoriesItem_Selected(object sender, RoutedEventArgs e)
        {
            localFiles.Items.Clear();
            TreeViewItem item = e.Source as TreeViewItem;
            DirectoryInfo expandedDir = null;
            if (item.Tag is DriveInfo)
                expandedDir = (item.Tag as DriveInfo).RootDirectory;
            if (item.Tag is DirectoryInfo)
                expandedDir = item.Tag as DirectoryInfo;
            DirectoryInfo dir = new DirectoryInfo(expandedDir.FullName);
            FileInfo[] files = dir.GetFiles().Where(f => supportedExtensions.Contains(f.Extension.ToLower())).ToArray();
            foreach (FileInfo file in files)
            {
                if (supportedImageExtensions.Contains(file.Extension.ToLower()))
                {
                    localFiles.Items.Add(new ImageMedia(file.FullName, file.Name, file.Extension, file.FullName, 3));
                }
                else if (supportedVideoExtensions.Contains(file.Extension.ToLower()))
                {
                    localFiles.Items.Add(new VideoMedia(file.Name, file.Extension, file.FullName));
                }
                else if (supportedAudioExtensions.Contains(file.Extension.ToLower()))
                {
                    localFiles.Items.Add(new AudioMedia(file.Name, file.Extension, file.FullName));
                }

            }
        }

        /// <summary>
        /// Detects when a TreeViewItem is expanded in "Directories" and loads each subdirectory.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        public void LocalDirectoriesItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();
                DirectoryInfo expandedDir = null;
                if (item.Tag is DriveInfo)
                    expandedDir = (item.Tag as DriveInfo).RootDirectory;
                if (item.Tag is DirectoryInfo)
                    expandedDir = item.Tag as DirectoryInfo;
                try
                {
                    foreach (DirectoryInfo subDir in expandedDir.GetDirectories())
                        item.Items.Add(CreateTreeItem(subDir));
                }
                catch { }
            }
        }

        /// <summary>
        /// Detects when "New Album" is clicked and opens an input dialog to enter a name.
        /// If value of dialog is set a new album is created with name entered.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void NewAlbum_Click(object sender, RoutedEventArgs e)
        {
            windowInput = new WindowInputDialog("New Album", "Please Enter Name", "", "Apply");
            windowInput.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowInput.Topmost = true;
            windowInput.ShowDialog();
            if (windowInput.ValueSet)
                albumList.Add(new MediaList(windowInput.InputText));
        }

        /// <summary>
        /// Detects when "Delete Album" is clicked and opens a confirmation dialog.
        /// If answer is "Yes", album is deleted, otherwise not.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void DeleteAlbum_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete selected album?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                albumList.Remove((MediaList)((FrameworkElement)sender).DataContext);
            }
        }

        /// <summary>
        /// Detects when "Rename Album" is clicked and opens rename dialog.
        /// If value of dialog is set album is renamed with name entered.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void RenameAlbum_Click(object sender, RoutedEventArgs e)
        {
            MediaList album = (MediaList)((FrameworkElement)sender).DataContext;
            if (album == null)
                return;
            windowInput = new WindowInputDialog("Rename Album", "Current Album Name", album.Name, "Rename");
            windowInput.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowInput.Topmost = true;
            windowInput.ShowDialog();
            if (windowInput.ValueSet)
                album.Name = windowInput.InputText; 
        }

        /// <summary>
        /// Generated a new slideshow based on contents of selected album.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void GenerateSlideshow_Click(object sender, RoutedEventArgs e)
        {
            MediaList album = (MediaList)((FrameworkElement)sender).DataContext;
            MediaList slideshow = new MediaList(album.Name + " Slideshow");
            foreach (Media m in album.Media)
                slideshow.Media.Add(m);
            slideshowList.Add(slideshow);
        }

        /// <summary>
        /// Detects when an "Album" is selected and loads attached media in to "Media Files"
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void AlbumTree_ItemSelected(object sender, RoutedEventArgs e)
        {
            albumTree.Tag = e.OriginalSource;
            albumFiles.ItemsSource = albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media;
        }

        /// <summary>
        /// Used to detect enable/diable of "Media Files"-ListView.
        /// Disabled when no albums exist (selected) and only enabled when one or more albums have been created.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void AlbumTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (albumTree.SelectedItem != null)
            {
                albumFiles.IsEnabled = true;
            }
            else
            {
                albumFiles.IsEnabled = false;
                albumFiles.ItemsSource = new List<Media>();
            }
        }

        /// <summary>
        /// Detects when "New Slideshow" is clicked and opens an input dialog to enter a name.
        /// If value of dialog is set a new slideshow is created with name entered.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void NewSlideshow_Click(object sender, RoutedEventArgs e)
        {
            windowInput = new WindowInputDialog("New Slideshow", "Please Enter Name", "", "Apply");
            windowInput.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowInput.Topmost = true;
            windowInput.ShowDialog();
            if (windowInput.ValueSet)
                slideshowList.Add(new MediaList(windowInput.InputText));
        }

        /// <summary>
        /// Detects when "Play Slideshow" is clicked and plays selected slideshow (if not empty).
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void RunSlideshow_Click(object sender, RoutedEventArgs e)
        {
            if (slideshowTree.SelectedItem != null && slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Count() > 0)
            {
                windowPlaySlideshow = new WindowPlaySlideshow(slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media);
                windowPlaySlideshow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                windowPlaySlideshow.Topmost = true;
                windowPlaySlideshow.NextSlide();
                windowPlaySlideshow.ShowDialog();
            }
        }

        /// <summary>
        /// Detects when "Delete Slideshow" is clicked and opens a confirmation dialog.
        /// If answer is "Yes", slideshow is deleted, otherwise not.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void DeleteSlideshow_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete selected slideshow?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                slideshowList.Remove((MediaList)((FrameworkElement)sender).DataContext);
            }
        }

        /// <summary>
        /// Detects when "Rename Slideshow" is clicked and opens rename dialog.
        /// If value of dialog is set slideshow is renamed with name entered.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void RenameSlideshow_Click(object sender, RoutedEventArgs e)
        {
            MediaList slideshow = ((MediaList)((FrameworkElement)sender).DataContext);
            if (slideshow == null)
                return;
            windowInput = new WindowInputDialog("Rename Slideshow", "Current Slideshow Name", slideshow.Name, "Rename");
            windowInput.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowInput.Topmost = true;
            windowInput.ShowDialog();
            if (windowInput.ValueSet)
                slideshow.Name = windowInput.InputText;
        }

        /// <summary>
        /// Detects when a "Slideshow" is selected and loads attached items in to "Playlist"
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void SlideshowTree_ItemSelected(object sender, RoutedEventArgs e)
        {
            slideshowTree.Tag = e.OriginalSource;
            playList.ItemsSource = slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media;
        }

        /// <summary>
        /// Used to detect enable/diable of "Playlist-ListView.
        /// Disabled when no slidehsows exist (selected) and only enabled when one or more slideshows have been created.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void SlideshowTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (slideshowTree.SelectedItem != null)
            {
                playList.IsEnabled = true;
            }
            else
            {
                playList.IsEnabled = false;
                playList.ItemsSource = new List<Media>();
            }
        }
    }
}
