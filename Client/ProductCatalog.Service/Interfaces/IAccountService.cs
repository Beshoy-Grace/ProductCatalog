using ProductCatalog.Common.User.Request;
using ProductCatalog.Common.User.Response;
using ProductCatalog.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseCommandResponse<bool>> CreateUserAsync(CreateUserDTO req);
        Task<BaseCommandResponse<LoginResDTO>> LoginAsync(LoginReqDTO req);
    }
}
