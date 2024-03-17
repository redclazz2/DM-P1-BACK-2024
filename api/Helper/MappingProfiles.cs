using api.Dto;
using api.Models;
using AutoMapper;

namespace api.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles(){
            CreateMap<Product,ProductDto>();
        }
    }
}