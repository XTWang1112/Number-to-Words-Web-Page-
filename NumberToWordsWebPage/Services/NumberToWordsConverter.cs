namespace NumberToWordsWebPage.Services;

public class NumberToWordsConverter : INumberToWordsConverter
{
    private const decimal MaxValue = 999999999.99m;
    private static readonly string[] Numbers = { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };

    private static readonly string[] Tens = { "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

    private static string ConvertUnderHundred(int number)
    {
        if (number < 20)
        {
            return Numbers[number];
        }

        var tens = number / 10;
        var remain = number % 10;

        if (remain == 0)
        {
            return Tens[tens - 2]; // -2 because the Tens array starts from 20
        }

        return $"{Tens[tens - 2]}-{Numbers[remain]}";
    }

    private static string ConvertUnderThousand(int number)
    {
        if (number < 100)
        {
            return ConvertUnderHundred(number);
        }

        var hundreds = number / 100;
        var remain = number % 100;

        var result = $"{Numbers[hundreds]} HUNDRED";

        if (remain == 0)
        {
            return result;
        }

        // hundred + AND + remaining number (e.g., "ONE HUNDRED AND TWENTY-THREE")
        return $"{result} AND {ConvertUnderHundred(remain)}";
    }

    private static string ConvertWholeNumber(long number)
    {
        if (number == 0)
        {
            return Numbers[0];
        }

        var components = new List<string>();

        // Millions
        if (number >= 1000000)
        {
            var millions = number / 1000000;

            components.Add(ConvertUnderThousand((int)millions) + " MILLION");
            number %= 1000000;
        }
        
        // Thousands
        if (number >= 1000)
        {
            var thousands = number / 1000;
            
            components.Add(ConvertUnderThousand((int)thousands) + " THOUSAND");
            number %= 1000;
        }

        if (number > 0)
        {
            // Thousands + AND + two digits number (e.g., "ONE THOUSAND AND TWENTY-THREE")
            if (components.Count > 0 && number < 100)
            {
                components.Add($"AND {ConvertUnderThousand((int)number)}");
            }
            else
            {
                components.Add(ConvertUnderThousand((int)number));
            }
        }
        
        return string.Join(" ", components);
    }

    public string Convert(decimal amount)
    {
        Validate(amount);

        var wholeNumber = (long)decimal.Truncate(amount);
        var smallNumber = (int)((amount - wholeNumber) * 100);
        
        var wholeNumberInWords = ConvertWholeNumber(wholeNumber);
        var dollarTail = wholeNumber == 1 ? "DOLLAR" : "DOLLARS";
        
        var convertedDollarWords = $"{wholeNumberInWords} {dollarTail}";

        if (smallNumber == 0)
        {
            return convertedDollarWords;
        }

        var smallNumberInWords = ConvertUnderHundred(smallNumber);

        var centTail = smallNumber == 1 ? "CENT" : "CENTS";

        return $"{convertedDollarWords} AND {smallNumberInWords} {centTail}";
    }

    private static void Validate(decimal amount)
    {
        if (amount < 0 || amount > MaxValue)
        {
            throw new NumberToWordsException($"Amount must be between 0 and {MaxValue}.");
        }

        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];

        if (decimalPlaces > 2)
        {
            throw new NumberToWordsException("Amount must have at most two decimal places.");
        }
    }
}
