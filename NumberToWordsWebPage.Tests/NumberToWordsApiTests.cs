using Microsoft.AspNetCore.Mvc.Testing;
using NumberToWordsWebPage.Models;
using System.Net.Http.Json;
using System.Net;

namespace NumberToWordsWebPage.Tests;

public sealed class NumberToWordsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public NumberToWordsApiTests(WebApplicationFactory<Program> factory)
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
        Assert.Equal("The 'Value' field is required.", error?.Error);
    }

    [Fact]
    public async Task InvalidNumber()
    {
        var request = new NumberToWordsRequest("Test123");
        var response = await _client.PostAsJsonAsync("/api/number-to-words", request);
        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The 'Value' field must be a valid decimal number.", error?.Error);
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
        Assert.Equal("Amount must be between 0 and 999999999.99.", error?.Error);
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
}

// mock???