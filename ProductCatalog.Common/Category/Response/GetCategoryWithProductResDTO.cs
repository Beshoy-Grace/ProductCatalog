using ProductCatalog.Common.Product.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Common.Category.Response
{
    public class GetCategoryWithProductResDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetProductResDTO> Products { get; set; }
    }
}
