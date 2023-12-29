public static class NumberUtilsSango
{
    public static int GetNumberFromNumberConvertProtocol(char inputChar, NumberConvertProtocol protocol)
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

    public static char GetCharFromNumberConvertProtocol(int inputNumber, NumberConvertProtocol protocol)
    {
        char res = '\0';
        switch (protocol)
        {
            case NumberConvertProtocol.ASCII_A0a26:
                res = NumberMapSango.GetCharConvertProtocol_ASCII_A0a26(inputNumber);
                break;
        }
        return res;
    }
}

public enum NumberConvertProtocol
{
    ASCII_A0a26,
}
