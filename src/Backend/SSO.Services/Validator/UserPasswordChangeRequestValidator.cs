using FluentValidation;
using FluentValidation.Validators;
using SSO.Services.CreateModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SSO.Services.Validator
{
    public class UserPasswordChangeRequestValidator : AbstractValidator<UserPasswordChangeUpdateRequest>
    {
        public UserPasswordChangeRequestValidator()
        {
            RuleFor(x=> x.UserId).NotEmpty().WithMessage("UserId name is required");

            RuleFor(x=> x.CurrentPassword).NotEmpty().WithMessage("CurrentPassword name is required");
           
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Password is required")
                 .MinimumLength(8).WithMessage("Password has to atleast 8 characters")
                 .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
                 .WithMessage("Password is not match complexity rules.");
        }
    }
}
