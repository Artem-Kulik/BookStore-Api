using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Models.Dto.ResultDto;
using BookStore.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public BookController(ApplicationContext context)
        {
            _context = context;
        }

        public ResultDto GetBooks()
        {
            var books = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Author = c.Author,
                Category = c.Category.Name,
                Year = c.Year
            }).ToList();

            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = books
            };
        }
        [HttpGet]
        [Route("sortByName")]
        public ResultDto GetBooksSortedByName()
        {
            var books = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Author = c.Author,
                Category = c.Category.Name,
                Year = c.Year
            }).OrderBy(el=>el.Name).ToList();

            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = books
            };
        }
       
        [HttpGet]
        [Route("sortByAuthor")]
        public ResultDto GetBooksSortedByAuthor()
        {
            var books = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Author = c.Author,
                Category = c.Category.Name,
                Year = c.Year
            }).OrderBy(el => el.Author).ToList();

            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = books
            };
        }
       
        [HttpGet]
        [Route("sortByYear")]
        public ResultDto GetBooksSortedByYear()
        {
            var books = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Author = c.Author,
                Category = c.Category.Name,
                Year = c.Year
            }).OrderByDescending(el => el.Year).ToList();

            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = books
            };
        }
       
        [HttpGet]
        [Route("sortByCategory")]
        public ResultDto GetBooksSortedByCategory()
        {
            var books = _context.Books.Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Author = c.Author,
                Category = c.Category.Name,
                Year = c.Year
            }).OrderBy(el => el.Category).ToList();

            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = books
            };
        }

        [HttpGet]
        [Route("getBooks/{id}")]
        public ResultDto GetBooksByCategory([FromRoute]int id)
        {
            var books = _context.Books.Where(el=>el.Category.Id == id).Select(c => new BookDto()
            {
                Id = c.Id,
                Name = c.Name,
                Author = c.Author,
                Category = c.Category.Name,
                Year = c.Year
            }).ToList();

            return new CollectionResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = books
            };
        }

        [HttpDelete]
        public ResultDto DeleteBook(int id)
        {
            try
            {
                if (id != null)
                {
                    var c = _context.Books.Find(id);
                    _context.Books.Remove(c);
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccessful = true,
                        Message = "Successfully deleted"
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        IsSuccessful = false,
                        Message = "Id is not defined"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Something goes wrong"
                };
            }

        }

        [HttpPost]
        public ResultDto AddBook([FromBody]BookDto c)
        {
            try
            {
                if (c != null)
                {
                    Book newB = new Book()
                    {
                        Name = c.Name,
                        Author = c.Author,
                        Year = c.Year
                    };
                    newB.Category = _context.Categories.Where(el => el.Name == c.Category).FirstOrDefault();
                    
                    _context.Books.Add(newB);
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccessful = true,
                        Message = "Successfully added"
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        IsSuccessful = false,
                        Message = "Model is null"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Something goes wrong"
                };
            }
        }

        [HttpPut]
        public ResultDto EditBook([FromBody]BookDto dto)
        {
            try
            {
                if (dto != null)
                {
                    Book book = _context.Books.Where(el => el.Id == dto.Id).FirstOrDefault();
                    book.Name = dto.Name;
                    book.Author = dto.Author;
                    book.Year = dto.Year;
                    book.Category = _context.Categories.Where(el => el.Name == dto.Category).FirstOrDefault();
                    _context.SaveChanges();
                    return new ResultDto
                    {
                        IsSuccessful = true,
                        Message = "Successfully edited"
                    };
                }
                else
                {
                    return new ResultDto
                    {
                        IsSuccessful = false,
                        Message = "Model is null"
                    };
                }
            }
            catch (Exception)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Something goes wrong"
                };
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ResultDto GetBook([FromRoute] int id)
        {
            var book = _context.Books.Find(id);
            BookDto res = new BookDto()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Year = book.Year
            };

            return new SingleResultDto<BookDto>
            {
                IsSuccessful = true,
                Data = res
            };
        }
    }
}