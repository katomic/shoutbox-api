﻿using Capisol.Interview.Assessment.Context;
using Capisol.Interview.Assessment.Models.ShoutModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capisol.Interview.Assessment.Controllers.API;

public class ShoutController : APIBaseController
{
    private readonly ILogger<WeatherForecastController> _logger;

    public ShoutController(DataContext dataContext, ILogger<WeatherForecastController> logger)
        : base(dataContext)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<ShoutModel> Get()
    {
        return DataContext.ShoutSet?.Select(a => new ShoutModel()
        {
            Username = a.Username,
            Message = a.Message,
            CreateDate = a.CreateDate
        }).ToArray() ?? Array.Empty<ShoutModel>();
    }

    [HttpPost]
    public ShoutModel PostShoutModel(ShoutModel shout)
    {

         var newShout = DataContext.ShoutSet.Add(new Context.Entities.Shouts.Shout
         {
             CreateDate=DateTime.Now,
             Username = shout.Username,
             Message = shout.Message
         });

        DataContext.SaveChanges();


        return  shout;
    }

    //[HttpPost]
    //public async Task<ActionResult<ShoutModel>> PostShoutModel(ShoutModel shout)
    //{

    //    var newShout = DataContext.ShoutSet.Add(new Context.Entities.Shouts.Shout
    //    {
    //        CreateDate = DateTime.Now,
    //        Username = shout.Username,
    //        Message = shout.Message
    //    });

    //   await DataContext.SaveChangesAsync();



    //    return CreatedAtAction("GetShout", new { id = shout.Id }, shout);
    //}


}
