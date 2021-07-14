namespace Jaxofy.Models.Filters
{
    /// <summary>
    /// Filter for driving pagination of query responses.
    /// </summary>
    public class PaginationFilter
    {
        /// <summary>
        /// Which page to return (page index that starts at 1).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// How many items per page to return.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Prevent clients from over-querying.
        /// </summary>
        /// <param name="maxPageSize">Maximum amount of items per page allowed. Excess will be truncated, just tell clients not to query so damn much data, gosh!</param>
        public void Clamp(int maxPageSize = 100)
        {
            if (Page < 1)
                Page = 1;

            if (PageSize < 1)
                PageSize = 10;

            if (PageSize > maxPageSize)
                PageSize = maxPageSize;
        }
    }
}