using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DCC.API.Model
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}