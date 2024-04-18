using COMMSFinalProject.Models;

namespace COMMSFinalProject.Interfaces
{
    public interface IAccountant
    {
        public Task<IQueryable<Loan>> GetLoan(int userId);
        public Task<Loan> UpdateLoan(UpdatedLoan newLoan);
        public Task<Loan> DeleteLoan(int loanId);
        public Task<User> RestrictUser(int userId);
        public Task<User> UnrestrictUser (int userId);
        public Task<User> GenerateAccountant(string first, string last);
    }
}
