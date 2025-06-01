using ProductCatalog.Common;
using ProductCatalog.Common.Product.Request;
using ProductCatalog.Common.Product.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service.Interfaces
{
    public interface IProductService
    {
        Task<BaseCommandResponse<GetProductResDTO>> GetProductByIdAsync(int id);

        Task<BaseCommandResponse<List<GetProductResDTO>>> GetAllProductsAsync();
        Task<BaseCommandResponse<List<GetProductResDTO>>> GetAllProductsByCategoryAsync(int id);

        Task<BaseCommandResponse<GetProductResDTO>> CreateProductAsync(CreateProductReqDTO product);

        Task<BaseCommandResponse<GetProductResDTO>> UpdateProductAsync(UpdateProductReqDTO product);

        Task<BaseCommandResponse<GetProductResDTO>> DeleteProductAsync(int id);
    }
}

