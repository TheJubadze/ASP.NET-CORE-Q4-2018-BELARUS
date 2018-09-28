using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        public ICollection<Territory> Territories { get; set; }
    }
}
