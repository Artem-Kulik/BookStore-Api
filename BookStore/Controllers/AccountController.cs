using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Dto.ResultDto;
using BookStore.Models.Entities;
using BookStore.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ApplicationContext ctx;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        public AccountController(
                ApplicationContext context,
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            ctx = context;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }
        [HttpGet]
        public ResultDto ok()
        {
            return new ResultDto
            {
                IsSuccessful = true
            };
        }

        [HttpPost("register")]
        public async Task<ResultDto> Register([FromBody] RegisterDto model)
        {
            User user = new User()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email
            };
            await _userManager.CreateAsync(user, model.Password);
            UserAdditionalInfo ui = new UserAdditionalInfo()
            {
                Id = user.Id,
                Age = model.Age,
                FullName = model.FullName,
                Image = model.Image
            };
            var result = _userManager.AddToRoleAsync(user, "Guest").Result;
            await ctx.UserAdditionalInfo.AddAsync(ui);
            await ctx.SaveChangesAsync();

            return new ResultDto
            {
                IsSuccessful = true
            };
        }

        [HttpPost("login")]
        public async Task<ResultDto> Login(LoginDto model)
        {
            try
            {
                var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (!res.Succeeded)
                    return new ResultDto
                    {
                        IsSuccessful = false
                    };

                var user = await _userManager.FindByEmailAsync(model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                

                var id = ctx.Users.Where(el => el.Email == model.Email).FirstOrDefault().Id;
                return new ResultLoginDto
                {
                    IsSuccessful = true,
                    Token = _jwtTokenService.CreateToken(user),
                    Message = id
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = ex.Message
                };
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ResultDto GetUser([FromRoute] string id)
        {
            var user = ctx.Users.Find(id);
            var userA = ctx.UserAdditionalInfo.Find(id);
            EditDto res = new EditDto()
            {
                Id = userA.Id,
                FullName = userA.FullName,
                Age = userA.Age,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Image = userA.Image
            };

            return new SingleResultDto<EditDto>
            {
                IsSuccessful = true,
                Data = res
            };
        }

        [HttpPost("edit")]
        public ResultDto EditUser(EditDto model)
        {
            var user = ctx.Users.Find(model.Id);
            var userA = ctx.UserAdditionalInfo.Find(model.Id);

            userA.FullName = model.FullName;
            userA.Age = model.Age;
            userA.Image = model.Image;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;

            ctx.SaveChanges();

            return new ResultDto
            {
                IsSuccessful = true
            };
        }
    }
}