
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Ntier.DAL.DTO.User;
using System;
using System.Collections.Generic;

namespace Ntier.DAL.Entities
{
    public partial class AppUser : IdentityUser<string>
    {

        public AppUser()
        {
            Orders = new HashSet<Order>();
        }
        public string? FullName { get; set; }
        public DateTime? ExpireAt { get; set; }
        public string? RefreshTk { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

    }
}
