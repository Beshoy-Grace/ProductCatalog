using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.IServices;
using ProductCatalog.Common;
using ProductCatalog.Common.Category.Request;
using ProductCatalog.Common.Category.Response;
using ProductCatalog.Common.Pagination;
using ProductCatalog.Domain;
using ProductCatalog.Framework.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse<GetCategoryResDTO>> CreateCategory(CreateCategoryReqDTO createCategoryReqDTO)
        {
            var category = new Category
            {
                Name = createCategoryReqDTO.Name,
            };

            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return new BaseCommandResponse<GetCategoryResDTO> { ResponseData = new GetCategoryResDTO { Id = category.Id, Name = category.Name }, IsSuccess = true };
        }

        public async Task<BaseCommandResponse<bool>> DeleteCategory(int id)
        {
           var category = (await _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.Id == id,include: x => x.Include(p => p.Products))).FirstOrDefault();
            if (category == null)
            {
                return new BaseCommandResponse<bool> { ResponseData = false, IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Category not found" } }
                };
            }
            if (category.Products != null && category.Products.Any())
            {
                return new BaseCommandResponse<bool>
                {
                    ResponseData = false,
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.BadRequest, Value = "Category cannot be deleted as it has associated products" } }
                };
            }
            _unitOfWork.GetRepository<Category>().Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return new BaseCommandResponse<bool> { ResponseData = true, IsSuccess = true , Message = "Category deleted successfully"};
        }

        public async Task<BaseCommandResponse<PagedResult<GetCategoryResDTO>>> GetCategories(GetAllCategoryDTO req)
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAllAsync();
            if (categories.Result == null || !categories.Result.Any())
            {
                return new BaseCommandResponse<PagedResult<GetCategoryResDTO>>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "No categories found" } }
                };
            }

            var result = categories.Result.Skip((req.page - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList(); ;
            var pagedResult = new PagedResult<GetCategoryResDTO>
            {
                Result = result.Select(c => new GetCategoryResDTO { Id = c.Id, Name = c.Name }).ToList(),
                TotalCount = categories.Result.Count(),
                PageSize = req.PageSize,
                page = req.page
            };

            return new BaseCommandResponse<PagedResult<GetCategoryResDTO>> { ResponseData = pagedResult, IsSuccess = true };
           
        }

        public Task<BaseCommandResponse<GetCategoryWithProductResDTO>> GetCategory(int id)
        {
            var category = _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.Id == id,include: x => x.Include(p => p.Products)).Result.FirstOrDefault();
            if (category == null)
            {
                return Task.FromResult(new BaseCommandResponse<GetCategoryWithProductResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Category not found" } }
                });
            }
            return Task.FromResult(new BaseCommandResponse<GetCategoryWithProductResDTO> { ResponseData = _mapper.Map<GetCategoryWithProductResDTO>(category), IsSuccess = true });
        }

        public Task<BaseCommandResponse<GetCategoryResDTO>> UpdateCategory( UpdateCategoryReqDTO updateCategoryReqDTO)
        {
           var category = _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.Id == updateCategoryReqDTO.Id).Result.FirstOrDefault();
            if (category == null)
            {
                return Task.FromResult(new BaseCommandResponse<GetCategoryResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Category not found" } }
                });
            }
            category.Name = updateCategoryReqDTO.Name;
            _unitOfWork.GetRepository<Category>().Update(category);
            _unitOfWork.SaveChangesAsync();
            return Task.FromResult(new BaseCommandResponse<GetCategoryResDTO> { ResponseData = new GetCategoryResDTO { Id = category.Id, Name = category.Name }, IsSuccess = true });
        }
    }
}
