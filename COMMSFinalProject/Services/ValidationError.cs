using FluentValidation.Results;
namespace COMMSFinalProject.Services
{
    public class ValidationError
    {
        public static List<string> GetErrors(ValidationResult result)
        {
            var errorList = result.Errors.ToList<ValidationFailure>();
            var errorMessageList = new List<string>();
            foreach (var i in errorList)
            {
                errorMessageList.Add(i.ErrorMessage);
            }
            return errorMessageList;
        }
    }
}
