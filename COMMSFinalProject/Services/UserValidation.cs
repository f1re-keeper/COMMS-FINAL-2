using FluentValidation;
using COMMSFinalProject.Models;

namespace COMMSFinalProject.Services
{
    public class UserValidation : AbstractValidator<NewUser>
    {
        UserContext _context;
        public UserValidation(UserContext context)
        {
            _context = context;

            RuleFor(NewUser => NewUser.FirstName)
                .Length(1, 50).WithMessage("Out of bounds")
                .NotNull().WithMessage("Enter First Name")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Invalid First Name");
            RuleFor(NewUser => NewUser.LastName)
                .Length(1, 50).WithMessage("Out of bounds")
                .NotNull().WithMessage("Enter Last Name")
                .Matches(@"^[a-zA-Z]+$").WithMessage("Invalid Last Name");
            RuleFor(NewUser => NewUser.UserName)
                .Length(1, 50).WithMessage("Out of bounds")
                .NotNull().WithMessage("Enter Username")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Invalid Username")
                .Must(UniqueUserName).WithMessage("Username already exists");
            RuleFor(NewUser => NewUser.Password)
                .Length(8, 50).WithMessage("Out of bounds")
                .NotNull().WithMessage("Enter Password")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.");
            RuleFor(NewUser => NewUser.Age)
                .NotNull().WithMessage("Enter Age")
                .GreaterThan(18).WithMessage("User not allowed to register");
            RuleFor(NewUser => NewUser.Salary)
                .NotNull().WithMessage("Enter Salary")
                .GreaterThan(100).WithMessage("Salary must be higher than 100");

        }

        private bool UniqueUserName(string name)
        {
            var uniqueCheck = _context.Users.Where(x => x.UserName.ToLower() == name.ToLower()).FirstOrDefault();

            if (uniqueCheck == null) return true;
            return false;
        }
    }
}
