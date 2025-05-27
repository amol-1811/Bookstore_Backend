using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using RepositoryLayer.Models;

namespace Bookstore.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookManager _bookManager;
        public BookController(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }

        [HttpPost]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "Admin")
                {
                    await _bookManager.UploadBooksFromCsv(file, userId);

                    return Ok("Books uploaded successfully.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied: Only admin can upload file to add books."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the book.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("addbook")]
        public async Task<IActionResult> CreateBook(BookModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "Admin")
                {
                    bool result = await _bookManager.AddBook(userId, model);
                    if (result)
                    {
                        return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Book Added Successfully", Data = result });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<bool>
                        {
                            IsSuccess = false,
                            Message = "Failed to add book due to internal error."
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied: Only admin can add books."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the book.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("{bookId:int}")]
        public IActionResult UpdateBook(int bookId, BookModel model)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "Admin")
                {
                    var result = _bookManager.UpdateBook(bookId, model);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<BookEntity> { IsSuccess = true, Message = "Book Updated Successfully", Data = result });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { IsSuccess = false, Message = $"Book with ID {bookId} not found." });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool> { IsSuccess = false, Message = "Access denied: Only admins can update books."});
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string> { IsSuccess = false, Message = "An unexpected error occurred.", Data = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                var books = _bookManager.GetAllBooks();
                if (books != null && (role == "Admin" || role == "User"))
                {
                    return Ok(books);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied: Only admin or user can view books."
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving books: {ex.Message}");
            }
        }

        [HttpGet("{bookId:int}")]
        public IActionResult GetBookById(int bookId)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                BookEntity book = _bookManager.GetBookById(bookId);
                if (book != null && (role == "Admin" || role == "User"))
                {
                    return Ok(book);
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new ResponseModel<bool>
                        {
                            IsSuccess = false,
                            Message = "Access denied: Only admin or user can view book details."
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while retrieving books by id: {ex.Message}");
            }
        }


        [HttpDelete("{bookId}")]

        public IActionResult DeleteBook(int bookId)
        {
            try
            {
                int adminId = int.Parse(User.FindFirst("UserId").Value);
                string role = User.FindFirst("Role")?.Value;
                if (role != null && role == "Admin")
                {
                    var result = _bookManager.DeleteBook(bookId, adminId);
                    if (result != null)
                    {
                        return Ok(new ResponseModel<bool> { IsSuccess = true, Message = "Book deleted Successfully" });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool>
                        {
                            IsSuccess = false,
                            Message = $"Book with ID {bookId} not found."
                        });
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden,
                        new ResponseModel<bool>
                        {
                            IsSuccess = false,
                            Message = "Access denied: Only admins can delete books."
                        });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = "An unexpected error occurred.",
                        Data = ex.Message
                    });
            }
        }

        [HttpGet("sort-book-by-price-asc")]
        public async Task<IActionResult> SearchBooksByPriceAsync()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "Admin" || role == "User")
                {
                    var books = await _bookManager.GetBooksByPriceAscending();

                    if (books != null)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>
                        {
                            IsSuccess = true,
                            Message = "Books retrieved and sorted by price in ascending order",
                            Data = books
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { IsSuccess = false, Message = "No books found" });
                    }
                }
                else
                {
                    return StatusCode(403,
                        new ResponseModel<bool>
                        {
                            IsSuccess = false,
                            Message = "Access denied. Only admin or user can view books"
                        });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = $"Server error: {ex.Message}",
                        Data = null
                    });
            }
        }

        [HttpGet("sort-book-by-price-desc")]
        public async Task<IActionResult> SortBooksByPriceDescending()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;

                if (role == "Admin" || role == "User")
                {
                    var books = await _bookManager.SortBookByPriceByDescending();

                    if (books != null)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>
                        {
                            IsSuccess = true,
                            Message = "Books retrieved and sorted by price in Descending order",
                            Data = books
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { IsSuccess = false, Message = "No books found" });
                    }
                }
                else
                {
                    return StatusCode(403, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied. Only admin or user can view books"
                    });

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    new ResponseModel<string>
                    {
                        IsSuccess = false,
                        Message = $"Server error: {ex.Message}",
                        Data = null
                    });
            }
        }

        [HttpGet("search-by-author")]
        public async Task<IActionResult> SearchBooksByAuthor(string authorName)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role == "Admin" || role == "User")
                {
                    var books = await _bookManager.SearchBookByAuthor(authorName);
                    if (books != null && books.Count > 0)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>
                        {
                            IsSuccess = true,
                            Message = "Books retrieved by author",
                            Data = books
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { IsSuccess = false, Message = "No books found for the specified author" });
                    }
                }
                else
                {
                    return StatusCode(403, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied. Only admin or user can view books"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = $"Server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("search-by-bookname")]
        public async Task<IActionResult> SearchBookByName(string bookName)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role == "Admin" || role == "User")
                {
                    var books = await _bookManager.SearchBookByName(bookName);
                    if (books != null && books.Count > 0)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>
                        {
                            IsSuccess = true,
                            Message = "Books retrieved by author",
                            Data = books
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { IsSuccess = false, Message = "No books found for the specified author" });
                    }
                }
                else
                {
                    return StatusCode(403, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied. Only admin or user can view books"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = $"Server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("recent-book")]
        public async Task<IActionResult> GetMostRecentBook()
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role == "Admin" || role == "User")
                {
                    var books = await _bookManager.GetMostRecentBook();
                    if (books != null && books.Count > 0)
                    {
                        return Ok(new ResponseModel<List<BookEntity>>
                        {
                            IsSuccess = true,
                            Message = "Books retrieved by author",
                            Data = books
                        });
                    }
                    else
                    {
                        return NotFound(new ResponseModel<bool> { IsSuccess = false, Message = "No books found for the specified author" });
                    }
                }
                else
                {
                    return StatusCode(403, new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "Access denied. Only admin or user can view books"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = $"Server error: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("books/paginated")]
        public async Task<IActionResult> GetPaginatedBooks(int pageNumber = 1, int pageSize = 8)
        {
            try
            {
                string role = User.FindFirst("Role")?.Value;
                if (role == "Admin" || role == "User")
                {
                    var response = await _bookManager.GetBooksPaginatedAsync(pageNumber, pageSize);

                    if (response.Data != null && response.Data.Count > 0)
                    {
                        return Ok(new ResponseModel<PaginationModel<BookEntity>>
                        {
                            IsSuccess = true,
                            Message = "Books retrieved successfully",
                            Data = response
                        });
                    }

                    return NotFound(new ResponseModel<bool>
                    {
                        IsSuccess = false,
                        Message = "No books found"
                    });
                }

                return StatusCode(403, new ResponseModel<bool>
                {
                    IsSuccess = false,
                    Message = "Access denied. Only admin or user can view books"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel<string>
                {
                    IsSuccess = false,
                    Message = $"Server error: {ex.Message}"
                });
            }
        }

    }
}
