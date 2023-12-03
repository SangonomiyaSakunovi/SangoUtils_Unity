public static class NumberUtilsSango
{
    public static int GetNumberFromNumberToCharProtocol(char inputChar, NumberConvertProtocol protocol)
    {
        int res = -1;
        switch (protocol)
        {
            case NumberConvertProtocol.ASCII_A0a26:
                res = NumberMapSango.GetNumberConvertProtocol_ASCII_A0a26(inputChar);
                break;
        }
        return res;
    }

    public static char GetCharFromNumberToCharProtocol(int inputNumber, NumberConvertProtocol protocol)
    {
        char res = '\0';
        switch (protocol)
        {
            case NumberConvertProtocol.ASCII_A0a26:
                res = NumberMapSango.GetCharConverterProtocol_ASCII_A0a26(inputNumber);
                break;
        }
        return res;
    }
}

public enum NumberConvertProtocol
{
    ASCII_A0a26,
}
