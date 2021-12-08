﻿using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext applicationDbContext,IMapper mapper)
        {
            this._context = applicationDbContext;
            this._mapper = mapper;
        }
        public async Task<bool> ClearCart(string userId)
        {
            try
            {
                var cartHeaderFromDb = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);
                if (cartHeaderFromDb != null)
                {
                    _context.CartDetails.RemoveRange(_context.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                    _context.CartHeaders.Remove(cartHeaderFromDb);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            try
            {
                Cart cart = _mapper.Map<Cart>(cartDto);

                //check if product exists in database.if not create it
                var productInDb = await _context.Products.FirstOrDefaultAsync(
                    u => u.ProductId == cartDto.CartDetailsDto.FirstOrDefault().ProductId);

                if (productInDb == null)
                {
                    _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                    await _context.SaveChangesAsync();
                }


                var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
                //If header is null.create header and details
                if (cartHeaderFromDb == null)
                {
                    _context.CartHeaders.Add(cart.CartHeader);
                    await _context.SaveChangesAsync();
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else 
                {

                    //if header is not null.check if details has same product
                    var cartDeatilsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                        u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                        u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    //if not then create details
                    if (cartDeatilsFromDb==null)
                    {
                        //create details
                        cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        cart.CartDetails.FirstOrDefault().Product = null;
                        _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                        await _context.SaveChangesAsync();
                    }
                    //if yes then update the count
                    else
                    {
                        cart.CartDetails.FirstOrDefault().Count += cartDeatilsFromDb.Count;
                        cart.CartDetails.FirstOrDefault().Product = null;
                        _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                        await _context.SaveChangesAsync();
                    }

                 
                
                }
                return _mapper.Map<CartDto>(cart);
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            try
            {
                Cart cart = new()
                {
                    CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId),
                };
                cart.CartDetails = _context.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).Include(u => u.Product);

                return _mapper.Map<CartDto>(cart);
                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {

                CartDetails cartDetails = await _context.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = _context.CartDetails.Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _context.CartDetails.Remove(cartDetails);
                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
