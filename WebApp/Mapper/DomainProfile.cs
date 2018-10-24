using AutoMapper;
using DataAccess.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<ProductEditViewModel, Product>();
            
            CreateMap<CategoryEditViewModel, Category>()
                .ForMember(dest => dest.CategoryName, opts => opts.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Category.Description));
        }
    }
}
