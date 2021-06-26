using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Lab1_.NET.Models;

namespace Lab1_.NET.Data
{
    public class SeedMovies
    {
        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyz";
        private static readonly Random random = new();

        public static void Seed(IServiceProvider serviceProvider, int count)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            if (context.Movies.Count() < 1200)
            {
                for (int i = 0; i < count; ++i)
                {
                    context.Movies.Add(new Movie
                    {
                        Title = GetRandomString(3, 10),
                        Description = GetRandomString(10, 100),
                        Director = GetRandomString(3, 10),
                        Genre = GetRandomGenre(),
                        DurationInMinutes = GetRandomUshort(20, 1600),
                        YearOfRelease = GetRandomUshort(1860, (ushort)DateTime.Now.Year),
                        Watched = GetRandomBoolean(),
                        DateAdded = GetRandomDate(),
                        Rating = GetRandomFloat(1.0f, 10.0f)
                    });
                }

                context.SaveChanges();
            }
        }
        private static bool GetRandomBoolean()
        {
            return random.Next(0, 2000) < 1000;
        }

        private static float GetRandomFloat(float min, float max)
        {
            var r =  random.Next((int)min, (int)max);
            return r;
        }

        private static ushort GetRandomUshort(ushort min, ushort max)
        {
            return (ushort)random.Next(min, max);
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

        private static string GetRandomGenre()
        {
            var v = Enum.GetValues(typeof(MovieGenre));
            var randomGenre =  (MovieGenre)v.GetValue(random.Next(v.Length));

            return randomGenre.ToString();
        }

        private static DateTime GetRandomDate()
        {
            int rangePastThreeYears = 3 * 365;
            DateTime randomDate = DateTime.Today.AddDays(-random.Next(rangePastThreeYears));

            return randomDate;
        }
    }
}
