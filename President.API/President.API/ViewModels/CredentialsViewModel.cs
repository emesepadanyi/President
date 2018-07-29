using FluentValidation.Attributes;
using President.API.ViewModels.Validations;

namespace President.API.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
