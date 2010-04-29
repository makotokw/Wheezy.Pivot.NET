using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wheezy.Pivot;
using System.Reflection;
using System.IO;
using iTunesPivotCollectionCreator.Data;
using iTunesPivotCollectionCreator.Model;

namespace iTunesPivotCollectionCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var pivotCollection = new PivotCollection<Track>();
                pivotCollection.CollectionName = "iTunes Library";

                var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var dataDir = dir + Path.DirectorySeparatorChar + "data";
                var cacheDir = dir + Path.DirectorySeparatorChar + "cache";
                if (!Directory.Exists(cacheDir)) Directory.CreateDirectory(cacheDir);
                if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);

                //var parser = new ITunesLibraryParser();
                //parser.ParseLibrary();
                //foreach (var t in parser.Tracks)
                //{
                //    pivotCollection.Add(t);
                //}

                var itunes = new ITunesRemote();
                itunes.Connect();
                pivotCollection.AddRange(itunes.GetMusicTracks(cacheDir));
                pivotCollection.Write(dataDir + Path.DirectorySeparatorChar + @"iTunesLibrary.cxml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
