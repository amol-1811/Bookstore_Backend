using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using RepositoryLayer.Services;

namespace ManagerLayer.Services
{
    public class BookManager : IBookManager
    {
        private readonly IBookRepo _bookRepo;
        public BookManager(IBookRepo bookRepo)
        {
            _bookRepo = bookRepo;
        }
        public async Task UploadBooksFromCsv(IFormFile file, int AdminId)
        {
            _bookRepo.UploadBooksFromCsv(file, AdminId);
        }

        public Task<bool> AddBook(int AdminId, BookModel model)
        {
            return _bookRepo.AddBook(AdminId, model);
        }
        public BookEntity UpdateBook(int bookId, BookModel model)
        {
            return _bookRepo.UpdateBook(bookId, model);
        }

        public BookEntity GetBookById(int bookId)
        {
            return _bookRepo.GetBookById(bookId);
        }

        public List<BookEntity> GetAllBooks()
        {
            return _bookRepo.GetAllBooks();
        }

        public string DeleteBook(int bookId, int adminId)
        {
            return _bookRepo.DeleteBook(bookId, adminId);
        }

        public Task<List<BookEntity>> SortBookByPriceByDescending()
        {
            return _bookRepo.SortBookByPriceByDescending();
        }

        public Task<List<BookEntity>> GetBooksByPriceAscending()
        {
            return _bookRepo.GetBooksByPriceAscending();
        }
        public Task<List<BookEntity>> SearchBookByAuthor(string authorName)
        {
            return _bookRepo.SearchBookByAuthor(authorName);
        }
        public Task<List<BookEntity>> SearchBookByName(string bookName)
        {
            return _bookRepo.SearchBookByName(bookName);
        }
        public Task<List<BookEntity>> GetMostRecentBook()
        {
            return _bookRepo.GetMostRecentBook();
        }
        public Task<PaginationModel<BookEntity>> GetBooksPaginatedAsync(int pageNumber, int pageSize)
        {
            return _bookRepo.GetBooksPaginatedAsync(pageNumber, pageSize);
        }
    }
}
