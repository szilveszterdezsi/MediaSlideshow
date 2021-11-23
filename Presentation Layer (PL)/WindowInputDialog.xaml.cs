/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-20
/// Modified: n/a
/// ---------------------------

using System.Windows;

namespace PL
{
    /// <summary>
    /// Presentation dialog class used for generic I/O interaction with the user.
    /// </summary>
    public partial class WindowInputDialog : Window
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public WindowInputDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowTitle"></param>
        /// <param name="gbHeader"></param>
        /// <param name="inputText"></param>
        /// <param name="okText"></param>
        public WindowInputDialog(string windowTitle, string gbHeader, string inputText, string okText)
        {
            InitializeComponent();
            Title = windowTitle;
            this.gbHeader.Header = gbHeader;
            this.inputText.Text = inputText;
            this.okText.Content = okText;
        }

        /// <summary>
        /// Value set flag for dialog.
        /// </summary>
        public bool ValueSet { get; set; }

        /// <summary>
        /// Sets and gets input text.
        /// </summary>
        public string InputText
        {
            get { return inputText.Text; }
            set { inputText.Text = value; }
        }

        /// <summary>
        /// Detects when the "OK" button is clicked.
        /// Set ValueSet property "true" if "InputCheck" passes then closes itself.
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
        /// Checks if input is not empty.
        /// </summary>
        /// <returns>True if not empty, otherwise false.</returns>
        private bool InputCheck()
        {
            string title = "Incorrect Input";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage image = MessageBoxImage.Warning;
            if (string.IsNullOrEmpty(inputText.Text))
                MessageBox.Show("Please enter a name!", title, button, image);
            else
                return true;
            return false;

        }
    }
}
