namespace NumberToWordsWebPage.Services;

public sealed class CurrencyToWordsException : Exception
{
    public CurrencyToWordsException(string message) : base(message)
    {
    }
}
