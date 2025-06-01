using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Application.IServices;
using ProductCatalog.Common;
using ProductCatalog.Common.User.Request;
using ProductCatalog.Common.User.Response;
using ProductCatalog.Framework.UoW;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StatusCodes = ProductCatalog.Common.StatusCodes;

namespace ProductCatalog.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<BaseCommandResponse<bool>> CreateUserAsync(CreateUserDTO req)
        {
            var emailExist = (await _unitOfWork.GetRepository<IdentityUser>().GetAllAsync(x => x.Email.ToLower() == req.Email.ToLower())).FirstOrDefault();
            if (emailExist != null)
            {
                return new BaseCommandResponse<bool> { ResponseData = false, IsSuccess = false,
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.BadRequest, Value = "Email already exist" } }
                };
            }

            var user = new IdentityUser
            {
                Email = req.Email,
                UserName = req.FirstName,
            };
            var result = await _userManager.CreateAsync(user, req.Password);
            if (result.Succeeded)
            {
                return new BaseCommandResponse<bool> { ResponseData = true, IsSuccess = true, Message = "User created successfully" };
            }
            return new BaseCommandResponse<bool> { ResponseData = false, IsSuccess = false,
                Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.InternalServerError, Value = "User creation failed" } }
            };
        }

        public async Task<BaseCommandResponse<LoginResDTO>> LoginAsync(LoginReqDTO req)
        {
            var user = await _userManager.FindByEmailAsync(req.Email);
            if (user == null)
            {
                return new BaseCommandResponse<LoginResDTO>
                {
                    IsSuccess = false,
                    StatusCode = (int)StatusCodes.BadRequest,
                    Message = "Invalid Username or Password",
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.InvalidUsernameOrPassword, Value = "Invalid username or password" } }
                };

            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, req.Password,false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return new BaseCommandResponse<LoginResDTO>
                {
                    IsSuccess = false,
                    StatusCode = (int)StatusCodes.BadRequest,
                    Message = "Invalid Username or Password", // return this error for security reasons to prevent if error is email or username
                    Errors = new List<Errors> { new Errors { Key = (int)StatusCodes.InvalidUsernameOrPassword, Value = "Invalid username or password" } }
                };
                
            }
            var token = GenerateJwtToken(user);
            return new BaseCommandResponse<LoginResDTO> { ResponseData = new LoginResDTO { Token = token }, IsSuccess = true };

        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };
          
            var sKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Generate Token for user 
            var JWToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(JWToken);
        }

    }
}
