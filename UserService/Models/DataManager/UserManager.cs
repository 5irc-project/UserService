using Microsoft.AspNetCore.Mvc;
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

        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            return userDBContext.Users.ToList();
        }


    }
}
