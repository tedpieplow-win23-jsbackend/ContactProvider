using ContactProvider.Models;
using ContactProvider.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ContactProvider.Functions
{
    public class ContactFunction(ILogger<ContactFunction> logger, ContactService contactService)
    {
        private readonly ILogger<ContactFunction> _logger = logger;
        private readonly ContactService _contactService = contactService;

        [Function("ContactFunction")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                if (body != null)
                {
                    var result = await _contactService.SaveContactAsync(body);
                    if (result.StatusCode == StatusCode.OK)
                    {
                        return new OkResult();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error :: ContactProvider.ContactFunction.Run() :: message : {ex.Message}");
            }
            return new BadRequestResult();
        }
    }
}
