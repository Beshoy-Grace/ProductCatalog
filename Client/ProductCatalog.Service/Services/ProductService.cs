using Microsoft.AspNetCore.Http;
using ProductCatalog.Common;
using ProductCatalog.Common.Category.Response;
using ProductCatalog.Common.Product.Request;
using ProductCatalog.Common.Product.Response;
using ProductCatalog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductCatalog.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseCommandResponse<GetProductResDTO>> CreateProductAsync(CreateProductReqDTO product)
        {
            AddAuthHeader();
            var response = await _httpClient.PostAsJsonAsync("api/Product/Create", product);
            return JsonSerializer.Deserialize<BaseCommandResponse<GetProductResDTO>> (await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<BaseCommandResponse<GetProductResDTO>> DeleteProductAsync(int id)
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/Product/Delete/{id}");
            return JsonSerializer.Deserialize<BaseCommandResponse<GetProductResDTO>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        }

        public async Task<BaseCommandResponse<List<GetProductResDTO>>> GetAllProductsAsync()
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync($"api/Product/GetAll");
            return JsonSerializer.Deserialize<BaseCommandResponse<List<GetProductResDTO>>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<BaseCommandResponse<List<GetProductResDTO>>> GetAllProductsByCategoryAsync(int id)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync($"api/Product/GetByCategory/{id}");
            return JsonSerializer.Deserialize<BaseCommandResponse<List<GetProductResDTO>>> (await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        }

        public async Task<BaseCommandResponse<GetProductResDTO>> GetProductByIdAsync(int id)
        {
            AddAuthHeader();

            var response = await _httpClient.GetAsync($"api/Product/GetById/{id}");
            return JsonSerializer.Deserialize<BaseCommandResponse<GetProductResDTO>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<BaseCommandResponse<GetProductResDTO>> UpdateProductAsync(UpdateProductReqDTO product)
        {
            AddAuthHeader();

            var response = await _httpClient.PutAsJsonAsync($"api/Product/Update", product);
            return JsonSerializer.Deserialize<BaseCommandResponse<GetProductResDTO>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        }

        private void AddAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

    }
}
