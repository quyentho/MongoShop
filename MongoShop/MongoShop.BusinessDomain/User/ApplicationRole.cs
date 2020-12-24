﻿using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace MongoShop.BusinessDomain.User
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

    public enum UserRole
    {
        Admin,
        User
    }
}
