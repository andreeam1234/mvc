using Lab06.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lab06.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.MigrateAsync();

        string[] roleNames = ["Admin", "User"];

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@newsportal.com";

        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        if (!context.Categories.Any() && !context.Articles.Any())
        {
            var technology = new Category { Name = "Tehnologie" };
            var sport = new Category { Name = "Sport" };
            var culture = new Category { Name = "Cultură" };
            var actualitate = new Category { Name = "Actualitate" };

            context.Categories.AddRange(technology, sport, culture, actualitate);
            await context.SaveChangesAsync();

            context.Articles.AddRange(
                new Article
                {
                    Title = "Universitățile testează platforme AI pentru predare și evaluare",
                    Content = "Mai multe universități europene analizează modul în care instrumentele bazate pe inteligență artificială pot sprijini activitatea didactică...",
                    PublishedAt = new DateTime(2026, 3, 10),
                    CategoryId = technology.Id
                },
                new Article
                {
                    Title = "Noi generații de procesoare promit eficiență energetică mai bună",
                    Content = "Producătorii de hardware au prezentat în ultimele luni noi arhitecturi...",
                    PublishedAt = new DateTime(2026, 3, 12),
                    CategoryId = technology.Id
                },
                new Article
                {
                    Title = "Companiile investesc în centre de date optimizate pentru sarcini AI",
                    Content = "Interesul crescut pentru modele de inteligență artificială...",
                    PublishedAt = new DateTime(2026, 3, 16),
                    CategoryId = technology.Id
                },
                new Article
                {
                    Title = "Start de sezon în Formula 1, cu accent pe noile pachete tehnice",
                    Content = "Echipele au prezentat noile monoposturi...",
                    PublishedAt = new DateTime(2026, 3, 15),
                    CategoryId = sport.Id
                },
                new Article
                {
                    Title = "Turneu internațional de tenis aduce la start jucători din topul mondial",
                    Content = "Competiția reunește sportivi...",
                    PublishedAt = new DateTime(2026, 3, 11),
                    CategoryId = sport.Id
                },
                new Article
                {
                    Title = "Cluburile europene își pregătesc loturile pentru fazele decisive ale sezonului",
                    Content = "În competițiile continentale...",
                    PublishedAt = new DateTime(2026, 3, 18),
                    CategoryId = sport.Id
                },
                new Article
                {
                    Title = "Festivalul de film european aduce proiecții speciale și dezbateri",
                    Content = "Ediția din acest an include...",
                    PublishedAt = new DateTime(2026, 3, 9),
                    CategoryId = culture.Id
                },
                new Article
                {
                    Title = "Muzeele extind programele educaționale pentru publicul tânăr",
                    Content = "Tot mai multe instituții culturale...",
                    PublishedAt = new DateTime(2026, 3, 14),
                    CategoryId = culture.Id
                },
                new Article
                {
                    Title = "Expoziție de artă contemporană explorează relația dintre tehnologie și memorie",
                    Content = "Noua expoziție reunește lucrări multimedia...",
                    PublishedAt = new DateTime(2026, 3, 17),
                    CategoryId = culture.Id
                },
                new Article
                {
                    Title = "A murit Chuck Norris...",
                    Content = "Lumea cinematografiei este în doliu...",
                    PublishedAt = new DateTime(2026, 3, 20),
                    ImagePath = "/images/chuck-norris-in-the-expendables-2.png",
                    CategoryId = actualitate.Id
                },
                new Article
                {
                    Title = "Criza carburanților...",
                    Content = "Prețurile carburanților au crescut...",
                    PublishedAt = new DateTime(2026, 3, 21),
                    ImagePath = "/images/alimentare_pompa_benzina.jpg",
                    CategoryId = actualitate.Id
                },
                new Article
                {
                    Title = "Război în Orientul Mijlociu...",
                    Content = "Președintele american Donald Trump...",
                    PublishedAt = new DateTime(2026, 3, 21),
                    ImagePath = "/images/trump.png",
                    CategoryId = actualitate.Id
                },
                new Article
                {
                    Title = "Clasamentul reciclării din București și Ilfov",
                    Content = "Autoritatea Națională pentru Protecția Mediului...",
                    PublishedAt = new DateTime(2026, 3, 21),
                    ImagePath = "/images/clasamentul_reciclarii.png",
                    CategoryId = actualitate.Id
                }
            );

            await context.SaveChangesAsync();
        }
    }
}