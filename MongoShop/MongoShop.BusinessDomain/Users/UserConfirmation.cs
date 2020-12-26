using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MongoShop.BusinessDomain.Users
{

    public class UserConfirmation : IUserConfirmation<ApplicationUser>
    {
        public Task<bool> IsConfirmedAsync(UserManager<ApplicationUser> manager, ApplicationUser user) =>
            Task.FromResult(user.Status);
    }
}
