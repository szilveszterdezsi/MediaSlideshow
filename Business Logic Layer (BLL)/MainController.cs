/// ---------------------------
/// Author: Szilveszter Dezsi
/// Created: 2019-09-14
/// Modified: -
/// ---------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using DL;
using DAL;

namespace BLL
{
    /// <summary>
    /// Controller class of the Slideshow Manager that serves the presentation layer.
    /// </summary>
    public class MainController
    {
        public string mediaRootDirectory = @"..\\..\\..\\TestFiles";
        public List<string> supportedExtensions = new List<string>();
        public List<string> supportedImageExtensions = new List<string>();
        public List<string> supportedVideoExtensions = new List<string>();
        public List<string> supportedAudioExtensions = new List<string>();
        public BindingList<MediaList> albumList = new BindingList<MediaList>();
        public BindingList<MediaList> slideshowList = new BindingList<MediaList>();
        bool sessionSaved = false;
        bool neverSaved = true;
        string defaultFilePath = "Default.dat";

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public MainController()
        {
            InitializeSupportedExtensions();
        }

        /// <summary>
        /// Initializes the supported file type extensions.
        /// </summary>
        private void InitializeSupportedExtensions()
        {
            supportedExtensions.Clear();
            supportedExtensions.AddRange(supportedImageExtensions = GetEnumsAsStringList(typeof(ImageExtension)));
            supportedExtensions.AddRange(supportedVideoExtensions = GetEnumsAsStringList(typeof(VideoExtension)));
            supportedExtensions.AddRange(supportedAudioExtensions = GetEnumsAsStringList(typeof(AudioExtension)));
        }

        /// <summary>
        /// Returns an Enum-type as a List of strings.
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        private List<string> GetEnumsAsStringList(Type enumType)
        {
            List<string> extensions = new List<string>();
            foreach (object obj in Enum.GetValues(enumType))
                extensions.Add("." + obj);
            return extensions;
        }

        /// <summary>
        /// Gets save status about the current session.
        /// </summary>
        /// <returns>True if session is saved, false if unsaved.</returns>
        public bool SessionSaved()
        {
            return sessionSaved;
        }

        /// <summary>
        /// Gets status on whether current session has never been saved.
        /// </summary>
        /// <returns>True if session has never been saved, otherwise false.</returns>
        public bool NeverSaved()
        {
            return neverSaved;
        }

        /// <summary>
        /// Inizializes a new empty session by reinitalizing support extensions and clearing lists.
        /// </summary>
        public void New()
        {
            InitializeSupportedExtensions();
            albumList.Clear();
            slideshowList.Clear();
            neverSaved = true;
            sessionSaved = false;
        }

        /// <summary>
        /// Inizializes a new session by loading selected file into workspace.
        /// </summary>
        /// <param name="filePath">Path of the selected file.</param>
        public void Open(string filePath)
        {
            neverSaved = false;
            sessionSaved = true;
            defaultFilePath = filePath;
            SaveBundle save = Serialization.BinaryDeserializeFromFile<SaveBundle>(filePath);
            mediaRootDirectory = save.MediaRootDirectory;
            supportedExtensions = save.SupportedExtensions;
            supportedImageExtensions = save.SupportedImageExtensions;
            supportedVideoExtensions = save.SupportedVideoExtensions;
            supportedAudioExtensions = save.SupportedAudioExtensions;
            albumList.Clear();
            foreach (MediaList ml in save.AlbumList)
                albumList.Add(ml);
            slideshowList.Clear();
            foreach (MediaList pl in save.SlideshowList)
                slideshowList.Add(pl);
        }

        /// <summary>
        /// Saves the currect session to the default file.
        /// </summary>
        public void Save()
        {
            SaveAs(defaultFilePath);
            neverSaved = false;
            sessionSaved = true;
        }

        /// <summary>
        /// Saves the currect session to the selected file.
        /// </summary>
        /// <param name="filePath">Path of the selected file.</param>
        public void SaveAs(string filePath)
        {
            neverSaved = false;
            sessionSaved = true;
            defaultFilePath = filePath;
            SaveBundle save = new SaveBundle(   mediaRootDirectory,
                                                supportedExtensions,
                                                supportedImageExtensions,
                                                supportedVideoExtensions,
                                                supportedAudioExtensions, 
                                                albumList, 
                                                slideshowList   );
            Serialization.BinarySerializeToFile(save, filePath);
        }
    }
}
