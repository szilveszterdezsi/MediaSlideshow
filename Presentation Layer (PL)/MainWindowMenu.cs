/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: n/a
/// ---------------------------

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Windows;

namespace PL
{
    /// <summary>
    /// Partial presentation class that handles File-menu events and I/O interaction with the user.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Checks "save status" and lets user choose "Yes, No or Cancel" if status is "unsaved".
        /// If user chooses "Yes" the method SaveCommand_Executed is fired as if user clicked "Save" in menu.
        /// If user chooses "No" or "Cancel" nothing happens.
        /// </summary>
        /// <returns>True if status is "saved" and if user chooses "Yes" or "No", otherwise false.</returns>
        private bool SaveCheck()
        {
            if (!mainController.SessionSaved() && localDirectories.Items.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Would you like to save current session?", "Save", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SaveCommand_Executed(null, null);
                    return true;
                }
                else if (result == MessageBoxResult.No)
                    return true;
                else
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Detects when "New" is clicked in the File-menu and performs a SaveCheck.
        /// If SaveCheck returns true workspace will reset and Root Dir. dialog will open.
        /// </summary>
        /// <param name="sender">Component clicked.</param>
        /// <param name="e">Routed event.</param>
        private void NewCommand_Executed(object sender, RoutedEventArgs e)
        {
            if (SaveCheck())
            {
                mainController.New();
                localFiles.Items.Clear();
                localDirectories.Items.Clear();
                RootCommand_Executed(null, null);
                miSaveAs.IsEnabled = false;
            }
        }

        /// <summary>
        /// Detects when "Open..." is clicked in the File-menu and performs a 'SaveCheck'.
        /// If 'SaveCheck' returns true a dialog to select file opens and attempt to load data from selected file is made.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void OpenCommand_Executed(object sender, RoutedEventArgs e)
        {
            if (SaveCheck())
            {
                OpenFileDialog op = new OpenFileDialog { Title = "Open", Filter = "Data files (*.dat)|*.dat" };
                if (op.ShowDialog() == true)
                {
                    try
                    {
                        mainController.Open(op.FileName);
                        LoadMediaRootDirectory(mediaRootDirectory);
                        miSaveAs.IsEnabled = true;
                        MessageBox.Show("Session successfully opened from file!", "Open", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Detects when "Save" is clicked in the File-menu and checks if game has never been saved.
        /// If 'neverSaved' returns false the method 'SaveAsCommand_Executed' is fired as if user clicked "Save Game as..." in menu.
        /// If 'neverSaved' returns true attempt to save current session to default save file is made.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Component clicked.</param>
        /// <param name="e">Routed event.</param>
        private void SaveCommand_Executed(object sender, RoutedEventArgs e)
        {
            if (mainController.NeverSaved())
                SaveAsCommand_Executed(sender, e);
            else
            {
                try
                {
                    mainController.Save();
                    MessageBox.Show("Session successfully saved to file!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Detects when "Save As..." is clicked in the File-menu.
        /// A dialog to select file opens and  attempt to save current session to selected save file is made.
        /// If attempt fails an error info message is displayed.
        /// </summary>
        /// <param name="sender">Component clicked.</param>
        /// <param name="e">Routed event.</param>
        private void SaveAsCommand_Executed(object sender, RoutedEventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog { Title = "Save As...", Filter = "Data files (*.dat)|*.dat" };
            if (op.ShowDialog() == true)
            {
                try
                {
                    mainController.SaveAs(op.FileName);
                    miSaveAs.IsEnabled = true;
                    MessageBox.Show("Session successfully saved to file!", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace, ex.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Detects when "Exit" is clicked in the File-menu and exits.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void ExitCommand_Executed(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Overrides and detects when any shutdown event is triggered and performs a 'SaveCheck'.
        /// If SaveCheck returns true game exits.
        /// If SaveCheck returns false exit is aborted.
        /// </summary>
        /// <param name="e">CancelEventArgs.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!SaveCheck())
                e.Cancel = true;
        }

        /// <summary>
        /// Detects when "Extensions" is clicked in the Settings-menu.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void ExtensionsCommand_Executed(object sender, RoutedEventArgs e)
        {
            windowExtensions = new WindowExtensionSettings(mainController);
            windowExtensions.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowExtensions.Topmost = true;
            windowExtensions.ShowDialog();
        }

        /// <summary>
        /// Detects when "Root" is clicked in the Settings-menu.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void RootCommand_Executed(object sender, RoutedEventArgs e)
        {
            windowRoot = new WindowRootDirectorySettings(mediaRootDirectory);
            windowRoot.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowRoot.Topmost = true;
            windowRoot.ShowDialog();
            if (windowRoot.ValueSet)
            {
                mediaRootDirectory = windowRoot.Path;
                LoadMediaRootDirectory(windowRoot.Path);
            }
        }

        /// <summary>
        /// Detects when "View Help" is clicked in the Help-menu.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">RoutedEventArgs.</param>
        private void ViewHelpCommand_Executed(object sender, RoutedEventArgs e)
        {
            windowViewHelp = new WindowViewHelp();
            windowViewHelp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            windowViewHelp.Topmost = true;
            windowViewHelp.ShowDialog();
        }
    }
}
