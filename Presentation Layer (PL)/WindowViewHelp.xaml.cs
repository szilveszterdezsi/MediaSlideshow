/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System.Windows;

namespace PL
{
    /// <summary>
    /// Presentation dialog class used for help.
    /// </summary>
    public partial class WindowViewHelp : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public WindowViewHelp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Detects when the "Close" button is clicked and closes itself.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">EventArgs.</param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
