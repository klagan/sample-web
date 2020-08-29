using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Samples.ActionResult.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            var iActionResult = GetIActionResult();

            var json = GetJsonResult();

            var result = GetActionResult();
                       
            var payload = result.Convert();

            Console.WriteLine($"response => {payload.FirstName} {payload.LastName}");  
        }

        static ActionResult<Person> GetActionResult()
        {
            return  new OkObjectResult(new Person { FirstName = "Kam", LastName = "Lagan" });
        }

        static JsonResult GetJsonResult()
        {
            var result = new JsonResult(new Person { FirstName = "Kam", LastName = "Lagan" })
            {
                ContentType = "application/json",                
                StatusCode = (int)HttpStatusCode.OK
            };

            return result;
        }

        static IActionResult GetIActionResult()
        {
            return new OkObjectResult(new Person { FirstName = "Kam", LastName = "Lagan" });
        }
    }
}
