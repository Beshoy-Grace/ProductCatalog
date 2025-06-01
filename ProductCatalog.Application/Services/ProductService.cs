using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.IServices;
using ProductCatalog.Common;
using ProductCatalog.Common.Product.Request;
using ProductCatalog.Common.Product.Response;
using ProductCatalog.Domain;
using ProductCatalog.Framework.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse<GetProductResDTO>> CreateProductAsync(CreateProductReqDTO product)
        {
            var category = _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.Id == product.CategoryId).Result.FirstOrDefault();
            if (category == null)
            {
                return new BaseCommandResponse<GetProductResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Category not found" } }
                };
            }
            var productEntity = _mapper.Map<Product>(product);
            await _unitOfWork.GetRepository<Product>().InsertAsync(productEntity);
            await _unitOfWork.SaveChangesAsync();
            return new BaseCommandResponse<GetProductResDTO> { ResponseData = _mapper.Map<GetProductResDTO>(productEntity), IsSuccess = true };
        }

        public async Task<BaseCommandResponse<GetProductResDTO>> DeleteProductAsync(int id)
        {
            var product = _unitOfWork.GetRepository<Product>().GetAllAsync(x => x.Id == id).Result.FirstOrDefault();
            if (product == null)
            {
                return new BaseCommandResponse<GetProductResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Product not found" } }
                };
            }
            _unitOfWork.GetRepository<Product>().Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return new BaseCommandResponse<GetProductResDTO> { ResponseData = _mapper.Map<GetProductResDTO>(product), IsSuccess = true, Message = "Product deleted successfully" };
        }
        public async Task<BaseCommandResponse<List<GetProductResDTO>>> GetAllProductsAsync()
        {
            var products = _unitOfWork.GetRepository<Product>().GetAllAsync(include: x => x.Include(p => p.Category));
            return new BaseCommandResponse<List<GetProductResDTO>>
            {
                ResponseData = products.Result.Select(p => _mapper.Map<GetProductResDTO>(p)).ToList(),
                IsSuccess = true
            };
        }

        public async Task<BaseCommandResponse<List<GetProductResDTO>>> GetAllProductsByCategoryAsync(int id)
        {
            var products = _unitOfWork.GetRepository<Product>().GetAllAsync(x => x.CategoryId == id);
         return new BaseCommandResponse<List<GetProductResDTO>> 
         {
             ResponseData = products.Result.Select(p => _mapper.Map<GetProductResDTO>(p)).ToList(),
             IsSuccess = true
         };
        }

        public async Task<BaseCommandResponse<GetProductResDTO>> GetProductByIdAsync(int id)
        {
             var product = _unitOfWork.GetRepository<Product>().GetAllAsync(x => x.Id == id,include: x => x.Include(p => p.Category)).Result.FirstOrDefault();
            if (product == null)
            {

                return new BaseCommandResponse<GetProductResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Product not found" } }
                };

            }
            return new BaseCommandResponse<GetProductResDTO> { ResponseData = _mapper.Map<GetProductResDTO>(product), IsSuccess = true };
        }

        public async Task<BaseCommandResponse<GetProductResDTO>> UpdateProductAsync(UpdateProductReqDTO product)
        {
            var category = _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.Id == product.CategoryId).Result.FirstOrDefault();
            if (category == null)
            {
                return new BaseCommandResponse<GetProductResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Category not found" } }
                };
            }
            var productEntity = _unitOfWork.GetRepository<Product>().GetAllAsync(x => x.Id == product.Id).Result.FirstOrDefault();
            if (productEntity == null)
            {
               
                return new BaseCommandResponse<GetProductResDTO>
                {
                    IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.NotFound, Value = "Product not found" } }
                };
            }
             productEntity = _mapper.Map<Product>(product);
            // productEntity.Id = product.Id;
            _unitOfWork.GetRepository<Product>().Update(productEntity);
            await _unitOfWork.SaveChangesAsync();
            return new BaseCommandResponse<GetProductResDTO> { ResponseData = _mapper.Map<GetProductResDTO>(productEntity), IsSuccess = true };
        }
    }
}
