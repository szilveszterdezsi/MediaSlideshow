/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: -
/// ---------------------------

using System;
using System.ComponentModel;

namespace DL
{
    /// <summary>
    /// Abstract class for handling media.
    /// </summary>
    [Serializable]
    public abstract class Media : IMedia
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private string image;
        private string name;
        private MediaType type;
        private string extension;
        private string path;
        private int duration;
        private string description;
        private string saveDescription = "Collapsed";
        private string editDescription = "Visible";
        private string saveDuration = "Collapsed";
        private string editDuration = "Visible";

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Media()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="extension"></param>
        /// <param name="path"></param>
        /// <param name="duration"></param>
        /// <param name="description"></param>
        public Media(string image, string name, MediaType type, string extension, string path, int duration, string description)
        {
            this.image = image;
            this.name = name;
            this.type = type;
            this.extension = extension;
            this.path = path;
            this.duration = duration;
            this.description = description;
        }

        /// <summary>
        /// Gets and sets the image source
        /// </summary>
        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// Gets and sets the name
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets and sets the type
        /// </summary>
        public MediaType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets and sets the extension
        /// </summary>
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        /// <summary>
        /// Gets and sets the path
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        /// <summary>
        /// Gets and sets the duration
        /// </summary>
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// Gets and sets the description
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets and sets the save description boolean.
        /// Used to enable/diable save description mode in GUI.
        /// 'NotifyPropertyChanged' event fires when value is set to update GUI components.
        /// </summary>
        public string SaveDescription
        {
            get { return saveDescription; }
            set
            {
                if (saveDescription != value)
                {
                    EditDescription = "Collapsed";
                    saveDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the edit duration boolean.
        /// Used to enable/diable edit duration mode in GUI.
        /// 'NotifyPropertyChanged' event fires when value is set to update GUI components.
        /// </summary>
        public string EditDescription
        {
            get { return editDescription; }
            set
            {
                if (editDescription != value)
                {
                    SaveDescription = "Collapsed";
                    editDescription = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string SaveDuration
        {
            get { return saveDuration; }
            set
            {
                if (saveDuration != value)
                {
                    EditDuration = "Collapsed";
                    saveDuration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets and sets the edit duration boolean.
        /// Used to enable/diable edit duration mode in GUI.
        /// 'NotifyPropertyChanged' event fires when value is set to update GUI components.
        /// </summary>
        public string EditDuration
        {
            get { return editDuration; }
            set
            {
                if (editDuration != value)
                {
                    SaveDuration = "Collapsed";
                    editDuration = value;
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

        /// <summary>
        /// Return the string representation of the Media
        /// </summary>
        /// <returns>String representation of the Media</returns>
        public override string ToString()
        {
            return "File name: " + name + "\nType: " + type.ToString() + "\nPath: " + path;
        }
    }
}
