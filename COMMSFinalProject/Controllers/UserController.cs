using COMMSFinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using COMMSFinalProject.Interfaces;
using COMMSFinalProject.Services;
using FluentValidation.Results;

namespace COMMSFinalProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserContext _context;
        private IUser _userService;
        private readonly ILogger<UserController> _logger;
        private User curr;
        public UserController(UserContext context, IUser userService, ILogger<UserController> logger)
        {
            _context = context;
            _userService = userService;
            _logger = logger;
        }


        //Register
        [AllowAnonymous]
        [HttpPost("register/regData")]
        public async Task<ActionResult<User>> AddUser(NewUser regData)
        {
            UserValidation validator = new UserValidation(_context);
            ValidationResult result = validator.Validate(regData);
            if (!result.IsValid)
            {
                foreach (var error in ValidationError.GetErrors(result))
                {
                    _logger.LogError(error);
                }
                return BadRequest(ValidationError.GetErrors(result));

            }
            await _userService.Register(regData);
            await _context.SaveChangesAsync();
            return Ok("Registration Successful");
        }

        //Login
        [AllowAnonymous]
        [HttpPost("login/loginModel")]
        public async Task<IActionResult> Login(Login model)
        {
            var user = _userService.Authenticate(model.UserName, model.Password);
            if (user == null)
            {
                _logger.LogError("Username or Password incorrect");
                return BadRequest("Username or Password incorrect");
            }
            await _userService.Login(user);
            curr = user;
            return Ok($"Login Successful. Your token: {user.Token}");
        }

        //Data
        [AllowAnonymous]
        [HttpGet("userData")]
        public IActionResult GetOwnData()
        {
            var userData = _userService.GetOwnData();
            return Ok(userData);
        }
    }
}
