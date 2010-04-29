using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wheezy.Pivot
{
    public class FacetLink
    {
        public string Href { get; set; }
        public string Name { get; set; }

        public FacetLink()
        {
            this.Href = "";
            this.Name = "";
        }   

        public override string ToString()
        {
            return Name;
        }
    }
}
