using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiddleWare.Data;
using MiddleWare.Models.User;
using MiddleWare.Static;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiddleWare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            _logger.LogInformation($"Register attemp for {userDto.Email}");
            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                //if(string.IsNullOrEmpty(userDto.Role))
                //{
                //    await _userManager.AddToRoleAsync(user, "User");
                //}
                //else
                //{
                //    await _userManager.AddToRoleAsync(user, "Admin");
                //}
                await _userManager.AddToRoleAsync(user, "User");
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)} ");
                return Problem($"Something went wrong in the {nameof(Register)} ", statusCode: 500);

            }

        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
        {
            _logger.LogInformation($"Register attemp for {userDto.Email}");
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await _userManager.CheckPasswordAsync(user, userDto.Password); 
                if (user == null || passwordValid == false)
                {
                    return Unauthorized(userDto);
                }
                var tokenStrings = await GenerateToken(user);

                var response = new AuthResponse
                {
                    Email = userDto.Email,
                    Token = tokenStrings,
                    UserId = user.Id
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)} ");
                return Problem($"Something went wrong in the {nameof(Login)} ", statusCode: 500);
            }
        }
        private async Task<string> GenerateToken(ApiUser apiUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var role = await _userManager.GetRolesAsync(apiUser);
            var roleClaims = role.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(apiUser);    
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, apiUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, apiUser.Email),
                new Claim(CustomClaimTypes.Uid, apiUser.Id),
            }.Union(roleClaims).Union(userClaims);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
                
        }
    }
}
