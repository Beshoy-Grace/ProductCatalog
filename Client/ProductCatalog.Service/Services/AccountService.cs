using ProductCatalog.Common;
using ProductCatalog.Common.Category.Response;
using ProductCatalog.Common.User.Request;
using ProductCatalog.Common.User.Response;
using ProductCatalog.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductCatalog.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseCommandResponse<bool>> CreateUserAsync(CreateUserDTO req)
        {
            var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Account/register", content);

            var result = JsonSerializer.Deserialize<BaseCommandResponse<bool>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task<BaseCommandResponse<LoginResDTO>> LoginAsync(LoginReqDTO req)
        {
            var content = new StringContent(JsonSerializer.Serialize(req), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Account/login", content);

            var result = JsonSerializer.Deserialize<BaseCommandResponse<LoginResDTO>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }
    }
}
