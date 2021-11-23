/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-27
/// Modified: n/a
/// ---------------------------

using System;

namespace DL
{
    /// <summary>
    /// Class for handling audio Media.
    /// </summary>
    [Serializable]
    public class AudioMedia : Media
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Media name.</param>
        /// <param name="extension">Media extension.</param>
        /// <param name="path">Media path.</param>
        public AudioMedia(string name, string extension, string path)
        {
            Image = "pack://application:,,,/images/audio.png";
            Name = name;
            Type = MediaType.Audio;
            Extension = extension;
            Path = path;
            EditDuration = "Collapsed";
            Description = "<Description>";
        }
    }
}
