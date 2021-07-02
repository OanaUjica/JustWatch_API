using Lab1_.NET.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Lab1_.NET.Data
{
    public class SeedReviews
    {
        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyz123456890";
        private static readonly Random random = new();

        public static void Seed(IServiceProvider serviceProvider, int count)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            var numberOfMovies = context.Movies.Count();

            if (context.Reviews.Count() < 1200)
            {
                for (int i = 0; i < count; ++i)
                {
                    var movie = context.Movies.Skip(random.Next(1, numberOfMovies)).Take(1).First();

                    context.Reviews.Add(new Review
                    {
                        Text = GetRandomString(3, 10),
                        Important = GetRandomBoolean(),
                        DateTime = GetRandomDate(),
                        Movie = movie
                    });
                }

                context.SaveChanges();
            }
        }
        private static bool GetRandomBoolean()
        {
            return random.Next(0, 2000) < 1000;
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

        private static DateTime GetRandomDate()
        {
            int rangePastThreeYears = 3 * 365;
            DateTime randomDate = DateTime.Today.AddDays(-random.Next(rangePastThreeYears));

            return randomDate;
        }
    }
}
