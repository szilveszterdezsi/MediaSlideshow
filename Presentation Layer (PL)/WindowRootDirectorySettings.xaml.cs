/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-20
/// Modified: n/a
/// ---------------------------

using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace PL
{
    /// <summary>
    /// Presentation dialog class used for settings I/O interaction with the user.
    /// </summary>
    public partial class WindowRootDirectorySettings : Window
    {

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public WindowRootDirectorySettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mediaRootDirectory"></param>
        public WindowRootDirectorySettings(string mediaRootDirectory)
        {
            InitializeComponent();
            tbPath.Text = mediaRootDirectory;
        }

        /// <summary>
        /// Value set flag for dialog.
        /// </summary>
        public bool ValueSet { get; set; }

        /// <summary>
        /// Sets and gets input text.
        /// </summary>
        public string Path
        {
            get { return tbPath.Text; }
            set { tbPath.Text = value; }
        }

        /// <summary>
        /// Detects when the "Browse" button is clicked and opens a "Browse Floder"-dialog.
        /// Set Path property to path of folder selected.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void Button_Browse_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    tbPath.Text = dialog.SelectedPath;
                }
            }
        }

        /// <summary>
        /// Detects when the "OK" button is clicked.
        /// Adds values to supported types List and closes itself.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            if (InputCheck())
            {
                ValueSet = true;
                Close();
            }
        }

        /// <summary>
        /// Detects when the "Canel" button is clicked.
        /// Set ValueSet property "false" then closes itself.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void Button_Cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ValueSet = false;
            Close();
        }

        /// <summary>
        /// Checks all Checkboxes and displays a message if none are checked.
        /// </summary>
        /// <returns>True if atleast one Checkbox is checked, otherwise false.</returns>
        private bool InputCheck()
        {
            string title = "Incorrect Input";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Warning;
            if (string.IsNullOrEmpty(tbPath.Text))
                System.Windows.MessageBox.Show("Please enter a path!", title, button, image);
            else if (!Directory.Exists(tbPath.Text))
                System.Windows.MessageBox.Show("Please eneter a path that exsist!", title, button, image);
            else
                return true;
            return false;
        }
    }
}
