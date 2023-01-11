using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Exceptions;
using UserService.Models.EntityFramework;
using UserService.Models.Repository;

namespace UserService.Models.DataManager
{
    public class UserManager : IDataRepository<User>
    {
        readonly UserDBContext? userDBContext;

        public UserManager(){}
        public UserManager(UserDBContext userDBContext)
        {
            this.userDBContext = userDBContext;
        }

        public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
        {
            return await userDBContext.Users.ToListAsync();
        }

        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            var user = await userDBContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task<ActionResult<User>> GetByStringAsync(string email)
        {
            var user = await userDBContext.Users.FirstOrDefaultAsync(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            return user;
        }

        public async Task AddAsync(User entity)
        {
            try
            {
                await userDBContext.Users.AddAsync(entity);
                await userDBContext.SaveChangesAsync();
            } catch (Exception) {
                throw new UserDBCreationException();
            }

        }

        public async Task UpdateAsync(User user, User entity)
        {
            try
            {
                userDBContext.Entry(user).State = EntityState.Modified;
                user.UserId = entity.UserId;
                user.Nom = entity.Nom;
                user.ProfilePictureUrl = entity.ProfilePictureUrl;
                user.Email = entity.Email;
                await userDBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new UserDBUpdateException();
            }
        }

        public async Task DeleteAsync(User user)
        {
            try
            {
                userDBContext.Users.Remove(user);
                await userDBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new UserDBDeletionException();
            }

        }

    }
}
