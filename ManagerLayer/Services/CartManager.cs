using System;
using System.Collections.Generic;
using System.Linq;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using static RepositoryLayer.Models.UpdatecartModel;

namespace ManagerLayer.Services
{
    public class CartManager : ICartManager
    {

        private readonly ICartRepo cartRepo;
        private readonly IBookRepo bookRepo;
        private readonly ICartItemRepo cartItemRepo;

        public CartManager(ICartRepo cartRepo, IBookRepo bookRepo, ICartItemRepo cartItemRepo)
        {
            this.cartRepo = cartRepo;
            this.bookRepo = bookRepo;
            this.cartItemRepo = cartItemRepo;
        }
        
        private CartModel map(CartEntity cartEntity)
        {
            try
            {
                CartModel model = new CartModel();

                model.Id = cartEntity.CartId;

                ICollection<CartItemModel> cartItemModelList = new List<CartItemModel>();

                for (int i = 0; i < cartEntity.CartItems?.Count; i++)
                {
                    CartItemEntity cartItemEntity = cartEntity.CartItems.ElementAt(i);
                    CartItemModel cartItemModel = new CartItemModel();

                    cartItemModel.CartItemId = cartItemEntity.CartItemId;
                    cartItemModel.Quantity = cartItemEntity.Quantity;
                    cartItemModel.Price = cartItemEntity.Price;

                    if (cartItemEntity.Book != null)
                    {
                        BookModel bookModel = new BookModel();
                        bookModel.BookId = cartItemEntity.Book.BookId;
                        bookModel.Author = cartItemEntity.Book.Author;
                        bookModel.Price = cartItemEntity.Book.Price;
                        bookModel.BookImage = cartItemEntity.Book.BookImage;
                        bookModel.Description = cartItemEntity.Book.Description;
                        bookModel.BookName = cartItemEntity.Book.BookName;
                        bookModel.CreatedAt = cartItemEntity.Book.CreatedAt;        
                        bookModel.UpdatedAt = cartItemEntity.Book.UpdatedAt;

                        cartItemModel.BookModel = bookModel;
                    }

                    cartItemModelList.Add(cartItemModel);
                }
                model.CartItems = cartItemModelList;

                model.Total = cartItemModelList?.Sum(cartModel => cartModel.Quantity * cartModel.Price) ?? 0;

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception($"failed to transform entity to model {ex.Message}");
            }
        }

        public CartModel createCart(int userId)
        {
            try
            {
                CartEntity entity = cartRepo.getCartByUserId(userId);

                if (entity != null) throw new Exception("The active cart is already associated with the logged in user");

                entity = new CartEntity();

                entity.UserId = userId;

                return map(cartRepo.save(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public CartModel getCartByUserId(int userId)
        {
            try
            {
               return map(cartRepo.getCartByUserId(userId));
               
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while Fetching Cart by useId: {ex.Message}");
            }

        }

        public CartModel addToCart(int userId, AddToCartModel addToCartModel)
        {
            try
            {
                CartEntity entity = cartRepo.getCartByUserId(userId);
                if (entity == null)
                {
                    throw new Exception($"Failed to add item to Cart because Cart does not exist for user: {userId}");
                }

                BookEntity bookEntity = bookRepo.GetBookById(addToCartModel.BookId);

                if (bookEntity != null)
                {
                    var existingCartItem = entity.CartItems.FirstOrDefault(ci => ci.BookId == addToCartModel.BookId);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += 1;
                        existingCartItem.Price = bookEntity.Price * existingCartItem.Quantity;
                        cartItemRepo.update(existingCartItem);
                    }
                    else
                    {
                        CartItemEntity cartItemEntity = new CartItemEntity();
                        cartItemEntity.Quantity = 1;
                        cartItemEntity.Price = bookEntity.Price * 1;
                        cartItemEntity.CartId = entity.CartId;
                        cartItemEntity.BookId = bookEntity.BookId;
                        cartItemEntity.IsPurchased = false;

                        entity.CartItems.Add(cartItemEntity);
                        cartItemRepo.save(cartItemEntity);
                    }
                }
                else
                {
                    throw new Exception($"Requested Books [ID: {addToCartModel.BookId}] does not exist");
                }

                return map(cartRepo.getCartByUserId(userId));

            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add item in cart => {e.Message}");
            }
        }

        public CartModel updateCart(int userId, UpdateCartModel updateCartModel)
        {
            try
            {
                CartEntity entity = cartRepo.getCartByUserId(userId);
                if (entity == null)
                {
                    throw new Exception($"Failed to update cart because Cart does not exist for user: {userId}");
                }

                BookEntity bookEntity = bookRepo.GetBookById(updateCartModel.BookId);

                if (bookEntity != null)
                {
                    var existingCartItem = entity.CartItems.FirstOrDefault(ci => ci.BookId == updateCartModel.BookId);

                    if (existingCartItem != null)
                    {
                        existingCartItem.Quantity += updateCartModel.Quantity;
                        existingCartItem.Price = bookEntity.Price * updateCartModel.Quantity;
                        cartItemRepo.update(existingCartItem);
                    }
                    else
                    {
                        CartItemEntity cartItemEntity = new CartItemEntity();
                        cartItemEntity.Quantity = updateCartModel.Quantity;
                        cartItemEntity.Price = bookEntity.Price * updateCartModel.Quantity;
                        cartItemEntity.CartId = entity.CartId;
                        cartItemEntity.BookId = bookEntity.BookId;
                        cartItemEntity.IsPurchased = false;

                        entity.CartItems.Add(cartItemEntity);
                        cartItemRepo.save(cartItemEntity);
                    }
                }
                else
                {
                    throw new Exception($"Requested Book [ID: {updateCartModel.BookId}] does not exist");
                }

                return map(cartRepo.getCartByUserId(userId));

            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update cart => {e.Message}");
            }
        }

        public CartModel removeFromCart(int userId, int cartItemId)
        {
            CartEntity cartEntity = cartRepo.getCartByUserId(userId);
            if (cartEntity != null)
            {
                if (isCartItemPresent(cartEntity.CartItems, cartItemId))
                {
                    cartItemRepo.deleteById(cartItemId);
                    return map(cartRepo.getCartByUserId(userId));
                }
                else
                {
                    throw new Exception($"No CartItem present with an ID {cartItemId}");
                }
            }
            else
            {
                throw new Exception("There is no cart associated with the User");
            }

            return map(cartRepo.getCartByUserId(userId));

        }

        private bool isCartItemPresent(ICollection<CartItemEntity> cartItems, int cartItemId)
        {
            return cartItems.Where(cartItem => cartItem.CartItemId ==  cartItemId).Any();
        }

        public ICollection<CartModel> getAllCarts()
        {
            try
            {
                ICollection<CartEntity> allCarts = cartRepo.getAllCarts();
                ICollection<CartModel> allCartList = new List<CartModel>();

                foreach (var cart in allCarts)
                {
                    allCartList.Add(map(cart));
                }

                return allCartList;

            } catch (Exception ex)
            {
                throw new Exception($"error while fetchig all carts {ex.Message}");
            }
        }
    }
}
