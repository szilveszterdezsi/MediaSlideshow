/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System;

namespace DL
{
    /// <summary>
    /// Class for handling video Media.
    /// </summary>
    [Serializable]
    public class VideoMedia : Media
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Media name.</param>
        /// <param name="extension">Media extension.</param>
        /// <param name="path">Media path.</param>
        public VideoMedia(string name, string extension, string path)
        {
            Image = "pack://application:,,,/images/video.png";
            Name = name;
            Type = MediaType.Video;
            Extension = extension;
            Path = path;
            EditDuration = "Collapsed";
            Description = "<Description>";
        }
    }
}
