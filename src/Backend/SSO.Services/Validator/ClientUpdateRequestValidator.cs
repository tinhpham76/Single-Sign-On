
using FluentValidation;
using SSO.Service.CreateModel;
using SSO.Services.CreateModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SSO.Service.Validator
{
    public class ClientUpdateRequestValidator : AbstractValidator<ClientUpdateRequest>
    {
        public ClientUpdateRequestValidator()
        {
            
        }
    }
}