using AutoMapper;
using ZeemProductManagementTest.Models;
using ZeemProductManagementTest.Services.Pagination;

namespace ZeemProductManagementTest.DTOs
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<Product, GetProductDTO>();
            CreateMap<PaginationModel<Product>, PaginationModel<GetProductDTO>>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
}
