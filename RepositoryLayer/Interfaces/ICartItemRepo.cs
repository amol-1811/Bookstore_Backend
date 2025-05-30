using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICartItemRepo
    {
        public CartItemEntity save(CartItemEntity entity);

        public ICollection<CartItemEntity> getByCartId(int cartId);

        public void deleteById(int cartItemId);

        public void deleteByCartId(int cartId);
    }
}
