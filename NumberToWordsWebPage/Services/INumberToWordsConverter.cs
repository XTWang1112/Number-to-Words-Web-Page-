namespace NumberToWordsWebPage.Services;

public interface INumberToWordsConverter
{
    string Convert(decimal amount);
}
