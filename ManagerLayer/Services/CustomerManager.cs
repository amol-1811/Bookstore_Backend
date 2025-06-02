using System;
using System.Collections.Generic;
using System.Text;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerManager(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public CustomerEntity AddCustomerDetails(int userId, CustomerDetailModel model)
        {
            return _customerRepo.AddCustomerDetails(userId, model);
        }

        public List<CustomerResponseModel> GetAllCustomerDetails()
        {
            return _customerRepo.GetAllCustomerDetails();
        }
    }
}
