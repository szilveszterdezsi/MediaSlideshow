/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-20
/// Modified: n/a
/// ---------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using BLL;
using DL;

namespace PL
{
    /// <summary>
    /// Partial presentation class that initializes the GUI components and refreshes, sets and gets status of components.
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainController mainController;
        private WindowExtensionSettings windowExtensions;
        private WindowRootDirectorySettings windowRoot;
        private WindowInputDialog windowInput;
        private WindowViewHelp windowViewHelp;
        private WindowPlaySlideshow windowPlaySlideshow;
        public string mediaRootDirectory { get { return mainController.mediaRootDirectory; } set { mainController.mediaRootDirectory = value; } }
        public List<string> supportedExtensions { get { return mainController.supportedExtensions; } }
        public List<string> supportedImageExtensions { get { return mainController.supportedImageExtensions; } }
        public List<string> supportedVideoExtensions { get { return mainController.supportedVideoExtensions; } }
        public List<string> supportedAudioExtensions { get { return mainController.supportedAudioExtensions; } }
        public BindingList<MediaList> albumList { get { return mainController.albumList; } }
        public BindingList<MediaList> slideshowList { get { return mainController.slideshowList; } }

        /// <summary>
        /// Constrctor that initializes the GUI componenets and controller.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            mainController = new MainController();
            InitializeGUI();
        }

        /// <summary>
        /// Initializes the GUI
        /// </summary>
        private void InitializeGUI()
        {
            //LoadLocalDrives();
            LoadMediaRootDirectory(mediaRootDirectory);
        }

        /// <summary>
        /// Detects when text is edited and automatically resizes column width to match width.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">TextChangedEventArgs.</param>
        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Name.Equals("DigitOnly"))
            {
                string txt = tb.Text;
                if (txt != "")
                {
                    tb.Text = Regex.Replace(tb.Text, "[^0-9]", "");
                }
            }
            AutoResizeColumns();
        }

        /// <summary>
        /// Forces ListView to automatically resize column widths to fit updated content.
        /// </summary>
        private void AutoResizeColumns()
        {
            GridView gridView = playList.View as GridView;
            if (gridView != null)
            {
                foreach (GridViewColumn gridViewColumn in gridView.Columns)
                {
                    if (double.IsNaN(gridViewColumn.Width))
                    {
                        gridViewColumn.Width = gridViewColumn.ActualWidth;
                    }
                    gridViewColumn.Width = double.NaN;
                }
            }
        }
    }
}
