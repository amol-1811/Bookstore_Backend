using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Models;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IOrderManager
    {
        public List<OrdersWithBookDetails> PlaceOrderFromCart(int userId);
        public List<OrdersWithBookDetails> GetOrderDetails(int userId);
    }
}
