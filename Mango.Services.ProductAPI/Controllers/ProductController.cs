using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        protected ResponseDto _response;
        public ProductController(IProductRepository repository)
        {
            this._repository = repository;
            this._response = new ResponseDto();
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await _repository.GetProducts();
                _response.Result = productDtos;
                _response.IsSuccess = true;
                _response.ErrorMessages = null; 
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                ProductDto productDtos = await _repository.GetProductById(id);
                if(productDtos == null)
                {
                    _response.Result = null;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() {"No product with given id present" };
                    return NotFound(_response);
                }
                _response.Result = productDtos;
                _response.IsSuccess = true;
                _response.ErrorMessages = null;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

    
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            ProductDto product = null;
            try
            {
                 product = await _repository.CreateUpdateProduct(productDto);
                _response.IsSuccess = true;
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
            return CreatedAtAction(nameof(Get),new { id = product.ProductId  }, _response);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductDto productDto)
        {
            ProductDto product = null;
            try
            {
                product = await _repository.CreateUpdateProduct(productDto);
                _response.IsSuccess = true;
                _response.Result = product;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
            return Ok( _response);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _repository.DeleteProduct(id);
                _response.IsSuccess = true;
                _response.Result = result;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
            return Ok(_response);
        }

    }
}
