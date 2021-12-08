using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context,IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                // update
                if(product.ProductId>0)
                {
                    _context.Products.Update(product);
                }
                //create
                else
                {
                    _context.Products.Add(product);
                }
                await _context.SaveChangesAsync();
                return _mapper.Map<ProductDto>(product);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if(product==null)
                {
                    throw new Exception("No such product present in DB");
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            try
            {
                Product product = await _context.Products.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
                return _mapper.Map<ProductDto>(product);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                List<Product> products = await _context.Products.ToListAsync();
                return _mapper.Map<List<ProductDto>>(products);
            }
            catch(Exception )
            {
                throw;
            }
        }
    }
}
