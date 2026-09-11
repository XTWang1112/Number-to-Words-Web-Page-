namespace NumberToWordsWebPage.Services;

public sealed class NumberToWordsException : Exception
{
    public NumberToWordsException(string message) : base(message)
    {
    }
}
