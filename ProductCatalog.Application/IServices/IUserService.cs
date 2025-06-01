using ProductCatalog.Common;
using ProductCatalog.Common.User.Request;
using ProductCatalog.Common.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.IServices
{
    public interface IUserService
    {
        Task<BaseCommandResponse<bool>> CreateUserAsync(CreateUserDTO req);
        Task<BaseCommandResponse<LoginResDTO>> LoginAsync(LoginReqDTO req);
    }
}
