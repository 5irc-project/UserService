using UserService.DTO;
using UserService.Models.EntityFramework;

namespace UserService.Mappers
{
    public class UserMapper
    {
        public static UserDTO ModelToDto(User user)
        {
            UserDTO dto = new UserDTO();
            dto.UserId = user.UserId;
            dto.Email = user.Email;
            dto.Nom = user.Nom;
            dto.ProfilePictureUrl = user.ProfilePictureUrl;

            return dto;
        }

        public static User DtoToModel(UserDTO dto)
        {
            User user = new User();
            user.Email = dto.Email;
            user.Nom = dto.Nom;
            user.ProfilePictureUrl = dto.ProfilePictureUrl;

            return user;
        }
    }
}
