/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System;

namespace DL
{
    /// <summary>
    /// Class for handling image Media.
    /// </summary>
    [Serializable]
    public class ImageMedia : Media
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="image">Media image (icon).</param>
        /// <param name="name">Media name.</param>
        /// <param name="extension">Media extension.</param>
        /// <param name="path">Media path.</param>
        /// <param name="duration">Media duration.</param>
        public ImageMedia(string image, string name, string extension, string path, int duration)
        {
            Image = image;
            Name = name;
            Type = MediaType.Image;
            Extension = extension;
            Path = path;
            Duration = duration;
            Description = "<Description>";
        }
    }
}
