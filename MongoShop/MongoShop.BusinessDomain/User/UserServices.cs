using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoShop.BusinessDomain.User
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<ApplicationUser> _collection;
        private readonly IDatabaseSetting _databaseSetting;
        private const string CollectionName = "user";

        public UserServices(IDatabaseSetting databaseSetting)
        {
            _databaseSetting = databaseSetting;

            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);

            _collection = database.GetCollection<ApplicationUser>(CollectionName);
        }

        /// <inheritdoc/>
        public async Task DeleteUserAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);

            if (user is null)
            {
                throw new ArgumentOutOfRangeException(userId, "User not exists.");
            }

            user.Status = false;

            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationUser>> GetActiveUsersAsync()
        {
            var users = await _collection.FindAsync(u => u.Status == true);

            return await users.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var users = await _collection.FindAsync(u => u.Email.Equals(email));

            return users.SingleOrDefault();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var id = Guid.Parse(userId);
            var users = await _collection.FindAsync(u => u.Id == id);

            return users.SingleOrDefault();
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            var users = await _collection.FindAsync(_ => true);
            return await users.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateUserAsync(string userId, ApplicationUser newUser)
        {
            var user = await GetUserByIdAsync(userId);

            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
    }
}
