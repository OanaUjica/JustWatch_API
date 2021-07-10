using Lab1_.NET.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Lab1_.NET.Data
{
    public class SeedUsers
    {
        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyz123456890";
        private static readonly Random random = new();

        public static void Seed(IServiceProvider serviceProvider, int count)
        {
            var userContext = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            if (userContext.Users.Count() < 1200)
            {
                for (int i = 0; i < count; ++i)
                {
                    var email = GetRandomString(2, 10) + "@" + GetRandomString(2, 3);
                    var firstName = GetRandomString(2, 10);
                    var lastName = GetRandomString(2, 10);
                    var role = GetRandomRole();

                    var user = new ApplicationUser
                    {
                        Email = email,
                        UserName = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Role = role
                    };
                    user.PasswordHash = userContext.PasswordHasher.HashPassword(user, "Test1234!");
                    user.EmailConfirmed = true;
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

        private static string GetRandomString(int min, int max)
        {
            string s = "";

            for (int j = 0; j < random.Next(min, max); ++j)
            {
                s += Characters[random.Next(Characters.Length)];
            }

            return s;
        }

        private static Role GetRandomRole()
        {
            var v = Enum.GetValues(typeof(Role));
            var randomGenre = (Role)v.GetValue(random.Next(v.Length));

            return randomGenre;
        }
    }
}
