using AutoMapper;
using Ntier.DAL.DTO.Products;
using Ntier.DAL.DTO.User;
using Ntier.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.BLL.Extentions
{
    public class AutoMapper : Profile
    {
        public AutoMapper() { 
            CreateMap<AppUser,UserRegisterDTO>().ReverseMap();
            CreateMap<ProductImage, ImageDTO>().ReverseMap();
            CreateMap<ProductSizeDetail, StockDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
