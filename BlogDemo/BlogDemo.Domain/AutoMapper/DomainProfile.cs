using AutoMapper;
using BlogDemo.Domain.Data;
using BlogDemo.Domain.Models;

namespace BlogDemo.Domain.AutoMapper
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Post, PostItem>().ReverseMap();
        }
    }
}
