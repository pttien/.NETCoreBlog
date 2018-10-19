using AutoMapper;
using BlogDemo.Domain.Data;
using BlogDemo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
