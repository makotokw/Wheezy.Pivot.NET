using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using iTunesPivotCollectionCreator.Model;

namespace iTunesPivotCollectionCreator.Data
{
    public class ITunesLibraryParser
    {
        public readonly string LibraryXmlPathName = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\iTunes\iTunes Music Library.xml";

        public List<Track> Tracks = new List<Track>();

        public ITunesLibraryParser()
        {
        }

        public void ParseLibrary()
        {
            this.ParseLibrary(LibraryXmlPathName);
        }

        public void ParseLibrary(string pathName)
        {
            this.Tracks.Clear();

            var navigator = new XPathDocument(pathName).CreateNavigator();
            var iterator = navigator.Select("/plist/dict/dict/dict");
            while (iterator.MoveNext())
            {
                var track = this.parseTrack(iterator.Current.SelectChildren(XPathNodeType.All));
                if (track != null)
                {
                    this.Tracks.Add(track);
                }
            }
        }

        protected Track parseTrack(XPathNodeIterator nodeIterator)
        {
            var track = new Track();
            while (nodeIterator.MoveNext())
            {
                string key = nodeIterator.Current.Value;
                if (key.Equals("Track ID", StringComparison.OrdinalIgnoreCase))
                {
                    if (nodeIterator.MoveNext())
                    {
                        track.TrackID = int.Parse(nodeIterator.Current.Value);
                    }
                }
                else
                {
                    if (key.Equals("Name", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            track.Name = nodeIterator.Current.Value;
                        }
                        continue;
                    }
                    if (key.Equals("Artist", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            track.Artist = nodeIterator.Current.Value;
                        }
                        continue;
                    }
                    if (key.Equals("Album Artist", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            track.AlbumArtist = nodeIterator.Current.Value;
                        }
                        continue;
                    }
                    if (key.Equals("Album", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            track.Album = nodeIterator.Current.Value;
                        }
                        continue;
                    }
                    if (key.Equals("Genre", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            track.Genre = nodeIterator.Current.Value;
                        }
                        continue;
                    }
                    if (key.Equals("Total Time", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            try
                            {
                                track.TotalTime = int.Parse(nodeIterator.Current.Value);
                            }
                            catch
                            {
                            }
                        }
                        continue;
                    }
                    if (key.Equals("Track Number", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            try
                            {
                                track.TrackNumber = int.Parse(nodeIterator.Current.Value);
                            }
                            catch
                            {
                            }
                        }
                        continue;
                    }
                    if (key.Equals("Year", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            try
                            {
                                track.Year = int.Parse(nodeIterator.Current.Value);
                            }
                            catch
                            {
                            }
                        }
                        continue;
                    }
                    if (key.Equals("Play Count", StringComparison.OrdinalIgnoreCase))
                    {
                        if (nodeIterator.MoveNext())
                        {
                            try
                            {
                                track.PlayCount = int.Parse(nodeIterator.Current.Value);
                            }
                            catch
                            {
                            }
                        }
                        continue;
                    }
                }
            }
            return (track.TrackID != -1) ? track : null;
        }
    }
}