using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System.Threading.Tasks;
using RepositoryLayer.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class OrderRepo : IOrderRepo
    {
        private readonly BookstoreDBContext context;
        public OrderRepo(BookstoreDBContext context)
        {
            this.context = context;
        }

        public List<OrdersWithBookDetails> PlaceOrderFromCart(int userId)
        {
            try
            {
                var cart = context.Cart
                    .Where(c => c.UserId == userId)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Book)
                    .Include(c => c.User)
                    .FirstOrDefault();

                if (cart == null || !cart.CartItems.Any())
                    return null;

                var cartItems = cart.CartItems.ToList();
                var user = cart.User;
                decimal grandTotal = cartItems.Sum(item => item.TotalPrice);
                var ordersWithDetails = new List<OrdersWithBookDetails>();

                foreach (var item in cartItems)
                {
                    var order = new OrdersEntity
                    {
                        UserId = userId,
                        BookId = item.BookId,
                        Price = item.TotalPrice,
                        OrderDate = DateTime.UtcNow
                    };

                    context.Orders.Add(order);
                    context.SaveChanges(); // Save here to get generated OrderId

                    // Retrieve the saved order with generated ID
                    order = context.Orders
                        .OrderByDescending(o => o.OrderId)
                        .FirstOrDefault(o => o.UserId == userId && o.BookId == item.BookId);

                    var orderWithBookDetails = new OrdersWithBookDetails
                    {
                        OrderId = order?.OrderId ?? 0,
                        UserId = order.UserId,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        BookId = order.BookId,
                        Price = order.Price,
                        OrderDate = order.OrderDate,
                        BookName = item.Book.BookName,
                        Author = item.Book.Author,
                        Description = item.Book.Description,
                        BookImage = item.Book.BookImage,
                        Quantity = item.Quantity,
                        TotalPrice = grandTotal,
                    };

                    ordersWithDetails.Add(orderWithBookDetails);
                }

                context.CartItems.RemoveRange(cartItems);
                context.SaveChanges();

                return ordersWithDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in order placement: {ex.Message}");
            }
        }

        public List<OrdersWithBookDetails> GetOrderDetails(int userId)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    throw new Exception($"User with ID {userId} not found");
                }

                var orders = context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.Book)
                    .Include(o => o.User)
                    .ToList();

                if (!orders.Any())
                {
                    return new List<OrdersWithBookDetails>();
                }

                decimal grandTotal = orders.Sum(o => o.Price);

                var ordersWithDetails = new List<OrdersWithBookDetails>();

                foreach (var order in orders)
                {
                    if (order.Book == null)
                    {
                        throw new Exception($"Book information not found for order {order.OrderId}");
                    }

                    var orderDetail = new OrdersWithBookDetails
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        FullName = user.FullName ?? "N/A",
                        UserEmail = user.Email ?? "N/A",
                        BookId = order.BookId,
                        Price = order.Price,
                        OrderDate = order.OrderDate,
                        BookName = order.Book.BookName ?? "N/A",
                        Author = order.Book.Author ?? "N/A",
                        Description = order.Book.Description ?? "N/A",
                        BookImage = order.Book.BookImage ?? "N/A",
                        Quantity = order.Book.Quantity,
                        TotalPrice = grandTotal
                    };

                    ordersWithDetails.Add(orderDetail);
                }

                return ordersWithDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching order details: {ex.Message}");
            }
        }

    }
}
