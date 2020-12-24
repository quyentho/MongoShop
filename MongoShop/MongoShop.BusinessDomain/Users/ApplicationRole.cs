using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MongoShop.BusinessDomain.Products
{
    [CollectionName("role")]
    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole() : base()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }

    public static class UserRole
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
