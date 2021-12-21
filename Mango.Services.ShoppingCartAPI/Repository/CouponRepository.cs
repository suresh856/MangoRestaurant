using Mango.Services.ShoppingCartAPI.Models.Dto;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _client;
        public CouponRepository(HttpClient client)
        {
            _client = client;
        }
        public async  Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await _client.GetAsync($"/api/coupon/{couponName}");
            var apiConytent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(Convert.ToString(apiConytent));
            if(resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
