using Microsoft.AspNetCore.Mvc.Testing;
using NumberToWordsWebPage.Models;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;

namespace NumberToWordsWebPage.Tests;

public sealed class NumberToWordsApiTests : IClassFixture<NumberToWordsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public NumberToWordsApiTests(NumberToWordsWebApplicationFactory factory)
    {
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            });
    }

    [Fact]
    public async Task ConvertSuccessful()
    {
        var request = new NumberToWordsRequest("123.45");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var responseBody = await response.Content.ReadFromJsonAsync<NumberToWordsResponse>(); 

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal( "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", responseBody?.Words);
    }

    [Fact]
    public async Task EmptyInput()
    {
        var request = new NumberToWordsRequest("");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The 'value' field is required.", error?.Error);
    }

    [Fact]
    public async Task InvalidNumber()
    {
        var request = new NumberToWordsRequest("Test123");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The 'value' field must be a non-negative decimal using a period as the decimal separator.", error?.Error);
    }

    [Fact]
    public async Task MoreThanTwoDecimalPlaces()
    {
        var request = new NumberToWordsRequest("123.456");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Amount must have at most two decimal places.", error?.Error);
    }

    [Fact]
    public async Task NegativeInput()
    {
        var request = new NumberToWordsRequest("-123.45");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The 'value' field must be a non-negative decimal using a period as the decimal separator.", error?.Error);
    }

    [Fact]
    public async Task ExceedMaximum()
    {
        var request = new NumberToWordsRequest("1000000000");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Amount must be between 0 and 999999999.99.", error?.Error);
    }
    [Theory]
    [InlineData("1,2")]
    [InlineData("+1")]
    public async Task RejectsAmbiguousOrUnsupportedNumberFormats(string value)
    {
        var response = await _client.PostAsJsonAsync("/api/number-to-words", new NumberToWordsRequest(value));
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The 'value' field must be a non-negative decimal using a period as the decimal separator.", error?.Error);
    }

    [Fact]
    public async Task MalformedJsonUsesTheStandardErrorContract()
    {
        using var content = new StringContent("{", Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/number-to-words", content);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The request body must contain a valid 'value' field.", error?.Error);
    }
}

public sealed class NumberToWordsWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
    }
}
