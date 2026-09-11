using Microsoft.AspNetCore.Mvc;
using NumberToWordsWebPage.Models;
using NumberToWordsWebPage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CurrencyToWordsConverter>();

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