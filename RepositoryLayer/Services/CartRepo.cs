using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CartRepo : ICartRepo
    {
        private readonly BookstoreDBContext _dbcontext;

        public CartRepo(BookstoreDBContext context)
        {
            _dbcontext = context;
        }

        public CartEntity getCartById(int cartId)
        {
            try
            {
                CartEntity cartEntity = _dbcontext.Cart.Include(x => x.CartItems).ThenInclude(x => x.Book).First(c => c.CartId == cartId);

                if (cartEntity != null)
                {
                    return cartEntity;
                }
                else
                {
                    throw new Exception($"No Cart Found by Id: {cartId}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error while Fetching Cart by Id: {cartId} with Error: {ex.Message}");
            }
        }

        public CartEntity save(CartEntity cart)
        {
            EntityEntry<CartEntity> entryEntity = _dbcontext.Cart.Add(cart);

            _dbcontext.SaveChanges();

            return getCartById(entryEntity.Entity.CartId);
        }

        public ICollection<CartEntity> getAllCarts()
        {
            try
            {
                return _dbcontext.Cart
                    .Include(x => x.CartItems)
                    .ThenInclude(x => x.Book)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while fetching all carts: {ex.Message}");
            }
        }

        public CartEntity getCartByUserId(int userId)
        {
            return _dbcontext.Cart
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefault(c => c.UserId == userId);
        }

        //public CartEntity getCartByIdAndUserId(int cartId, int userId)
        //{
        //    try
        //    {
        //        CartEntity cartEntity = _dbcontext.Cart.Include(x => x.CartItems).ThenInclude(x => x.Book).FirstOrDefault(c => c.CartId == cartId && c.UserId == userId);

        //        if (cartEntity != null)
        //        {
        //            return cartEntity;
        //        }
        //        else
        //        {
        //            throw new Exception($"No Cart Found by Id: {cartId}");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error while Fetching Cart by Id: {cartId} with Error: {ex.Message}");
        //    }
        //}
    }
}
