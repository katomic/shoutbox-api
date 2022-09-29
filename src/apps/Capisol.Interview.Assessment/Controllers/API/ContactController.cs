using Capisol.Interview.Assessment.Context;
using Capisol.Interview.Assessment.Models.ContactModels;
using Microsoft.AspNetCore.Mvc;

namespace Capisol.Interview.Assessment.Controllers.API;

public class ContactController : APIBaseController
{
    private readonly ILogger<WeatherForecastController> _logger;

    public ContactController(DataContext dataContext, ILogger<WeatherForecastController> logger)
        : base(dataContext)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<ContactModel> Get()
    {
        return DataContext.ContactSet?.Select(a => new ContactModel()
        {
            Firstname = a.Firstname,
            Lastname = a.Lastname,
            Email = a.Email
        }).ToArray() ?? Array.Empty<ContactModel>();
    }
}
