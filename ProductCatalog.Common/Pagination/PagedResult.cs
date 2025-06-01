using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Common.Pagination
{
    public class PagedResult<TEntity> where TEntity : class
    {
        public List<TEntity> Result { get; set; }
        public int TotalCount { get; set; }
        public int page { get; set; }
        public int PageSize { get; set; }
    }
}
