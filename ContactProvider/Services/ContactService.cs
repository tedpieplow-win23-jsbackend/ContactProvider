using ContactProvider.Factories;
using ContactProvider.Helpers;
using ContactProvider.Models;
using ContactProvider.Repositories;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace ContactProvider.Services;

public class ContactService(ContactRepository repo, ContactFactory factory, ILogger<ContactService> logger)
{
    private readonly ILogger<ContactService> _logger = logger;
    private readonly ContactRepository _repo = repo;
    private readonly ContactFactory _factory = factory;

    public async Task<ResponseResult> SaveContactAsync(string body)
    {
        try
        {
            var contactModel = JsonConvert.DeserializeObject<ContactModel>(body);
            var validator = new ContactValidator();
            var validationResult = validator.Validate(contactModel!);
            if (validationResult.IsValid)
            {
                var contactEntity = _factory.PopulateContactEntity(contactModel!);
                var result = await _repo.CreateAsync(contactEntity);
                if (result.StatusCode == StatusCode.OK)
                {
                    return ResponseFactory.Ok();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Error :: ContactProvider.ContactFunction.Run() :: message : {ex.Message}");
        }
        return ResponseFactory.Error();
    }
}
