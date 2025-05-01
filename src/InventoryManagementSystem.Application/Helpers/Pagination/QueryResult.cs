using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Application.Helpers.Pagination
{
    public class QueryResult<T> where T : class
    {
        public int TotalRecords { get; set; }
        public List<T> Items { get; set; }

        public QueryResult(List<T> items, int totalRecords = 0)
        {
            TotalRecords = totalRecords;
            Items = items;
        }
    }
}