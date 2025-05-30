using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRepo
    {
        public CartEntity getCartById(int id);

        public CartEntity save(CartEntity entity);

        public ICollection<CartEntity> getAllCarts();

        public CartEntity getCartByUserId(int userId);

    }
}
