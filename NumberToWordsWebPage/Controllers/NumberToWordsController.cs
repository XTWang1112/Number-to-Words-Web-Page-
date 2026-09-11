using Microsoft.AspNetCore.Mvc;
using NumberToWordsWebPage.Models;
using NumberToWordsWebPage.Services;
using System.Globalization;

namespace NumberToWordsWebPage.Controllers;

[ApiController]
[Route("api/number-to-words")]
public sealed class  NumberToWordsController : ControllerBase
{
    private readonly NumberToWordsConverter _converter;

    public NumberToWordsController(NumberToWordsConverter converter)
    {
        _converter = converter;
    }

    [HttpPost]
    public ActionResult<NumberToWordsResponse> Convert([FromBody] NumberToWordsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Value))
        {
            return BadRequest(new
            {
                Error = "The 'Value' field is required."
            });
        }

        if (!decimal.TryParse(request.Value, out var number))
        {
            return BadRequest(new
            {
                Error = "The 'Value' field must be a valid decimal number."
            });
        }

        try
        {
            var result = _converter.Convert(number);

            return Ok(new NumberToWordsResponse(result));
        }
        catch (CurrencyToWordsException ex)
        {
            return BadRequest(new
            {
                Error = ex.Message
            });
        }
    }

}
