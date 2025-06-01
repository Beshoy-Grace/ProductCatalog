using ProductCatalog.Common.Category.Response;
using ProductCatalog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProductCatalog.Common.Category.Request;
using ProductCatalog.Service.Interfaces;
using ProductCatalog.Common.Pagination;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;




namespace ProductCatalog.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResult<GetCategoryResDTO>> GetAllAsync(GetAllCategoryDTO req)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"api/Category/GetAll?page={req.page}&PageSize={req.PageSize}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Optionally you can throw your own exception or return null
                throw new UnauthorizedAccessException();
            }
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<BaseCommandResponse<PagedResult<GetCategoryResDTO>>> (await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            if(!result.IsSuccess)
            {
               return new PagedResult<GetCategoryResDTO>();
            }
            return result.ResponseData;
        }

        public async Task<GetCategoryWithProductResDTO?> GetByIdAsync(int id)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync($"api/Category/GetById/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var result = JsonSerializer.Deserialize<BaseCommandResponse<GetCategoryWithProductResDTO>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if(!result.IsSuccess) return null;

            return result.ResponseData;
        }

        public async Task<bool> CreateAsync(CreateCategoryReqDTO category)
        {
            AddAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Category/Create", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryReqDTO category)
        {
            AddAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Category/Update", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<BaseCommandResponse<bool>> DeleteAsync(int id)
        {
            AddAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/Category/Delete/{id}");
           return JsonSerializer.Deserialize<BaseCommandResponse<bool>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
