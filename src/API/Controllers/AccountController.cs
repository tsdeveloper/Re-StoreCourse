using API.Data;
using API.DTOs;
using API.DTOs.Baskets;
using API.DTOs.JWTRefreshTokens;
using API.DTOs.Logins;
using API.DTOs.Registers;
using API.DTOs.UserLogins;
using API.Entities.Baskets;
using API.Entities.JWT;
using API.Entities.Users;
using API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
   private readonly UserManager<UserCustom> _userManager;
   private readonly TokenService _serviceToken;
   private readonly RestoreCourseDbContext _context;

   public AccountController(UserManager<UserCustom> userManager, TokenService serviceToken,
      RestoreCourseDbContext context)
   {
      _userManager = userManager;
      _serviceToken = serviceToken;
      _context = context;
   }

   [HttpPost("login")]
   public async Task<ActionResult<UserLoginDto>> Login(LoginDto login)
   {
      var user = await _userManager.FindByNameAsync(login.Username);
      if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
         return Unauthorized();

      var userBasket = await RetrieveBasket(login.Username);
      var anonBasket = await RetrieveBasket(Request.Cookies["buyerId"]);

      if (anonBasket != null)
      {
         if (userBasket != null) _context.DbSet<Basket>().Remove(userBasket);

         anonBasket.BuyerId = user.UserName;
         Response.Cookies.Delete("buyerId");
         await _context.SaveChangesAsync();
      }
      
      return new UserLoginDto
      {
         Email = user.Email,
         Token = await _serviceToken.GenerateToke(user),
         RefreshToken = await _serviceToken.GenerateRefreshToken(user.Id),
         Basket = anonBasket != null ? (BasketReturnDTO)anonBasket : (BasketReturnDTO)userBasket
      };
   }
   
   private async Task<Basket> RetrieveBasket(string buyerId)
   {
      if (string.IsNullOrWhiteSpace(buyerId))
      {
         Response.Cookies.Delete("BasketId");
         return null;
      }
            
      var basket = await _context.DbSet<Basket>()
         .Include(x => x.BasketItems)
         .ThenInclude(x => x.Product)
         .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
      
      return basket;
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

   [Authorize]
   [HttpGet("currentUser")]
   public async Task<ActionResult<UserLoginDto>> GetCurrentUser()
   {
      var user = await _userManager.FindByNameAsync(User.Identity.Name);
      var userBasket = await RetrieveBasket(user.UserName);
      
      return new UserLoginDto
      {
         Email = user.Email,
         Token = await _serviceToken.GenerateToke(user),
         Basket = userBasket != null ? (BasketReturnDTO)userBasket : null
      };
   }
   
   [Authorize]
   [HttpGet("saveAddress")]
   public async Task<ActionResult<UserLoginDto>> GetSavedAddress()
   {
      return new UserAddressDto
      {
         Email = user.Email,
         Token = await _serviceToken.GenerateToke(user),
         Basket = userBasket != null ? (BasketReturnDTO)userBasket : null
      };
   }

   [HttpPost("refresh")]
   public async Task<ActionResult<UserLoginDto>> RefreshToken([FromBody] JWTRefreshTokenDto dto)
   {
      var existingRefreshToken = await _context.DbSet<JWTRefreshToken>()
         .SingleOrDefaultAsync(rt => rt.Token == dto.RefreshToken);

      if (existingRefreshToken == null || existingRefreshToken.IsRevoked)
         return Unauthorized("Refresh token is invalid");


      if (existingRefreshToken.ExpiredAt < DateTime.UtcNow)
      {
         await _serviceToken.RevokeRefreshToken(existingRefreshToken.Token);
         return Unauthorized("Refresh token is expired");
      }
      
      await _serviceToken.RevokeRefreshToken(existingRefreshToken.Token);
      
      var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId.ToString());
      
      return new UserLoginDto
      {
         Email = user.Email,
         Token = await _serviceToken.GenerateToke(user),
         RefreshToken = await _serviceToken.GenerateRefreshToken(user.Id)
      };
   }


}