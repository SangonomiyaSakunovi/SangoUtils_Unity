public static class NumberUtilsSango
{
    public static int GetNumberFormNumberMapChar(char inputChar, NumberConvertProtocol protocol)
    {
        int res = 0;
        switch (protocol)
        {
            case NumberConvertProtocol.ASCII_A_1:
                res = NumberMapSango.GetNumberFormConvertProtocol_ASCII_A_1(inputChar);
                break;
        }
        return res;
    }
}

public enum NumberConvertProtocol
{
    ASCII_A_1,
}
