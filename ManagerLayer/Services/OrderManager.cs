using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepo _orderRepo;

        public OrderManager(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public List<OrdersWithBookDetails> PlaceOrderFromCart(int userId)
        {
            return _orderRepo.PlaceOrderFromCart(userId);
        }
        public List<OrdersWithBookDetails> GetOrderDetails(int userId)
        {
            return _orderRepo.GetOrderDetails(userId);
        }
    }
}
