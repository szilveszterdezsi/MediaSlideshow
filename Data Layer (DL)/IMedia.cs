/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: -
/// ---------------------------

using System.ComponentModel;

namespace DL
{
    /// <summary>
    /// Interface for implementation by abstract class Media.
    /// </summary>
    interface IMedia : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets and sets the media image source
        /// </summary>
        string Image { get; set; }

        /// <summary>
        /// Gets and sets the media name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets and sets the media type
        /// </summary>
        MediaType Type { get; set; }

        /// <summary>
        /// Gets and sets the media extension
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// Gets and sets the media file path
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets and sets the media duration
        /// </summary>
        int Duration { get; set; }

        /// <summary>
        /// Gets and sets the media description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets and sets the save description boolean
        /// </summary>
        string SaveDescription { get; set; }

        /// <summary>
        /// Gets and sets the edit description boolean
        /// </summary>
        string EditDescription { get; set; }
    }
}
