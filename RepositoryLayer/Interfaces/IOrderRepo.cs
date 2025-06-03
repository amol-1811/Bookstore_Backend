using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IOrderRepo
    {
        public List<OrdersWithBookDetails> PlaceOrderFromCart(int userId);
        public List<OrdersWithBookDetails> GetOrderDetails(int userId);
    }
}
