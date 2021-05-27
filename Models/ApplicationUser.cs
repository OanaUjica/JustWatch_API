using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Lab1_.NET.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Reservation> Reservations { get; set; }
    }
}
