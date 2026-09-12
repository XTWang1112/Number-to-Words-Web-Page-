using Microsoft.AspNetCore.Mvc;
using NumberToWordsWebPage.Models;
using NumberToWordsWebPage.Services;
using System.Globalization;

namespace NumberToWordsWebPage.Controllers;

[ApiController]
[Route("api/number-to-words")]
public sealed class NumberToWordsController : ControllerBase
{
    private readonly INumberToWordsConverter _converter;

    public NumberToWordsController(INumberToWordsConverter converter)
    {
        _converter = converter;
    }

    [HttpPost]
    public ActionResult<NumberToWordsResponse> Convert([FromBody] NumberToWordsRequest? request)
    {
        if (string.IsNullOrWhiteSpace(request?.Value))
        {
            return BadRequest(new ErrorResponse("The 'value' field is required."));
        }

        if (!decimal.TryParse(
                request.Value,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var number))
        {
            return BadRequest(new ErrorResponse("The 'value' field must be a non-negative decimal using a period as the decimal separator."));
        }

        try
        {
            var result = _converter.Convert(number);

            return Ok(new NumberToWordsResponse(result));
        }
        catch (NumberToWordsException ex)
        {
            return BadRequest(new ErrorResponse(ex.Message));
        }
    }

}
