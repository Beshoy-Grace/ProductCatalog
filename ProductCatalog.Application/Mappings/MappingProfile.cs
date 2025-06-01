using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Product
            CreateMap<Common.Product.Request.CreateProductReqDTO, Domain.Product>();
            CreateMap<Common.Product.Request.UpdateProductReqDTO, Domain.Product>();
            CreateMap<Domain.Product, Common.Product.Response.GetProductResDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            //Category
            CreateMap<Common.Category.Request.CreateCategoryReqDTO, Domain.Category>();
            CreateMap<Common.Category.Request.UpdateCategoryReqDTO, Domain.Category>();
            CreateMap<Domain.Category, Common.Category.Response.GetCategoryWithProductResDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        }
    }
}
