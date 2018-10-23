using LibraryManagement.Data.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder app)
        {

    

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();


                // Add Customers
                var justin = new Customer { Name = "Justin Noon", Cedula = "042-14447771-1244", Edad="45" };

                var willie = new Customer { Name = "Willie Parodi", Cedula = "042-1434771-1366", Edad = "55" };

                var leoma = new Customer { Name = "Leoma  Gosse" ,Cedula = "042-1664771-1244", Edad = "48" };

                context.Customers.Add(justin);
                context.Customers.Add(willie);
                context.Customers.Add(leoma);

                // Add Author
                var authorDeMarco = new Author
                {
                    Name = "M J DeMarco",
                    Books = new List<Book>()
                {
                    new Book { Title = "The Millionaire Fastlane", Precio= 400.0 },
                    new Book { Title = "Unscripted", Precio= 360.0  }
                }
                };

                var authorCardone = new Author
                {
                    Name = "Grant Cardone",
                    Books = new List<Book>()
                {
                    new Book { Title = "The 10X Rule", Precio= 200.0 },
                    new Book { Title = "If You're Not First, You're Last", Precio= 250.0 },
                    new Book { Title = "Sell To Survive", Precio= 150.0 }
                }
                };

                context.Authors.Add(authorDeMarco);
                context.Authors.Add(authorCardone);

                context.SaveChanges();
            }
        }
    }
}
