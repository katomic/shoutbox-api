using Capisol.Interview.Assessment.Context;
using Microsoft.AspNetCore.Mvc;

namespace Capisol.Interview.Assessment.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class APIBaseController : ControllerBase, IDisposable
{
    public readonly DataContext DataContext;

    public APIBaseController(DataContext dataContext)
    {
        DataContext = dataContext;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            DataContext.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}