using BookStore.Models.Configuration.Interfaces;
using BookStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Configuration.Initializers
{
    public class BooksInitializer : ITypeInitializer
    {
        public async Task Init(ApplicationContext context)
        {
            Book[] books = new Book[]
            {
                new Book { Name = "Harry Potter",
                           Author = "J. K. Rowling",
                           Category = context.Categories.Where(el => el.Name == "Fantasy").FirstOrDefault(),
                           Year = 2012
                },
                new Book { Name = "The Chronicles of Narnia",
                           Author = "Andrew Adamson",
                           Category = context.Categories.Where(el => el.Name == "Fantasy").FirstOrDefault(),
                           Year = 2005
                },
                new Book { Name = "To Kill a Mockingbird",
                           Author = "Harper Lee",
                           Category = context.Categories.Where(el => el.Name == "Drama").FirstOrDefault(),
                           Year = 2015
                },
                new Book { Name = "Things Fall Apart",
                           Author = "Chinua Achebe",
                           Category = context.Categories.Where(el => el.Name == "Detective").FirstOrDefault(),
                           Year = 2013
                }
            };

            await context.Set<Book>().AddRangeAsync(books);
        }
    }
}

