using Mango.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Services.IServices
{
    public interface IProductService:IBaseService
    {
        Task<T> GetAllProductsAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(string token, int id);
        Task<T> CreateProductAsync<T>(string token, ProductDto productDto);
        Task<T> UpdateProductAsync<T>(string token, ProductDto productDto);
        Task<T> DeleteProductAsync<T>(string token, int id);
    }
}
