using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Lab1_.NET.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Role Role { get; set; }

        public List<Watchlist> Watchlists { get; set; }
    }

    public enum Role
    {
        Admin,
        User
    }
}
