using AutoMapper;
using UserService.DTO;
using UserService.Models.EntityFramework;

namespace UserService.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
