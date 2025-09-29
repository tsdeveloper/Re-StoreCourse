using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("not-found")]
    public ActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("unauthorised")]
    public ActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("validation-error")]
    public ActionResult GetValidationError()
    {
        ModelState.AddModelError("Problem1", "This is first error");
        ModelState.AddModelError("Problem2", "This is second error");
        return ValidationProblem();
    }


    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest("This is a bad request");
    }

    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        throw new Exception("This is server error");
    }
    
    [HttpPost("stripe")]
    public async Task<IActionResult> PostStripePaymentEvent()
    {
        string jsonBody;

        using (var reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8))
        {
            jsonBody = await reader.ReadToEndAsync();
        }
        
        if (string.IsNullOrWhiteSpace(jsonBody)) return BadRequest();
        
        var stripeConverter = JsonConvert.DeserializeObject<StripeChargerPayment>(jsonBody);
        return Ok(stripeConverter);
    }
}