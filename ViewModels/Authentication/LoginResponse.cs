using Lab1_.NET.Models;
using System;

namespace Lab1_.NET.ViewModels.Authentication
{
    public class LoginResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Role Role { get; set; }

        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
