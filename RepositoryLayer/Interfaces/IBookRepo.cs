using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using RepositoryLayer.Models;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interfaces
{
    public interface IBookRepo
    {
        public Task UploadBooksFromCsv(IFormFile file, int AdminId);
        public Task<bool> AddBook(int AdminId, BookModel model);
        public BookEntity UpdateBook(int bookId, BookModel model);
        public BookEntity GetBookById(int bookId);
        public List<BookEntity> GetAllBooks();
        public string DeleteBook(int bookId, int adminId);
        public Task<List<BookEntity>> SortBookByPriceByDescending();
        public Task<List<BookEntity>> GetBooksByPriceAscending();
        public Task<List<BookEntity>> SearchBookByAuthor(string authorName);
        public Task<List<BookEntity>> SearchBookByName(string bookName);
        public Task<List<BookEntity>> GetMostRecentBook();
        public Task<PaginationModel<BookEntity>> GetBooksPaginatedAsync(int pageNumber, int pageSize);
    }
}
