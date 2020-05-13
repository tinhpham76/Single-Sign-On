
using FluentValidation;
using SSO.Service.CreateModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SSO.Service.Validator
{
    public class ClientCreateRequestValidator : AbstractValidator<ClientCreateRequest>
    {
        public ClientCreateRequestValidator()
        {
            RuleFor(x => x.ClientId).NotEmpty().WithMessage("ClientId value is required")
               .MaximumLength(50).WithMessage("ClientId cannot over limit 50 characters");

            RuleFor(x => x.ClientName).NotEmpty().WithMessage("ClientName value is required")
              .MaximumLength(200).WithMessage("ClientName cannot over limit 200 characters");

            RuleFor(x => x.ClientSecrets).MaximumLength(200).WithMessage("ClientSecrets cannot over limit 200 characters");

            RuleFor(x => x.RedirectUris).NotEmpty().WithMessage("RedirectUris value is required");

            RuleFor(x => x.PostLogoutRedirectUris).NotEmpty().WithMessage("PostLogoutRedirectUris value is required");

            RuleFor(x => x.AllowedCorsOrigins).NotEmpty().WithMessage("AllowedCorsOrigins value is required");
        }
    }
}