using Capisol.Interview.Assessment.Context;
using Capisol.Interview.Assessment.Models.ContactModels;
using Microsoft.AspNetCore.Mvc;

namespace Capisol.Interview.Assessment.Controllers.API;

public class CompanyController : APIBaseController
{
    private readonly ILogger<WeatherForecastController> _logger;

    public CompanyController(DataContext dataContext, ILogger<WeatherForecastController> logger)
        : base(dataContext)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<CompanyModel> Get()
    {
        return DataContext.CompanySet?.Select(a => new CompanyModel()
        {
            Name = a.Name,
            RegistrationDate = a.RegistrationDate,
            Active = a.Active
        }).Where(c => c.Active).ToArray() ?? Array.Empty<CompanyModel>();
    }
}
