using MMT_OMS.Models;
using System.Linq;

namespace MMT_OMS.Services
{
    public class UserService
    {
        public User ValidateUser(string username, string password)
        {
            using(var context = new MMTModel())
            {
                var user = context.Users.FirstOrDefault(dbUser => dbUser.Username == username);
                if (user == null)
                {
                    return user;
                }

                var isValidUser = SecurityService.ConfirmPassword(password, user.Salt, user.PasswordHash);
                return isValidUser ? user : null;
            }
        }
    }
}