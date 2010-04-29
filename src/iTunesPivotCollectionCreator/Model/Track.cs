using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wheezy.Pivot;

namespace iTunesPivotCollectionCreator.Model
{
    public class Track
    {
        [PivotItem(IsName = true)]
        public string Name { get; set; }

        [PivotItem(IsDescription = true)]
        public string Description { get; set; }
        
        [PivotItem(IsImage = true)]
        public string Image { get; set; }

        public int TrackID { get; set; }

        [PivotItem(FacetDisplayName = "Artist", FacetType = FacetTypes.String, IsFacet = true, IsCollection = false)]
        public string Artist { get; set; }

        [PivotItem(FacetDisplayName = "Album Artist", FacetType = FacetTypes.String, IsFacet = true, IsCollection = false)]
        public string AlbumArtist { get; set; }

        [PivotItem(FacetDisplayName = "Composer", FacetType = FacetTypes.String, IsFacet = true, IsCollection = false)]
        public string Composer { get; set; }

        [PivotItem(FacetDisplayName = "Album", FacetType = FacetTypes.String, IsFacet = true, IsCollection = false)]
        public string Album { get; set; }

        [PivotItem(FacetDisplayName = "Genre", FacetType = FacetTypes.String, IsFacet = true, IsCollection = false)]
        public string Genre { get; set; }

        public string Kind { get; set; }
        public int Size { get; set; }

        [PivotItem(FacetDisplayName = "TotalTime", FacetType = FacetTypes.Number, IsFacet = true, IsCollection = false)]
        public int TotalTime { get; set; }

        public int DiscNumber { get; set; }
        public int DiscCount { get; set; }
        public int TrackNumber { get; set; }
        public int TrackCount { get; set; }

        [PivotItem(FacetDisplayName = "Year", FacetType = FacetTypes.Number, IsFacet = true, IsCollection = false)]
        public int Year { get; set; }        

        public DateTime DateModified { get; set; }
        public DateTime DateAdded { get; set; }
        public int BiitRate { get; set; }
        public int SampleRate { get; set; }

        [PivotItem(FacetDisplayName = "PlayCount", FacetType = FacetTypes.Number, IsFacet = true, IsCollection = false)]
        public int PlayCount { get; set; }

        public int PlayDate { get; set; }
        public DateTime PlayDateUTC { get; set; }
        public int Rating { get; set; }
        public int ArtworkCount { get; set; }
        public int PersistentID { get; set; }
        public string TrackType { get; set; }
        public string Location { get; set; }
        public int FileFolderCount { get; set; }
        public int LibraryFolderCount { get; set; }
       
        public Track()
        {
            this.TrackID = -1;
        }
    }
}