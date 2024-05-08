using ContactProvider.Models;
using FluentValidation;

namespace ContactProvider.Helpers;

public class ContactValidator : AbstractValidator<ContactModel>
{
    public ContactValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Message).NotEmpty();
    }
}
