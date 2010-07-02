using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using Microsoft.DeepZoomTools;

namespace Wheezy.Pivot
{
    public class PivotCollection<T> : List<T>
    {
        protected static Facet[] collectionFacets;
        protected static PropertyInfo nameProperty;
        protected static PropertyInfo descriptionProperty;
        protected static PropertyInfo hrefProperty;
        protected static PropertyInfo imageProperty;
        
        public string SchemeVersion { get; set; }
        public string CollectionName { get; set; }
        public string AdditionalSearchText { get; set; }
        public string DziDestDirName { get; set; }

        public CollectionCreator DzcCreator { get; protected set; }
        public ImageCreator DziCreator { get; protected set; }
        public string TempImageDir { get; set; }

        protected Dictionary<string, int> imageIds = new Dictionary<string, int>(300); // pathName => imageId

        // factory for DeepZoom Creator
        virtual public ImageCreator CreateDeepZoomImageCreator() { return new ImageCreator(); }
        virtual public CollectionCreator CreateDeepZoomCollectionCreator() { return new CollectionCreator(); }
        
        public PivotCollection()
        {
            this.SchemeVersion = "1.0";
            this.CollectionName = "PivotCollection";
            this.AdditionalSearchText = string.Empty;
            this.DziDestDirName = @"dzi";

            this.TempImageDir = "DownloadedImages";

            this.DziCreator = this.CreateDeepZoomImageCreator();
            this.DziCreator.ServerFormat = ServerFormats.Default;
            this.DziCreator.TileFormat = ImageFormat.AutoSelect;
            this.DziCreator.TileSize = 254;
            this.DziCreator.MaxLevel = 8;
            this.DziCreator.ImageQuality = 0.95D;
            this.DziCreator.CopyMetadata = true;

            this.DzcCreator = this.CreateDeepZoomCollectionCreator();
            this.DzcCreator.ServerFormat = ServerFormats.Default;
            this.DzcCreator.TileFormat = ImageFormat.Jpg;
            this.DzcCreator.TileSize = 256;
            this.DzcCreator.MaxLevel = 8;
            this.DzcCreator.ImageQuality = 0.95D;
           
            this.validateProperties();
        }

