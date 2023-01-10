using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ActionResult<IEnumerable<User>> GetAll()
        {
            return userDBContext.Users.ToList();
        }

        public ActionResult<User> GetById(int id)
        {
            return userDBContext.Users.FirstOrDefault(u => u.UserId == id);
        }

        public ActionResult<User> GetByString(string email)
        {
            return userDBContext.Users.FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
        }

        public async Task AddAsync(User entity)
        {
            await userDBContext.Users.AddAsync(entity);
            await userDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user, User entity)
        {
            userDBContext.Entry(user).State = EntityState.Modified;
            user.UserId = entity.UserId;
            user.Nom = entity.Nom;
            user.ProfilePictureUrl = entity.ProfilePictureUrl;
            user.Email = entity.Email;
            await userDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            userDBContext.Users.Remove(user);
            await userDBContext.SaveChangesAsync();
        }

    }
}
