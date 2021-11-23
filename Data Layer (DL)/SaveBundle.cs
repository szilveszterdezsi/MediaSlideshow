/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DL
{
    /// <summary>
    /// Class for bundling workspace to be saved/loaded to/from file (serialization).
    /// </summary>
    [Serializable]
    public class SaveBundle
    {
        private string mediaRootDirectory;
        private List<string> supportedExtensions;
        private List<string> supportedImageExtensions;
        private List<string> supportedVideoExtensions;
        private List<string> supportedAudioExtensions;
        private BindingList<MediaList> albumList;
        private BindingList<MediaList> slideshowList;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="mediaRootDirectory">Media Root Directory.</param>
        /// <param name="supportedExtensions">Supported Extensions (All).</param>
        /// <param name="supportedImageExtensions">Supported Image Extensions.</param>
        /// <param name="supportedVideoExtensions">Supported Video Extensions.</param>
        /// <param name="supportedAudioExtensions">Supported Audio Extensions.</param>
        /// <param name="albumList">Album List.</param>
        /// <param name="slideshowList">Slideshow List.</param>
        public SaveBundle(string mediaRootDirectory, List<string> supportedExtensions, List<string> supportedImageExtensions, List<string> supportedVideoExtensions, List<string> supportedAudioExtensions, BindingList<MediaList> albumList, BindingList<MediaList> slideshowList)
        {
            this.mediaRootDirectory = mediaRootDirectory;
            this.supportedExtensions = supportedExtensions;
            this.supportedImageExtensions = supportedImageExtensions;
            this.supportedVideoExtensions = supportedVideoExtensions;
            this.supportedAudioExtensions = supportedAudioExtensions;
            this.albumList = albumList;
            this.slideshowList = slideshowList;
        }

        /// <summary>
        /// Gets and sets the Media Root Directory.
        /// </summary>
        public string MediaRootDirectory { get => mediaRootDirectory; set => mediaRootDirectory = value; }

        /// <summary>
        /// Gets and sets the Supported Extensions (All).
        /// </summary>
        public List<string> SupportedExtensions { get => supportedExtensions; set => supportedExtensions = value; }

        /// <summary>
        /// Gets and sets the Supported Image Extensions.
        /// </summary>
        public List<string> SupportedImageExtensions { get => supportedImageExtensions; set => supportedImageExtensions = value; }

        /// <summary>
        /// Gets and sets the Supported Video Extensions.
        /// </summary>
        public List<string> SupportedVideoExtensions { get => supportedVideoExtensions; set => supportedVideoExtensions = value; }

        /// <summary>
        /// Gets and sets the Supported Audio Extensions.
        /// </summary>
        public List<string> SupportedAudioExtensions { get => supportedAudioExtensions; set => supportedAudioExtensions = value; }

        /// <summary>
        /// Gets and sets the Album List.
        /// </summary>
        public BindingList<MediaList> AlbumList { get => albumList; set => albumList = value; }

        /// <summary>
        /// Gets and sets the Slideshow List.
        /// </summary>
        public BindingList<MediaList> SlideshowList { get => slideshowList; set => slideshowList = value; }
    }
}
