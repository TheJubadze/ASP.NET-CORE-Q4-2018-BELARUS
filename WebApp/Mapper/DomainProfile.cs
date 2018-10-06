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
        }
    }
}