        protected void validateProperties()
        {
            // find all properties that have a PivotItem Attribute
            IEnumerable<PropertyInfo> properties = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(false).Any(a => a.GetType() == typeof(PivotItemAttribute)));
            var facets = new List<Facet>();
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttributes(typeof(PivotItemAttribute), false).FirstOrDefault() as PivotItemAttribute;
                if (attr != null)
                {
                    if (attr.IsFacet)
                    {
                        var facet = new Facet { FacetName = property.Name, FacetDisplayName = attr.FacetDisplayName, FacetType = attr.FacetType };
                        facet.FacetOccurrenceLimit = (attr.IsCollection) ? FacetOccurrenceLimits.Many : FacetOccurrenceLimits.Single;
                        facets.Add(facet);
                    }
                    else
                    {
                        if (attr.IsName) nameProperty = property;
                        if (attr.IsHref) hrefProperty = property;
                        if (attr.IsImage) imageProperty = property;
                        if (attr.IsDescription) descriptionProperty = property;
                    }
                }
            }
            if (nameProperty == null)
            {
                throw new Exception("Name is not optional. Please make one of the properties of this class as IsName=true on its PivotItem Attribute");
            }
            collectionFacets = facets.ToArray();
        }

        virtual protected SurrogateImageInfo? createDeepZoomImage(string destination, Item collectionItem)
        {
            try
            {
                bool exists = false;
                string imagePathName = getImagePathNameForDeepZoomComposer(collectionItem.ImagePath);
                if (this.imageIds.ContainsKey(collectionItem.ImagePath))
                {
                    // already created
                    collectionItem.ImageId = this.imageIds[collectionItem.ImagePath];
                    exists = true;
                    return null;
                }
                string deepZoomImage = destination + Path.DirectorySeparatorChar + collectionItem.ImageId;
                // xmlImage should be URI for Silverlight PivotViewer
                var info = new SurrogateImageInfo(imagePathName, new Uri(deepZoomImage + ".xml").ToString());
                if (!exists)
                {
                    collectionItem.ImageId = imageIds.Count;
                    DziCreator.Create(imagePathName, deepZoomImage); // output desination.xml and desination/*/*.jpg                
                    imageIds.Add(collectionItem.ImagePath, collectionItem.ImageId);
                }
                return info;
            }
            catch (Exception)
            {
                // this item will not be added to the collection
            }
            return null;
        }

        virtual protected string getImagePathNameForDeepZoomComposer(string location)
        {
            if (location.StartsWith("http://") || location.StartsWith("https://"))
            {
                var wc = new WebClient();
                string tmpPathName = this.TempImageDir + Guid.NewGuid();
                wc.DownloadFile(location, tmpPathName);
                return tmpPathName;
            }
            return location;
        }

        virtual public void Write(string pathName)
        {
            var targetDir = Path.GetDirectoryName(pathName);
            var imageBase = Path.GetFileNameWithoutExtension(pathName) + ".dzc";
            var imageDir = Path.GetFileNameWithoutExtension(pathName) + "_files";
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
#if DEBUG
            using (var writer = new DebugWriter(pathName))
#else
            using (var writer = new StreamWriter(pathName))
#endif
            {
                writer.AutoFlush = true;

                { // header
                    writer.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                    string headFormat = "<Collection"
                                        + " xmlns=\"http://schemas.microsoft.com/collection/metadata/2009\""
                                        + " xmlns:p=\"http://schemas.microsoft.com/livelabs/pivot/collection/2009\""
                                        + " Name=\"{0}\""
                                        + "{1}"
                                        + " SchemaVersion=\"{2}\">"
                                        ;
                    string head = string.Format(headFormat,
                        this.CollectionName,
                        (this.AdditionalSearchText != string.Empty) ? string.Format(" p:AdditionalSearchText=\"{0}\"", this.AdditionalSearchText) : "", 
                        SchemeVersion);
                    writer.Write(head);
                }

                { // facet
                    if (collectionFacets.Length > 0)
                    {
                        writer.Write("<FacetCategories>");
                        string template = "<FacetCategory Name=\"{0}\" Type=\"{1}\" p:IsFilterVisible=\"true\" p:IsWordWheelVisible=\"true\" p:IsMetaDataVisible=\"true\"/>";
                        foreach (Facet facet in collectionFacets)
                        {
                            writer.Write(string.Format(template, facet.FacetDisplayName, facet.FacetType));
                        }
                        writer.Write("</FacetCategories>");
                    }
                }

                { // items
                    var index = 0;
                    var collectionItems = new List<Item>(this.Count);
                    var dzImages = new List<Microsoft.DeepZoomTools.SurrogateImageInfo>();
                    var dzImageTargetDir = targetDir + Path.DirectorySeparatorChar + DziDestDirName;
                    if (!Directory.Exists(dzImageTargetDir)) Directory.CreateDirectory(dzImageTargetDir);
                    writer.Write(string.Format("<Items ImgBase=\"{0}\">", imageBase));
                    foreach (var item in this)
                    {
                        var collectionItem = new Item(index, item);
                        if (collectionItem.Image != null)
                        {
                            collectionItem.CreateFacets(collectionFacets);

                            var imageInfo = this.createDeepZoomImage(dzImageTargetDir, collectionItem);
                            if (imageInfo != null)
                            {
                                dzImages.Add(imageInfo.Value);
                                writer.Write(collectionItem.ToString()); // TODO: 
                                index++;
                            }
                            else
                            {
                                // TODO: 
                                Console.WriteLine("DeepZoomImage can not be created for " + collectionItem.Name);
                            }                            
                        }
                        Console.Write(".");
                    }
                    writer.Write("</Items>");
                    if (dzImages.Count > 0)
                    {
                        try
                        {
                            Directory.Delete(imageDir, true);
                        }
                        catch (Exception) { }
                        this.DzcCreator.Create(dzImages, targetDir + Path.DirectorySeparatorChar + imageBase);
                    }
                }

                { // close xml
                    writer.Write("</Collection>");
                }
            }
        }

        protected static string GetHref(T item)
        {
            if (hrefProperty == null)
            {
                return "";
            }
            object obj = hrefProperty.GetValue(item, null);
            if (obj != null)
            {
                return " Href=\"" + obj + "\"";
            }
            return "";
        }

        protected static string GetName(T item)
        {
            string name = "";
            object obj = nameProperty.GetValue(item, null);
            if (obj != null)
            {
                name = obj.ToString();
            }
            return " Name=\"" + SecurityElement.Escape(name) + "\"";
        }

        protected static string GetImage(T item)
        {
            if (imageProperty == null)
            {
                return "";
            }
            object obj = imageProperty.GetValue(item, null);
            if (obj != null)
            {
                return " Img=\"" + obj + "\"";
            }
            return "";
        }

        protected static string GetDescription(T item)
        {
            if (descriptionProperty == null)
            {
                return "";
            }
            object obj = descriptionProperty.GetValue(item, null);
            if (obj != null)
            {
                return obj.ToString();
            }
            return "";
        }

        public class DebugWriter : StreamWriter
        {
            public DebugWriter(string pathName)
                : base(pathName)
            {
            }
            public override void Write(string value)
            {
                value = value.Replace(">", ">" + System.Environment.NewLine);
                base.Write(value);
            }
        }

        #region Nested type: Facet

        public class Facet
        {
            public const string DATETIME_TEMPLATE = "<DateTime Value=\"{0}\"/>";
            public const string FACET_TEMPLATE = "<Facet Name=\"{0}\">{1}</Facet>";
            public const string LINK_TEMPLATE = "<Link Name=\"{0}\" Href=\"{1}\"/>";
            public const string NUMBER_TEMPLATE = "<Number Value=\"{0}\"/>";
            public const string STRING_TEMPLATE = "<String Value=\"{0}\"/>";
               
            public string FacetName { get; set; }
            public string FacetDisplayName { get; set; }
            public FacetTypes FacetType { get; set; }
            public List<object> Values { get; private set; }
            protected FacetOccurrenceLimits facetOccurrenceLimit;           
            public FacetOccurrenceLimits FacetOccurrenceLimit
            {
                get { return facetOccurrenceLimit; }
                set
                {
                    if (facetOccurrenceLimit != value)
                    {
                        facetOccurrenceLimit = value;
                        if (facetOccurrenceLimit == FacetOccurrenceLimits.Single && Values.Count > 1)
                        {
                            Values.RemoveRange(0, Values.Count - 2);
                        }
                    }
                }
            }            

            public Facet()
            {
                this.Values = new List<object>();
            }

            public int AddValue(object value)
            {
                if (value == null)
                {
                    return -1;
                }
                if (facetOccurrenceLimit == FacetOccurrenceLimits.Single && Values.Count == 1)
                {
                    Values[0] = value;
                    return 0;
                }
                Values.Add(value);
                return Values.Count - 1;
            }

            public void ReadCollection(T backingItem)
            {
                Values.Clear();
                var items = typeof (T).GetProperty(FacetName).GetValue(backingItem, null) as IEnumerable;
                if (items == null)
                {
                    return;
                }
                foreach (object o in items)
                {
                    Values.Add(o);
                }
            }

            override public string ToString()
            {
                if (Values.Count == 0)
                {
                    return "";
                }

                string template = "";
                switch (FacetType)
                {
                    case FacetTypes.DateTime:
                        template = DATETIME_TEMPLATE;
                        break;
                    case FacetTypes.Item:
                        throw new NotImplementedException("Item Facet types are not yet supported");
                    case FacetTypes.Link:
                        template = LINK_TEMPLATE;
                        break;
                    case FacetTypes.Number:
                        template = NUMBER_TEMPLATE;
                        break;
                    default:
                        template = STRING_TEMPLATE;
                        break;
                }

                string fullFacet = "";
                foreach (object value in Values)
                {
                    // check to see if clean output is on and if so indent this with three tabs
                    if (FacetType == FacetTypes.Link)
                    {
                        var link = value as FacetLink;
                        Debug.Assert(link != null, "must use PivotFacetLink object for Link");
                        fullFacet += string.Format(template, link.Name, link.Href);
                    }
                    else
                    {
                        fullFacet += string.Format(template, SecurityElement.Escape(value.ToString()));
                    }
                }
                // put the facet into the facet template
                return string.Format(FACET_TEMPLATE, FacetDisplayName, fullFacet);
            }
        }

        #endregion

        #region Nested type: Item

        public class Item
        {
            protected readonly T associatedItem;
            protected readonly Dictionary<string, Facet> facets = new Dictionary<string, Facet>();
            public T AssociatedItem { get { return this.associatedItem; } }
            public int Id { get; set; }
            public int? _imageId;
            public int ImageId
            {
                get { return _imageId.HasValue ? _imageId.Value : Id; }
                set { _imageId = value; }
            }
            public string Image { get { return GetImage(associatedItem); } }
            public string ImagePath { get { return imageProperty.GetValue(associatedItem, null) as string; } }
            public string Href { get { return GetHref(associatedItem); } }
            public string Name { get { return GetName(associatedItem); } }
            public string Description { get { return GetDescription(associatedItem); } }

            public Item(int id, T item)
            {
                Id = id;
                associatedItem = item;
            }

            public void CreateFacets(Facet[] collectionFacets)
            {
                foreach (Facet facet in collectionFacets)
                {
                    var f = new Facet { FacetName = facet.FacetName, FacetDisplayName = facet.FacetDisplayName, FacetType = facet.FacetType };
                    f.FacetOccurrenceLimit = facet.FacetOccurrenceLimit;
                    facets.Add(facet.FacetName, f);
                }
            }

            protected void grabFacets()
            {
                foreach (var pair in facets)
                {
                    if (pair.Value.FacetOccurrenceLimit == FacetOccurrenceLimits.Single)
                    {
                        object value = typeof(T).GetProperty(pair.Key).GetValue(associatedItem, null);
                        pair.Value.AddValue(value);
                    }
                    else
                    {
                        pair.Value.ReadCollection(associatedItem);
                    }
                }
            }

            override public string ToString()
            {
                string item = string.Format("<Item Id=\"{0}\" Img=\"#{1}\" {2} {3}>", Id, ImageId, Name, Href);
                string facetString = "<Facets>";
                grabFacets();                
                if (Description.Length > 0)
                {
                    item += string.Format("<Description>{0}</Description>", SecurityElement.Escape(Description));
                }
                Facet[] items = facets.Select(p => p.Value).ToArray();
                int i = 0;
                foreach (Facet facet in items)
                {
                    string str = facet.ToString();
                    if (str.Length > 0)
                    {
                        i++;
                    }
                    facetString += str;
                }
                if (i > 0) item += facetString + "</Facets>";
                item += "</Item>";
                return item;
            }
        }

        #endregion
    }
} 