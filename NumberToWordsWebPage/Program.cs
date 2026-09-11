using Microsoft.AspNetCore.Mvc;
using NumberToWordsWebPage.Models;
using NumberToWordsWebPage.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<CurrencyToWordsConverter>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program
{
}