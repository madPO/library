using Microsoft.AspNet.Identity;

namespace application.Models
{
    public class UserManager : UserManager<Users, int>
    {
        public UserManager(IUserStore<Users, int> store)
            : base(store)
        {
            UserValidator = new UserValidator<Users, int>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}