/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: n/a
/// ---------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DL;

namespace PL
{
    /// <summary>
    /// Presentation dialog class used for settings I/O interaction with the user.
    /// </summary>
    public partial class WindowExtensionSettings : Window
    {
        private MainController mainController;
        public List<string> supportedExtensions { get { return mainController.supportedExtensions; } }
        public List<string> supportedImageExtensions { get { return mainController.supportedImageExtensions; } }
        public List<string> supportedVideoExtensions { get { return mainController.supportedVideoExtensions; } }
        public List<string> supportedAudioExtensions { get { return mainController.supportedAudioExtensions; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mainController">Controller.</param>
        public WindowExtensionSettings(MainController mainController)
        {
            InitializeComponent();
            this.mainController = mainController;
            InitializeCBGroup(spImageTypes, typeof(ImageExtension));
            InitializeCBGroup(spVideoTypes, typeof(VideoExtension));
            InitializeCBGroup(spAudioTypes, typeof(AudioExtension));
        }

        /// <summary>
        /// Dynamically populates target StackPanel with CheckBoxes based on Enum source. 
        /// </summary>
        /// <param name="sp">Target stackpanel.</param>
        /// <param name="enumType">Enum type source.</param>
        private void InitializeCBGroup(StackPanel sp, Type enumType)
        {
            foreach (object obj in Enum.GetValues(enumType))
            {
                sp.Children.Add(new CheckBox() { Content = "." + obj, IsChecked = mainController.supportedExtensions.Contains("." + obj) ? true : false });
            }
                
        }

        /// <summary>
        /// Returns List of string values of checked CheckBoxes in target Stackpanel.
        /// </summary>
        /// <param name="sp">Target stackpanel.</param>
        /// <returns>List of string values.</returns>
        private List<string> GetCheckedInCBGroup(StackPanel sp)
        {
            List<string> extensions = new List<string>();
            foreach (CheckBox cb in sp.Children)
                if (cb.IsChecked == true)
                    extensions.Add(cb.Content.ToString());
            return extensions;
        }

        /// <summary>
        /// Detects when the "OK" button is clicked.
        /// Adds values to supported types List and closes window.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (InputCheck())
            {
                supportedExtensions.Clear();
                supportedImageExtensions.Clear();
                supportedVideoExtensions.Clear();
                supportedAudioExtensions.Clear();
                supportedImageExtensions.AddRange(GetCheckedInCBGroup(spImageTypes));
                supportedVideoExtensions.AddRange(GetCheckedInCBGroup(spVideoTypes));
                supportedVideoExtensions.AddRange(GetCheckedInCBGroup(spAudioTypes));
                supportedExtensions.AddRange(supportedImageExtensions);
                supportedExtensions.AddRange(supportedVideoExtensions);
                supportedExtensions.AddRange(supportedAudioExtensions);
                Close();
            }
        }

        /// <summary>
        /// Detects when the "Cancel" button is clicked and closes window.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void Button_Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Checks all Checkboxes and displays a message if none are checked.
        /// </summary>
        /// <returns>True if atleast one Checkbox is checked, otherwise false.</returns>
        private bool InputCheck()
        {
            string title = "No Selection";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Warning;
            if (GetCheckedInCBGroup(spImageTypes).Count() + GetCheckedInCBGroup(spVideoTypes).Count() + GetCheckedInCBGroup(spAudioTypes).Count() == 0)
                MessageBox.Show("Please select at least one media type!", title, button, image);
            else
                return true;
            return false;
        }
    }
}
