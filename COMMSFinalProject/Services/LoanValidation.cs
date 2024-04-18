using COMMSFinalProject.Models;
using FluentValidation;

namespace COMMSFinalProject.Services
{
    public class LoanValidation : AbstractValidator<NewLoan>
    {
        UserContext _context;
        public LoanValidation(UserContext context)
        {
            _context = context;

            RuleFor(NewLoan => NewLoan.LoanType)
                .NotEmpty().NotNull().WithMessage("Enter Loan Type")
                .Must(isTypeValid).WithMessage("Invalid Loan Type");
            RuleFor(NewLoan => NewLoan.Currency)
                .NotEmpty().NotNull().WithMessage("Enter Currency")
                .Must(isCurrencyValid).WithMessage("Invalid Currency");
            RuleFor(NewLoan => NewLoan.Amount)
                .NotNull().NotEmpty().WithMessage("Enter Amount")
                .GreaterThan(500).WithMessage("Amount must be higher than 500")
                .LessThan(50000).WithMessage("More action required for loan more than 50000");
            RuleFor(NewLoan => NewLoan.LoanPeriod)
                .NotNull().NotEmpty().WithMessage("Enter Loan Period")
                .GreaterThan(0).WithMessage("Period must be longer than 1 month")
                .LessThan(120).WithMessage("Period must be lower than 10 years");
        }

        private bool isTypeValid(string loanType)
        {
            List<string> loanTypes = new List<string>()
                {
                    "quick loan",
                    "auto loan",
                    "installment"
                };
            var loanTypeLower = loanType.ToLower();
            if (loanTypes.Contains(loanTypeLower)) return true;
            else return false;
        }

        private bool isCurrencyValid(string currency)
        {
            List<string> currencies = new List<string>()
                {
                    "USD",
                    "GEL"
                };
            var currencyLower = currency.ToLower();
            if (currencies.Contains(currencyLower)) return true;
            else return false;
        }

        public NewLoan ConvertToValid(Loan model)
        {
            NewLoan newLoan = new NewLoan();
            newLoan.LoanType = model.LoanType;
            newLoan.LoanPeriod = model.LoanPeriod;
            newLoan.Currency = model.Currency;
            newLoan.Amount = model.Amount;
            return newLoan;
        }
    }
}
