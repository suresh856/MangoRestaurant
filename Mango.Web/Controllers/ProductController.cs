﻿using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new List<ProductDto>();
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.GetAllProductsAsync<ResponseDto>(accessToken);
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                }
                return View(list);
            }
            catch(Exception )
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");

                    var response = await _productService.CreateProductAsync<ResponseDto>(accessToken, productDto);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(ProductIndex));
                    }
                }
                return View(productDto);               
            }
            catch (Exception )
            {
                return View("Error");
            }
        }



        public async Task<IActionResult> ProductEdit(int productId)
        {
            ProductDto productDto = new ProductDto();
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.GetProductByIdAsync<ResponseDto>(accessToken, productId);
                if (response != null && response.IsSuccess)
                {
                    productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                    return View(productDto);
                }
                return NotFound();
                
            }
            catch (Exception )
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");

                    var response = await _productService.UpdateProductAsync<ResponseDto>(accessToken, productDto);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(ProductIndex));
                    }
                }
                return View(productDto);
            }
            catch (Exception )
            {
                return View("Error");
            }
        }


        public async Task<IActionResult> ProductDelete(int productId)
        {
            ProductDto productDto = new ProductDto();
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var response = await _productService.GetProductByIdAsync<ResponseDto>(accessToken, productId);
                if (response != null && response.IsSuccess)
                {
                    productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                    return View(productDto);
                }
                return NotFound();

            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var accessToken = await HttpContext.GetTokenAsync("access_token");

                    var response = await _productService.DeleteProductAsync<ResponseDto>(accessToken,productDto.ProductId);
                    if (response != null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(ProductIndex));
                    }
                }
                return View(productDto);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }



    }
}
