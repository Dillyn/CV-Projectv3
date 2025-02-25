using Domain.Models;
using FirstAPI.Authentication.Contracts;
using FirstAPI.Authentication.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Authentication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<UserI> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<UserI> _signinManager;

        public UserController(UserManager<UserI> userManager, ITokenService tokenService, SignInManager<UserI> signinManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signinManager;
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid username!");

            var result = _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false).Result;
            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
            return Ok(
               new NewUserDto
               {
                   UserName = user.UserName,
                   Email = user.Email,
                   Token = _tokenService.CreateToken(user)
               }
           );
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var UserA = new UserI
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(UserA, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var a = await _userManager.AddToRoleAsync(UserA, "User");
                    if (a.Succeeded)
                    {
                        return Ok(
                          new NewUserDto
                          {
                              UserName = UserA.UserName,
                              Email = UserA.Email,
                              Token = _tokenService.CreateToken(UserA)
                          }
                        );
                    }
                    else
                    {
                        return StatusCode(500, a.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
