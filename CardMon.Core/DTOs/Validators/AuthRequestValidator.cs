using CardMon.Core.DTOs.Request;
using CardMon.Core.Helpers;
using FluentValidation;
using System.Text.RegularExpressions;

namespace CardMon.Core.DTOs.Validators
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull()
                .NotEmpty()
                .Matches(new Regex(Constants.AppRegex.ALPHABET, RegexOptions.Compiled))
                .WithMessage("Only alphabets are allowed for username");
        }
    }
}
