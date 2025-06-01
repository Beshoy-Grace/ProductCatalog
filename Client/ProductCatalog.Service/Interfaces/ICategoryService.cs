using ProductCatalog.Common;
using ProductCatalog.Common.Category.Request;
using ProductCatalog.Common.Category.Response;
using ProductCatalog.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResult<GetCategoryResDTO>> GetAllAsync(GetAllCategoryDTO req);
        Task<GetCategoryWithProductResDTO?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateCategoryReqDTO category);
        Task<bool> UpdateAsync(UpdateCategoryReqDTO category);
        Task<BaseCommandResponse<bool>> DeleteAsync(int id);
    }
}
