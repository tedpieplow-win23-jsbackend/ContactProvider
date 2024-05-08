using ContactProvider.Entities;
using ContactProvider.Models;

namespace ContactProvider.Factories;

public class ContactFactory
{
    public ContactEntity PopulateContactEntity(ContactModel model)
    {
        return new ContactEntity
        {
            Name = model.Name,
            Email = model.Email,
            Service = model.Service,
            Message = model.Message,
        };
    }
}
