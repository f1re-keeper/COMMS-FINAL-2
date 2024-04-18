using COMMSFinalProject.Models;
using COMMSFinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using COMMSFinalProject.Interfaces;
using COMMSFinalProject.Services;
using COMMSFinalProject.Models;

namespace COMMSFinalProject.Controllers
{
    [Authorize(Roles = Role.Accountant)]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountantController : ControllerBase
    {
        private readonly UserContext _context;
        private IAccountant _accountantService;
        private IUser _userService;
        private readonly ILogger<AccountantController> _logger;
        public AccountantController(UserContext context, IAccountant accountantService, IUser userService,
            ILogger<AccountantController> logger)
        {
            _context = context;
            _accountantService = accountantService;
            _userService = userService;
            _logger = logger;
        }


        [HttpPut("blockuser")]
        public async Task<IActionResult> BlockUser(User model)
        {
            if (_context.Users.Find(model.Id) == null)
            {
                _logger.LogError("User not found");
                return UnprocessableEntity("User not found");
            }
            await _accountantService.RestrictUser(model.Id);
            return Ok("User Blocked");
        }

        [HttpPut("unblockuser")]
        public async Task<IActionResult> UnblockUser(User model)
        {
            if (_context.Users.Find(model.Id) == null)
            {
                _logger.LogError("User not found");
                return UnprocessableEntity("User not found");
            }
            await _accountantService.UnrestrictUser(model.Id);
            return Ok("User Unblocked");
        }

        [HttpGet("anyuserloans")]
        public async Task<IActionResult> GetAnyUserLoans(User model)
        {
            if (_context.Users.Find(model.Id) == null)
            {
                _logger.LogError("User not found");
                return UnprocessableEntity("User not found");
            }
            var userLoans = await _accountantService.GetLoan(model.Id);
            return Ok(userLoans);
        }

        [HttpDelete("deleteanyloan")]
        public async Task<IActionResult> DeleteAnyLoan(Loan model)
        {
            if (_context.Loans.Find(model.Id) == null)
            {
                _logger.LogError("Loan not found");
                return UnprocessableEntity("Loan not found");
            }
            await _accountantService.DeleteLoan(model.Id);
            return Ok("Loan Deleted");
        }

        [HttpPut("updateanyloan")]
        public async Task<IActionResult> UpdateAnyLoan(UpdatedLoan model)
        {
            if (_context.Loans.Find(model.Id) == null)
            {
                _logger.LogError("Loan not found");
                return UnprocessableEntity("Loan not found");
            }
            LoanValidation validator = new LoanValidation(_context);
            var tempLoan = await _accountantService.UpdateLoan(model);
            var verifiableLoan = validator.ConvertToValid(tempLoan);
            var result = validator.Validate(verifiableLoan);
            if (!result.IsValid)
            {
                return BadRequest(ValidationError.GetErrors(result));
            }
            _context.Loans.Update(tempLoan);
            await _context.SaveChangesAsync();
            return Ok("Loan Updated");
        }
        [AllowAnonymous]
        [HttpPost("generateaccountant")]
        public async Task<IActionResult> GenerateAccountant(string first, string last)
        {
            var accountant = await _accountantService.GenerateAccountant(first, last);
            var tokenString = _userService.GenerateToken(accountant);
            accountant.Token = tokenString;
            _context.Users.Update(accountant);
            _context.SaveChanges();
            return Ok($"Accountant created, your Credentials: Username: Accountant" +
                $"Password: 12345" +
                $"Token: {accountant.Token}");
        }
    }
}
