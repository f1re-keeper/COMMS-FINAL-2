using COMMSFinalProject.Models;
using COMMSFinalProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using COMMSFinalProject.Interfaces;

namespace COMMSFinalProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class LoanController : ControllerBase
    {
        private readonly UserContext _context;
        private ILoan _loanService;
        private readonly ILogger<LoanController> _logger;
        private readonly IUser _userService;

        public LoanController(UserContext context, ILoan loanService, ILogger<LoanController> logger, IUser userservice)
        {
            _context = context;
            _loanService = loanService;
            _logger = logger;
            _userService = userservice;
        }


        [Authorize(Roles = Role.User)]
        [HttpPost("newloan")]
        public IActionResult AddLoan(NewLoan addLoanModel)
        {
            LoanValidation validator = new LoanValidation(_context);
            var result = validator.Validate(addLoanModel);
            if (!result.IsValid)
            {
                foreach (var error in (ValidationError.GetErrors(result)))
                {
                    _logger.LogError(error);
                }
                return BadRequest(ValidationError.GetErrors(result));
            }
            var userId = _userService.GetOwnData().Id;
            if (_context.Users.Find(userId).isBlocked == true)
            {
                _logger.LogError("User blocked");
                return Unauthorized("The user is blocked");
            }
            _loanService.AddLoan(addLoanModel, userId);
            return Ok("Loan Created");

        }

        [Authorize(Roles = Role.User)]
        [HttpGet("ownloans")]
        public IActionResult GetOwnLoans()
        {
            var userId = _userService.GetOwnData().Id;
            return Ok(_loanService.GetOwnLoans(userId));
        }

        [Authorize(Roles = Role.User)]
        [HttpPut("updateownloan")]
        public async Task<IActionResult> UpdateOwnLoan(UpdatedLoan model)
        {
            LoanValidation validator = new LoanValidation(_context);
            var userId = _userService.GetOwnData().Id;
            var tempLoan = _loanService.UpdateOwnLoan(model).Result;
            tempLoan.UserId = userId;
            if (tempLoan.UserId != _context.Loans.Find(model.Id).UserId)
            {
                _logger.LogError("Not own loan");
                return Unauthorized("You are not allowed to modify this loan. Reason: Not your loan");
            }
            if (tempLoan.Status != "Processing")
            {
                _logger.LogError("Loan already processed");
                return Unauthorized("You are not allowed to modify this loan. Reason: Loan already processed");
            }
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

        [Authorize(Roles = Role.User)]
        [HttpDelete("deleteownloan")]
        public async Task<IActionResult> DeleteOwnLoan(Loan model)
        {
            var userId = _userService.GetOwnData().Id;
            IQueryable<Loan> ownLoans = _loanService.GetOwnLoans(userId);
            var loanToCheck = ownLoans.Where(loan => loan.Id == model.Id).FirstOrDefault();
            if (loanToCheck == null)
            {
                _logger.LogError("Loan not found");
                return UnprocessableEntity("Loan not found");
            }
            if (loanToCheck.Status != "Processing")
            {
                _logger.LogError("Loan already processed");
                return Unauthorized("You are not allowed to modify this loan. Reason: Loan already processed");
            }
            _loanService.DeleteOwnLoan(model.Id);
            return Ok("Loan Deleted");
        }
    }
}
