using AutoMapper;
using Domain.Entities;
using Infrastructure.Models;

namespace Infrastructure.Config
{
    public class Profile_Mapper : Profile
    {
        public Profile_Mapper()
        {
            // CATEGORY
            CreateMap<CategoryDTO, CategoryModel>().ReverseMap();
            CreateMap<CategoryRequestDTO, CategoryModel>().ReverseMap();

            // SUPPLIER
            CreateMap<SupplierDTO, SupplierModel>().ReverseMap();

            // EMPLOYEE
            CreateMap<EmployeeDTO, EmployeeModel>().ReverseMap();
            CreateMap<EmployeeDTO, EmployeeModel>().ReverseMap();

            // PRODUCT
            CreateMap<ProductModel, ProductDTO>()
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.CompanyName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.CategoryName));
        }
    }
}
