using COMMSFinalProject.Interfaces;
using COMMSFinalProject.Models;
using Microsoft.Extensions.Options;

namespace COMMSFinalProject.Services 
{
    public class AccountantImp : IAccountant
    {
        private UserContext _context;
        private readonly AppSettings _appSettings;
        //private UserImp _userService;
        public AccountantImp(UserContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public async Task<User> GenerateAccountant(string first, string last)
        {
            var accountant = new User()
            {
                Role = Role.Accountant,
                FirstName = first,
                LastName = last
            };
            await _context.Users.AddAsync(accountant);
            await _context.SaveChangesAsync();
            return accountant;
        }
        public async Task<IQueryable<Loan>> GetLoan(int id)
        {
            return _context.Loans.Where(loan => loan.UserId == id);
        }
        public async Task<Loan> UpdateLoan(UpdatedLoan update)
        {
            var newLoan = await _context.Loans.FindAsync(update.Id);
            if (update.LoanType != null) newLoan.LoanType = update.LoanType;
            if (update.Amount != 0) newLoan.Amount = update.Amount;
            if (update.Currency != null) newLoan.Currency = update.Currency;    
            if (update.LoanPeriod != 0) newLoan.LoanPeriod = update.LoanPeriod;
            return newLoan;
        }
        public async Task<Loan> DeleteLoan(int id)
        {
            var deleted = _context.Loans.Find(id);
            _context.Loans.Remove(deleted);
            await _context.SaveChangesAsync();
            return deleted;
        }
        public async Task<User> RestrictUser(int id)
        {
            var user = _context.Users.Find(id);
            user.isBlocked = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<User> UnrestrictUser(int id)
        {
            var user = _context.Users.Find(id);
            user.isBlocked = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
        
    }
}
