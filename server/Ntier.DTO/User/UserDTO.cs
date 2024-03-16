using AutoMapper;

namespace Ntier.DTO.User
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
                CreateMap<UserDTO, AppUser>();
            }
        }
    }
}
