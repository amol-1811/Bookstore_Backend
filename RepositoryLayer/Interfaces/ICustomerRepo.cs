using System;
using System.Collections.Generic;
using System.Text;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICustomerRepo
    {
        public CustomerEntity AddCustomerDetails(int userId, CustomerDetailModel model);
        public List<CustomerResponseModel> GetAllCustomerDetails();
    }
}
