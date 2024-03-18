    using AutoMapper;
using Ntier.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.DTO.User
{
    public class UserRegisterDTO
    {
        public string UserId { get; set; } = null!;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public class AutoMapper : Profile
        {
            public AutoMapper() 
            {
                CreateMap<AppUser, UserRegisterDTO>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));

                CreateMap<UserRegisterDTO, AppUser>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            }
        }
    }
}
