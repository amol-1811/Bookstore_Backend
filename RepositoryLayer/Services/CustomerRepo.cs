using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly BookstoreDBContext context;

        public CustomerRepo(BookstoreDBContext bookStoreContext)
        {
            this.context = bookStoreContext;
        }

        public CustomerEntity AddCustomerDetails(int userId, CustomerDetailModel model)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var customer = new CustomerEntity
                {
                    FullName = model.FullName,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    Type = model.Type,
                    UserId = model.UserId
                };

                context.Customer.Add(customer);
                context.SaveChanges();
                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"error in customer registration : {ex.Message}");
            }
        }

        public List<CustomerResponseModel> GetAllCustomerDetails()
        {
            try
            {
                var customers = context.Customer
                    .Select(c => new CustomerResponseModel
                    {
                        CustomerId = c.CustomerId,
                        UserId = c.UserId,
                        FullName = c.FullName,
                        Mobile = c.Mobile,
                        Address = c.Address,
                        City = c.City,
                        State = c.State,
                        Type = c.Type
                    })
                    .ToList();
                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting customer details: {ex.Message}");
            }
        }

    }
}
