using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoShop.Infrastructure.Helpers;

namespace MongoShop.BusinessDomain.Users
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<ApplicationUser> _collection;
        private readonly string _collectionName;

        public UserServices(IMongoClient mongoClient, IOptions<DatabaseSetting> settings)
        {
            _collectionName = MongoDbHelper.GetCollectionName(this.GetType().Name);

            var database =
            mongoClient.GetDatabase(settings.Value.DatabaseName);

            _collection = database.GetCollection<ApplicationUser>(_collectionName);
        }

        /// <inheritdoc/>
        public async Task DeleteUserAsync(string userId)
        {
            var user = await GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("User not exists.");
            }

            user.Status = false;

            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationUser>> GetAllActiveUsersAsync()
        {
            var users = await _collection.FindAsync(u => u.Status == true);

            return await users.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetActiveUserByEmailAsync(string email)
        {
            var users = await _collection.FindAsync(u => u.Email.Equals(email) && u.Status == true);

            return users.SingleOrDefault();
        }

        /// <inheritdoc/>
        public async Task<ApplicationUser> GetActiveUserByIdAsync(string userId)
        {
            var id = Guid.Parse(userId);
            
            var user = await _collection.FindAsync(u => u.Id == id && u.Status == true);

            return user.SingleOrDefault();
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
            var user = await GetActiveUserByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException("User not exists");
            }

            newUser.Id = Guid.Parse(userId);

            await _collection.ReplaceOneAsync(u => u.Id == user.Id, newUser);
        }
    }
}
