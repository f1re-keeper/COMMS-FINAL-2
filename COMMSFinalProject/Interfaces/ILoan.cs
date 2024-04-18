using COMMSFinalProject.Models;

namespace COMMSFinalProject.Interfaces
{
    public interface ILoan
    {
        public IQueryable<Loan> GetOwnLoans(int userId);
        public Task<Loan> AddLoan(NewLoan newLoan, int userId);
        public Task<Loan> UpdateOwnLoan(UpdatedLoan newLoan);
        public Task<Loan> DeleteOwnLoan(int loanId);
    }
}
