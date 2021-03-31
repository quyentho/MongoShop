using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoShop.BusinessDomain.Users
{
    public interface IUserServices
    {
        /// <summary>
        ///     Gets user with status = true by email.
        /// </summary>
        /// <param name="email">email of the user.</param>
        /// <returns>User that match required email or null if not found.</returns>
        Task<ApplicationUser> GetActiveUserByEmailAsync(string email);

        /// <summary>
        ///     Get user with status = true by id. Return null if not exist
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>User that match required id or null if not found.</returns>
        Task<ApplicationUser> GetActiveUserByIdAsync(string userId);

        /// <summary>
        ///     Gets all active users.
        /// </summary>
        /// <returns>List Users</returns>
        Task<List<ApplicationUser>> GetAllActiveUsersAsync();

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of users.</returns>
        Task<List<ApplicationUser>> GetAllAsync();

        /// <summary>
        ///     Update user's info.
        /// </summary>
        /// <param name="userId">Id of the required user.</param>
        /// <param name="newUser">New user contains changed fields.</param>
        /// <returns></returns>
        Task UpdateUserAsync(string userId, ApplicationUser newUser);

        /// <summary>
        /// Soft delete user by update state to inactive.
        /// </summary>
        /// <param name="userId">User id to delete.</param>
        /// <returns></returns>
        Task DeleteUserAsync(string userId);
    }
}
