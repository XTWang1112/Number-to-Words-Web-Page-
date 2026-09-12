using Microsoft.AspNetCore.Mvc;
using NumberToWordsWebPage.Models;
using NumberToWordsWebPage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = _ =>
            new BadRequestObjectResult(new ErrorResponse("The request body must contain a valid 'value' field."));
    });
builder.Services.AddSingleton<INumberToWordsConverter, NumberToWordsConverter>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // HTTPS is terminated by the hosting platform.
}
else
{
    app.UseHttpsRedirection();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();

app.Run();

public partial class Program
{
}
