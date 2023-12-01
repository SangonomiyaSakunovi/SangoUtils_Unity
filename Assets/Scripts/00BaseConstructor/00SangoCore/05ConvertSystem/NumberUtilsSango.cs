public static class NumberUtilsSango
{
    public static int GetNumberFormNumberMapChar(char inputChar, NumberConvertProtocol protocol)
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
}

public enum NumberConvertProtocol
{
    ASCII_A0a26,
}
