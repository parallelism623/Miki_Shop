using AutoMapper;
using Ntier.DAL.Entities;

namespace Ntier.DAL.DTO.User
{
    public class UserDTO
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; }
        public string Name { get; set; }


        public string Access_token { get; set; }

        public DateTime Expire_At { get; set; }

        public string Refresh_Token { get; set; }
        public class AutoMapper : Profile
        {
            public AutoMapper()
            {
                CreateMap<AppUser, UserDTO>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                    .ForMember(dest => dest.Expire_At, opt => opt.MapFrom(src => src.ExpireAt))
                    .ForMember(dest => dest.Refresh_Token, opt => opt.MapFrom(src => src.RefreshTk));
                CreateMap<UserDTO, AppUser>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.ExpireAt, opt => opt.MapFrom(src => src.Expire_At))
                    .ForMember(dest => dest.RefreshTk, opt => opt.MapFrom(src => src.Refresh_Token))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            }
        }
    }
}
