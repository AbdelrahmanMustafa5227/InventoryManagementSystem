using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Helpers.Pagination
{
    public class Paginated<T> : IEnumerable<T> where T : class
    {
        private readonly int PageSize = 2;

        [JsonIgnore]
        public int TotalCount { get; set; }

        public int PageIndex { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasNextPage => PageIndex * PageSize < TotalCount;
        public bool HasPrevPage => PageIndex > 1;
        public List<T> Items { get; set; } = null!;

        [JsonConstructor]
        public Paginated(List<T> items, int pageIndex, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalCount = totalCount;
        }


        public static Paginated<T> Create(List<T> items, int pageNumber, int totalCount)
        {
            return new Paginated<T>(items, pageNumber, totalCount);
        }

        public static Paginated<T> CreateEmpty() => new Paginated<T>([], 1, 0);

        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}