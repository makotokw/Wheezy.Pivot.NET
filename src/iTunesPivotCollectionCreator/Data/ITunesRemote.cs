using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using iTunesLib;
using iTunesPivotCollectionCreator.Model;

namespace iTunesPivotCollectionCreator.Data
{
    public class ITunesRemote
    {
        private iTunesApp iTunesApp = null;
        private Thread thread = null;
        private readonly object lockObject = new object();

        public ITunesRemote()
        {
        }

        public string Version { get; protected set; }
        public bool IsConnected { get; protected set; }
        public static bool IsRunning
        {
            get
            {
                Process[] ps = Process.GetProcesses();
                foreach (Process p in ps)
                {
                    try
                    {
                        string pathName = p.MainModule.FileName;
                        if (pathName.Contains(@"iTunes\iTunes.exe"))
                        {
                            return true;
                        }
                        
                    }
                    catch { }
                }
                return false;
            }
        }

        public bool Connect()
        {
            try
            {
                connect();
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public void ConnectAsync()
        {
            if (thread != null)
            {
                thread = new Thread(new ThreadStart(connectAsync));
                thread.Start();
            }
        }

        public bool Disconnect()
        {
            lock (lockObject)
            {
                if (iTunesApp != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(iTunesApp);
                    iTunesApp = null;
                    GC.Collect();
                }
            }
            return true;
        }

        private void connect()
        {
            lock (lockObject)
            {
                this.iTunesApp = new iTunesApp();
                this.iTunesApp.OnQuittingEvent += new _IiTunesEvents_OnQuittingEventEventHandler(iTunesApp_OnQuittingEvent);
            }
            this.IsConnected = true;
        }

        private void connectAsync()
        {
            Exception error = null;
            try
            {
                connect();
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (ITunesConnected != null)
            {                
                if (this.iTunesApp != null) this.Version = this.iTunesApp.Version;
                ITunesConnectedEventArgs arg = new ITunesConnectedEventArgs(error);
                ITunesConnected(this, arg);
            }
        }

        public bool ExtractArtwork(IITTrack iitrack, string filePath)
        {
            try
            {
                if (iitrack.Artwork.Count > 0)
                {
                    var a = iitrack.Artwork[1];
                    a.SaveArtworkToFile(filePath);
                    return true;
                }
            }
            catch (Exception) { }
            return false;
        }

        protected string HashString(string Value)
        {
            var provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return System.Convert.ToBase64String(provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(Value))).Replace("/", "_");
        }

        public List<Track> GetMusicTracks(string artworkDir)
        {
            List<Track> tracks = new List<Track>();
            try
            {
                var libraryPlaylist = this.iTunesApp.LibraryPlaylist;
                foreach (IITTrack iitrack in libraryPlaylist.Tracks)
                {
                    if (iitrack.Kind != ITTrackKind.ITTrackKindFile) continue;
                    var fileTrack = iitrack as IITFileOrCDTrack;
                    if (fileTrack == null) continue;
                    if (fileTrack.VideoKind != ITVideoKind.ITVideoKindNone) continue;
                    var t = new Track
                    {
                        TrackID = iitrack.trackID,
                        Name = iitrack.Name,
                        Album = iitrack.Album,
                        Artist = iitrack.Artist,
                        BiitRate = iitrack.BitRate,
                        Composer = iitrack.Composer,
                        DateAdded = iitrack.DateAdded,
                        DiscCount = iitrack.DiscCount,
                        DiscNumber = iitrack.DiscNumber,
                        TotalTime = iitrack.Duration,
                        Genre = iitrack.Genre,
                        Kind = iitrack.KindAsString,
                        DateModified = iitrack.ModificationDate,
                        PlayCount = iitrack.PlayedCount,
                        PlayDateUTC = iitrack.PlayedDate,
                        TrackCount = iitrack.TrackCount,
                        TrackNumber = iitrack.TrackNumber,
                        Year = iitrack.Year,
                    };

                    var imagePathName = artworkDir + Path.DirectorySeparatorChar + HashString(t.Album + t.Artist + t.DiscNumber.ToString());
                    if (File.Exists(imagePathName) || ExtractArtwork(iitrack, imagePathName))
                    {
                        t.Image = imagePathName;
                    }
                    tracks.Add(t);
                }
            }
            catch (Exception) { }
            return tracks;
        }

        void iTunesApp_OnQuittingEvent()
        {
            this.IsConnected = false;
        }

        public delegate void ITunesConnectedEventHandler(object sender, ITunesConnectedEventArgs e);
        public event ITunesConnectedEventHandler ITunesConnected;
        public class ITunesConnectedEventArgs : EventArgs
        {
            public Exception Error { get; protected set; }
            public ITunesConnectedEventArgs(Exception error)
            {
                this.Error = error;
            }
            
        }
    }
}
