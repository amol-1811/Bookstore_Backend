using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace ManagerLayer.Interfaces
{
    public interface ICustomerManager
    {
        public CustomerEntity AddCustomerDetails(int userId, CustomerDetailModel model);
        public List<CustomerResponseModel> GetAllCustomerDetails();
    }
}
