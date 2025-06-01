using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Common.Pagination
{
    public class PagingParams
    {
        private const int MaxPageSize = 50;
        public int page { get; set; } = 1;
        private int pageSize = 10;
        public SortTypeEnum SortType { get; set; }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
    }
}
