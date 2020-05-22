
using FluentValidation;
using SSO.Service.RequestModel.Client;

namespace SSO.Service.Validator
{
    public class ClientCreateRequestValidator : AbstractValidator<ClientQuickRequest>
    {
        public ClientCreateRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId value is required")
               .MaximumLength(50).WithMessage("ClientId cannot over limit 50 characters");

            RuleFor(x => x.ClientName).NotEmpty().WithMessage("ClientName value is required")
              .MaximumLength(200).WithMessage("ClientName cannot over limit 200 characters");

            RuleFor(x => x.Description).MaximumLength(200).WithMessage("Description cannot over limit 200 characters");

            RuleFor(x => x.ClientUri).NotEmpty().WithMessage("ClientUri value is required");
        }
    }
}