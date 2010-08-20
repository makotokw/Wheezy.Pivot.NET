using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wheezy.Pivot
{
    
    /// <summary>
    /// PivotItemAttribute class allows you to mark different properties of a class in a way that
    /// the PivotCollection class can read and correctly output a Pivot collection
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PivotItemAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PivotItemAttribute()
        {
            this.IsFilterVisible = true;
            this.IsWordWheelVisible = true;
            this.IsMetaDataVisible = true;
        }

        /// <summary>
        /// the type of facet to use for this property
        /// </summary>
        public FacetTypes FacetType { get; set; }

        /// <summary>
        /// the name to display instead of the name of the property for this facet.
        /// </summary>
        public string FacetDisplayName { get; set; }

        /// <summary>
        /// Optional .NET format string to be used for numeric types. It is recommended that data be rounded/truncated to match the format string to avoid potentially undesired behavior when filtering on that facet in the Pivot Graph View.
        /// </summary>
        public string FacetFormat { get; set; }

        /// <summary>
        /// Determines whether the category appears in the filter panel. Can only be true for categories of type String, Number, or DateTime.
        /// </summary>
        public bool IsFilterVisible { get; set; }

        /// <summary>
        /// Determines whether the category is visible in the info panel.
        /// </summary>
        public bool IsMetaDataVisible { get; set; }

        /// <summary>
        /// Determines whether the category is included in keyword filters over that collection. This attribute can only be true for categories of type String, LongString, or Link.
        /// </summary>
        public bool IsWordWheelVisible { get; set; }

        /// <summary>
        /// should the property that this attribute is associated with be treated as a facet and 
        /// mapped automatically.
        /// </summary>
        public bool IsFacet { get; set; }
        
        /// <summary>
        /// set this to true when attached to the property that is the display name for this item
        /// </summary>
        public bool IsName { get; set; }

        /// <summary>
        /// set this to true when attached to the property that is the link to associate with this item
        /// </summary>
        public bool IsHref { get; set; }

        /// <summary>
        /// set this to true when attached to the property that is the path to the image for this item
        /// </summary>
        public bool IsImage { get; set; }

        /// <summary>
        /// set this to true when attached to the property that is the description for this item
        /// </summary>
        public bool IsDescription { get; set; }
        
        /// <summary>
        /// any property that you set IsCollection to true on must impliment IEnumerable
        /// </summary>
        public bool IsCollection { get; set; }
    }
}