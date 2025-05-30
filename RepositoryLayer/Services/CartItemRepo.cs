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
    public class CartItemRepo : ICartItemRepo
    {
        private readonly BookstoreDBContext _dbcontext;
        private readonly ICartRepo cartRepo;

        public CartItemRepo(BookstoreDBContext context, ICartRepo cartRepo)
        {
            this._dbcontext = context;
            this.cartRepo = cartRepo;
        }

        public CartItemEntity save(CartItemEntity entity)
        {
            EntityEntry<CartItemEntity> s = _dbcontext.CartItems.Add(entity);
            
            _dbcontext.SaveChanges();
            return _dbcontext.CartItems
                .Include(x => x.Book)
                .Where(c => c.CartItemId == s.Entity.CartItemId)
                .First();
        }

        public ICollection<CartItemEntity> getByCartId(int cartId)
        {
            return _dbcontext.CartItems
                .Include(x => x.Book)
                .Where(cartItem =>  cartItem.CartId == cartId)
                .ToList();
        }

        public void deleteById(int cartItemId)
        {
            CartItemEntity cartItemEntity = _dbcontext.CartItems
                .Where(x => x.CartItemId == cartItemId)
                .FirstOrDefault();

            if (cartItemEntity == null)
            {
                throw new Exception("Specified Cart Item does not exist in the cart");
            }

            _dbcontext.Remove(cartItemEntity);
            _dbcontext.SaveChanges();
        }

        public void deleteByCartId(int cartId)
        {
            ICollection<CartItemEntity> cartItemEntities = _dbcontext.CartItems
                .Where(x => x.CartId == cartId)
                .ToList();

            if (cartItemEntities == null || cartItemEntities.Count == 0)
            {
                throw new Exception("No Cart items exist in the cart");
            }

            _dbcontext.Remove(cartItemEntities);
            _dbcontext.SaveChanges();
        }
    }
}
