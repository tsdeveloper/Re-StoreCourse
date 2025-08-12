using API.DTOs;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
   private readonly UserManager<UserCustom> _userManager;

   public AccountController(UserManager<UserCustom> userManager)
   {
      _userManager = userManager;
   }

   [HttpPost("login")]
   public async Task<ActionResult<UserCustom>> Login(LoginDto login)
   {
      var user = await _userManager.FindByNameAsync(login.Username);
      if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
         return Unauthorized();

      return user;
   }

   [HttpPost("register")]
   public async Task<ActionResult<UserCustom>> Login(RegisterDto registerDto)
   {
      var user = new UserCustom { UserName = registerDto.Username, Email = registerDto.Email };

      var result = await _userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded)
      {
         foreach (var error in result.Errors)
         {
            ModelState.AddModelError(error.Code, error.Description);
         }

         return ValidationProblem();
      }

      await _userManager.AddToRoleAsync(user, "Member");

      return StatusCode(201);
   }


}