﻿using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Data.Entities.Identity
{
    public class UserEntity : IdentityUser<long>
    {
        public DateTime DateCreated { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Image { get; set; } = null;

        public virtual ICollection<UserRoleEntity>? UserRoles { get; set; }
        public ICollection<CartEntity>? Carts { get; set; }
        public ICollection<OrderEntity>? Orders { get; set; }
        public virtual ICollection<UserLoginEntity>? UserLogins { get; set; }
    }
}
