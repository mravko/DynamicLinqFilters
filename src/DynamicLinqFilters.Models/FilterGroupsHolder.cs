using System.Collections.Generic;

namespace DynamicLinqFilters.Models
{
    public class FilterGroupsHolder
    {
        public List<FilterGroup> FilterGroups { get; set; } = new List<FilterGroup>();

        public FilterJoinType FilterGroupJoinType { get; set; }
    }
}
