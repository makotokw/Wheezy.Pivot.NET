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
        public PivotItemAttribute() { }
        /// <summary>
        /// the type of facet to use for this property
        /// </summary>
        public FacetTypes FacetType { get; set; }
        /// <summary>
        /// the name to display instead of the name of the property for this facet.
        /// </summary>
        public string FacetDisplayName { get; set; }
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