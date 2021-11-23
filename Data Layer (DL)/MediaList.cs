/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System;
using System.ComponentModel;

namespace DL
{
    /// <summary>
    /// Class for handling a collection of Media.
    /// </summary>
    [Serializable]
    public class MediaList : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private BindingList<Media> media;
        private string name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public MediaList(string name)
        {
            this.media = new BindingList<Media>();
            this.name = name;
        }

        /// <summary>
        /// List that contains the collection.
        /// </summary>
        public BindingList<Media> Media { get => media; set => media = value; }

        /// <summary>
        /// Gets and sets the media collection name.
        /// 'NotifyPropertyChanged' event fires when value is set to update GUI components.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Method that fires event to notify GUI components to update.
        /// </summary>
        /// <param name="propertyName">Empty string.</param>
        public void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
