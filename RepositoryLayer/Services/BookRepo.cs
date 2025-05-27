using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using RepositoryLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Services
{
    public class BookRepo : IBookRepo
    {
        private readonly IConfiguration _configuration;
        private readonly BookstoreDBContext _dbContext;
        private readonly TokenService _tokenService;
        public BookRepo(IConfiguration _configuration, BookstoreDBContext _dbContext, TokenService _tokenService)
        {
            this._dbContext = _dbContext;
            this._configuration = _configuration;
            this._tokenService = _tokenService;

        }

        public async Task UploadBooksFromCsv(IFormFile file, int AdminId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Invalid file.");

                var books = new List<BookEntity>();

                using var reader = new StreamReader(file.OpenReadStream());
                string line;
                int lineNumber = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lineNumber++;
                    if (lineNumber == 1) continue;

                    var columns = line.Split(',');

                    if (columns.Length < 4) continue;

                    books.Add(new BookEntity
                    {
                        BookName = columns[1],
                        Author = columns[2],
                        Description = columns[3],
                        Price = decimal.TryParse(columns[4], out var price) ? price : 0,
                        DiscountPrice = decimal.TryParse(columns[5], out var discountPrice) ? discountPrice : 0,
                        Quantity = int.TryParse(columns[6], out var quantity) ? quantity : 0,
                        BookImage = columns[7],
                        AdminId = AdminId,
                        CreatedAt = DateTime.TryParse(columns[9], out var parsedDate) ? parsedDate : DateTime.MinValue,
                        UpdatedAt = DateTime.TryParse(columns[10], out var parsedDate2) ? parsedDate2 : DateTime.MinValue,

                    });
                }

                await _dbContext.Books.AddRangeAsync(books);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload {ex.Message}");
            }
        }

        public async Task<bool> AddBook(int AdminId, BookModel model)
        {
            try
            {
                var book = new BookEntity()
                {
                    Description = model.Description,
                    DiscountPrice = model.DiscountPrice,
                    BookImage = model.BookImage,
                    AdminId = AdminId,
                    BookName = model.BookName,
                    Author = model.Author,
                    Quantity = model.Quantity,
                    Price = model.Price,
                    CreatedAt = model.CreatedAt,
                    UpdatedAt = model.UpdatedAt
                };
                _dbContext.Books.Add(book);
                _dbContext.SaveChangesAsync();
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Add Book {ex.Message}");
            }
        }

        public BookEntity UpdateBook(int bookId, BookModel model)
        {
            try
            {

                var book = _dbContext.Books.FirstOrDefault(b => b.BookId == bookId);
                if (book == null)
                    throw new Exception("Book not found.");

                book.BookName = model.BookName;
                book.Author = model.Author;
                book.Description = model.Description;
                book.Price = model.Price;
                book.DiscountPrice = model.DiscountPrice;
                book.Quantity = model.Quantity;
                book.BookImage = model.BookImage;
                book.UpdatedAt = DateTime.Now;

                _dbContext.SaveChanges();
                return book;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating book: {ex.Message}");
            }
        }

        public List<BookEntity> GetAllBooks()
        {
            try
            {
                List<BookEntity> books = new List<BookEntity>();
                books = _dbContext.Books.ToList();
                return books;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while Fetching book: {ex.Message}");
            }
        }

        public BookEntity GetBookById(int bookId)
        {
            try
            {
                BookEntity book = new BookEntity();
                book = _dbContext.Books.FirstOrDefault(u => u.BookId == bookId);
                return book;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while Fetching book by Id: {ex.Message}");
            }
        }

        public string DeleteBook(int bookId, int adminId)
        {
            try
            {
                var book = _dbContext.Books.FirstOrDefault(b => b.BookId == bookId && b.AdminId == adminId);
                if (book != null)
                {
                    _dbContext.Books.Remove(book);
                    _dbContext.SaveChanges();
                    return "Book deleted successfully";
                }
                else
                {
                    return "Book not Found";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting book: {ex.Message}");
            }

        }

        public async Task<List<BookEntity>> GetBooksByPriceAscending()
        {
            try
            {
                var books = await _dbContext.Books.OrderBy(b => b.Price).ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching books by price in ascending order: {ex.Message}");
            }
        }


        public async Task<List<BookEntity>> SortBookByPriceByDescending()
        {
            try
            {
                var books = await _dbContext.Books.OrderByDescending(b => b.Price).ToListAsync();

                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching books by price in ascending order: {ex.Message}");
            }
        }

        public async Task<List<BookEntity>> SearchBookByName(string bookName)
        {
            try
            {
                var books = await _dbContext.Books
                    .Where(b => b.BookName.Contains(bookName))
                    .ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while searching books by name: {ex.Message}");
            }
        }

        public async Task<List<BookEntity>> SearchBookByAuthor(string authorName)
        {
            try
            {
                var books = await _dbContext.Books
                    .Where(b => b.Author.Contains(authorName))
                    .ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while searching books by author: {ex.Message}");
            }
        }

        public async Task<List<BookEntity>> GetMostRecentBook()
        {
            try
            {
                var books = await _dbContext.Books
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(5)
                    .ToListAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching most recent books: {ex.Message}");
            }
        }

        public async Task<PaginationModel<BookEntity>> GetBooksPaginatedAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalBooks = await _dbContext.Books.CountAsync();

                var books = await _dbContext.Books
                    .OrderByDescending(b => b.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginationModel<BookEntity>
                {
                    TotalCount = totalBooks,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Data = books
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching paginated books: {ex.Message}");
            }
        }


    }
}
