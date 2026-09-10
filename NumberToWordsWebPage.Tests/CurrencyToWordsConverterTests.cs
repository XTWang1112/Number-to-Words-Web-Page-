using NumberToWordsWebPage.Services;

namespace NumberToWords.Tests;

public class CurrencyToWordsConverterTests
{
    private readonly CurrencyToWordsConverter _converter = new();

    [Theory]
    [InlineData(0, "ZERO DOLLARS")]
    [InlineData(1, "ONE DOLLAR")]
    [InlineData(2, "TWO DOLLARS")]
    public void ConvertToWords_BasicNumbers(decimal amount, string expectedResult)
    {
        var result = _converter.Convert(amount);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(19, "NINETEEN DOLLARS")]
    [InlineData(20, "TWENTY DOLLARS")]
    [InlineData(21, "TWENTY-ONE DOLLARS")]
    public void ConvertToWords_CrossTwenty(decimal amount, string expectedResult)
    {
        var result = _converter.Convert(amount);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(99, "NINETY-NINE DOLLARS")]
    [InlineData(100, "ONE HUNDRED DOLLARS")]
    [InlineData(101, "ONE HUNDRED AND ONE DOLLARS")]
    public void ConvertToWords_CrossHundred(decimal amount, string expectedResult)
    {
        var result = _converter.Convert(amount);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(999, "NINE HUNDRED AND NINETY-NINE DOLLARS")]
    [InlineData(1000, "ONE THOUSAND DOLLARS")]
    [InlineData(1001, "ONE THOUSAND AND ONE DOLLARS")]
    public void ConvertToWords_CrossThousand(decimal amount, string expectedResult)
    {
        var result = _converter.Convert(amount);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(999999, "NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS")]
    [InlineData(1000000, "ONE MILLION DOLLARS")]
    [InlineData(1000001, "ONE MILLION AND ONE DOLLARS")]
    public void ConvertToWords_CrossMillion(decimal amount, string expectedResult)
    {
        var result = _converter.Convert(amount);

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(0.01, "ZERO DOLLARS AND ONE CENT")]
    [InlineData(0.02, "ZERO DOLLARS AND TWO CENTS")]
    [InlineData(0.10, "ZERO DOLLARS AND TEN CENTS")]
    [InlineData(0.99, "ZERO DOLLARS AND NINETY-NINE CENTS")]
    [InlineData(1.01, "ONE DOLLAR AND ONE CENT")]
    [InlineData(2.02, "TWO DOLLARS AND TWO CENTS")]
    public void ConvertToWords_WithCents(decimal amount, string expectedResult)
    {
        var result = _converter.Convert(amount);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void ConvertToWords_SpecificExample()
    {
        var result = _converter.Convert(123.45m);

        Assert.Equal("ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", result);
    }

    [Fact]
    public void ConvertToWords_NegativeNumber()
    {
        var action = () => _converter.Convert(-0.01m);

        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void ConvertToWords_ExceedMaximum()
    {
        var action = () => _converter.Convert(1_000_000_000m);

        Assert.Throws<ArgumentOutOfRangeException>(action);
    }

    [Fact]
    public void ConvertToWords_WhenMaximum()
    {
        var result = _converter.Convert(999_999_999.99m);

        Assert.Equal("NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS AND NINETY-NINE CENTS", result);
    }
}
