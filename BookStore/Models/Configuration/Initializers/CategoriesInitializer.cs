using BookStore.Models.Configuration.Interfaces;
using BookStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Configuration.Initializers
{

    public class CategoriesInitializer : ITypeInitializer
    {
        public async Task Init(ApplicationContext context)
        {
            Category[] categories = new Category[]
            {
                new Category { Name = "Adventure" },
                new Category { Name = "Fantasy" },
                new Category { Name = "Comedy" },
                new Category { Name = "Detective" },
                new Category { Name = "Drama" },
                new Category { Name = "Horror" }
            };

            await context.Set<Category>().AddRangeAsync(categories);
        }
    }
}
