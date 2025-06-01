using ProductCatalog.Common;
using ProductCatalog.Common.Category.Request;
using ProductCatalog.Common.Category.Response;
using ProductCatalog.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.IServices
{
    public interface ICategoryService
    {
        Task<BaseCommandResponse<PagedResult<GetCategoryResDTO>>> GetCategories(GetAllCategoryDTO req);
        Task<BaseCommandResponse<GetCategoryWithProductResDTO>> GetCategory(int id);
        Task<BaseCommandResponse<GetCategoryResDTO>> CreateCategory(CreateCategoryReqDTO createCategoryReqDTO);
        Task<BaseCommandResponse<GetCategoryResDTO>> UpdateCategory(UpdateCategoryReqDTO updateCategoryReqDTO);
        Task<BaseCommandResponse<bool>> DeleteCategory(int id);

    }
}
