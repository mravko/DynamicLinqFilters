using System.Collections.Generic;

namespace DynamicLinqFilters.Models
{
    public class Filter
    {
        public string PropertyName { get; set; }

        public List<FilterValue> Value { get; set; } = new List<FilterValue>();

        public FilterJoinType FilterValueJoinType { get; set; } = FilterJoinType.Or;
    }
}
