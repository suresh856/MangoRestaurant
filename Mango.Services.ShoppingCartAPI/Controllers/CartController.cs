using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Messages;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMessageBus _messageBus;
        protected ResponseDto _responseDto;
        public CartController(ICartRepository cartRepository,IMessageBus messageBus)
        {
            this._cartRepository = cartRepository;
            this._messageBus = messageBus;
            _responseDto = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(userId);
                _responseDto.Result = cartDto;
                _responseDto.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }


        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDtoNew = await _cartRepository.CreateUpdateCart(cartDto);
                _responseDto.Result = cartDtoNew;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }


        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDtoNew = await _cartRepository.CreateUpdateCart(cartDto);
                _responseDto.Result = cartDtoNew;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost("RemoveCart")]
        public async Task<IActionResult> UpdateCart([FromBody] int cartId)
        {
            try
            {
                bool result = await _cartRepository.RemoveFromCart(cartId);
                _responseDto.Result = result;
                _responseDto.IsSuccess = result;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }

        [HttpPost("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                bool result = await _cartRepository.RemoveCoupon(userId);
                _responseDto.Result = result;
                _responseDto.IsSuccess = result;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }


        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] CartDto cartDto)
        {
            try
            {
                bool result = await _cartRepository.ApplyCoupon(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
                _responseDto.Result = result;
                _responseDto.IsSuccess = result;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }



        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(CheckoutHeaderDto checkoutHeaderDto)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(checkoutHeaderDto.UserId);
                if(cartDto==null)
                {
                    return BadRequest();
                }
                checkoutHeaderDto.CartDetails = cartDto.CartDetails;

                //logic to add message to process order
                await _messageBus.PublishMessage(checkoutHeaderDto, "checkoutmessagetopic");

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }










    }
}
