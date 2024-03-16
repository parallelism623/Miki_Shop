using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.SeedWorks
{
    public static class Permission
    {
        public static class Admin
        {
            public const string All = "Permission.Admin";
        }
        public static class User
        {
            public const string Get = "Permission.User.Get";
            public const string Delete = "Permission.User.Delete";
            public const string Update = "Permission.User.Update";
            public const string Add = "Permission.User.Add";
        }
    }
}
