/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DL;

namespace PL
{
    /// <summary>
    /// Partial presentation class that handles ListView related events and I/O interaction with the user.
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;

        /// <summary>
        /// Detects when left mouse button is down on ListView and saves the position.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">MouseButtonEventArgs.</param>
        private void ListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        /// <summary>
        /// Detects and initiates ListViewItem drag.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">MouseEventArgs.</param>
        private void ListView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    ListView listView = sender as ListView;
                    ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                    if (listViewItem == null)
                        return;
                    object item = (object)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);
                    DataObject data = new DataObject("item", item);
                    DragDropEffects de = DragDrop.DoDragDrop(sender as ListView, data, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Detects and initiates ListViewItem drop on "Album Files".
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">DragEventArgs.</param>
        private void AlbumFiles_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("item"))
            {
                Media item = e.Data.GetData("item") as Media;
                if (!albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media.Contains(item))
                {
                    albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media.Add(item);
                }
                albumFiles.ItemsSource = albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media;
            }
        }

        /// <summary>
        /// Detects and initiates ListViewItem drop on "Playlist".
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">DragEventArgs.</param>
        private void PlayList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("item"))
            {
                Media item = e.Data.GetData("item") as Media;
                ListViewItem listViewItem = FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    Media nameToReplace = (Media)(sender as ListView).ItemContainerGenerator.ItemFromContainer(listViewItem);
                    int index = (sender as ListView).Items.IndexOf(nameToReplace);

                    if (index >= 0)
                    {
                        if (slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Contains(item))
                        {
                            slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Remove(item);
                        }
                        slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Insert(index, item);
                    }
                }
                else
                {
                    slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Remove(item);
                    slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Add(item);
                }
                playList.ItemsSource = slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media;
            }
        }

        /// <summary>
        /// Detects when ListViewItem drag eneters a ListView and displays corresponding effect at mouse.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">DragEventArgs.</param>
        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("item"))
            {
                e.Effects = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Runs up the ListView VisualTree to find ListViewItem (DependencyObject) under dragged ListViewItem.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="current"></param>
        /// <returns></returns>
        private static T FindAnchestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        /// <summary>
        /// Fires when one or more media file(s) are selected and clicked for deletion.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void MediaFileDelete_Click(object sender, RoutedEventArgs e)
        {
            if (albumFiles.SelectedIndex == -1)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete selected file(s)?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (albumFiles.SelectedItems.Count > 1)
                {
                    Media[] items = new Media[albumFiles.SelectedItems.Count];
                    albumFiles.SelectedItems.CopyTo(items, 0);
                    foreach (Media item in items)
                        albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media.Remove(item);
                }
                else
                    albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media.RemoveAt(albumFiles.SelectedIndex);
                albumFiles.ItemsSource = albumList[albumTree.Items.IndexOf(albumTree.SelectedItem)].Media;
            }
            
        }

        /// <summary>
        /// Fires when one or more playlist item(s) are selected and clicked for deletion.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void PlaylistItemDelete_Click(object sender, RoutedEventArgs e)
        {
            if (playList.SelectedIndex == -1)
            {
                return;
            }
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete selected items(s)?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (playList.SelectedItems.Count > 1)
                {
                    Media[] items = new Media[playList.SelectedItems.Count];
                    playList.SelectedItems.CopyTo(items, 0);
                    foreach (Media item in items)
                        slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.Remove(item);
                }
                else
                    slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media.RemoveAt(playList.SelectedIndex);
                playList.ItemsSource = slideshowList[slideshowTree.Items.IndexOf(slideshowTree.SelectedItem)].Media;
            }
        }

        /// <summary>
        /// Detects when name "Save Description"-button is clicked and references underlaying ListViewItem to enable saving of description.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void SaveDescription_Click(object sender, RoutedEventArgs e)
        {
            ((Media)((FrameworkElement)sender).DataContext).EditDescription = "Visible";
            playList.PreviewMouseMove += ListView_PreviewMouseMove;
            playList.PreviewMouseLeftButtonDown += ListView_MouseLeftButtonDown;
        }

        /// <summary>
        /// Detects when name "Edit Description"-button is clicked and references underlaying ListViewItem to enable editing of description.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void EditDescription_Click(object sender, RoutedEventArgs e)
        {
            ((Media)((FrameworkElement)sender).DataContext).SaveDescription = "Visible";
            playList.PreviewMouseMove -= ListView_PreviewMouseMove;
            playList.PreviewMouseLeftButtonDown -= ListView_MouseLeftButtonDown;
        }


        /// <summary>
        /// Detects when name "Save Duration"-button is clicked and references underlaying ListViewItem to enable saving of duration.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void SaveDuration_Click(object sender, RoutedEventArgs e)
        {
            ((Media)((FrameworkElement)sender).DataContext).EditDuration = "Visible";
            playList.PreviewMouseMove += ListView_PreviewMouseMove;
            playList.PreviewMouseLeftButtonDown += ListView_MouseLeftButtonDown;
        }

        /// <summary>
        /// Detects when name "Edit Description"-button is clicked and references underlaying ListViewItem to enable editing of description.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void EditDuration_Click(object sender, RoutedEventArgs e)
        {
            ((Media)((FrameworkElement)sender).DataContext).SaveDuration = "Visible";
            playList.PreviewMouseMove -= ListView_PreviewMouseMove;
            playList.PreviewMouseLeftButtonDown -= ListView_MouseLeftButtonDown;
        }

        /// <summary>
        /// Detects when a "file" is clicked or double clicked to be opened and opens it with native application.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(((Media)((FrameworkElement)sender).DataContext).Path);
        }
    }
}
