using System;

public static class NumberMapSango
{
    public static int GetNumberFormConvertProtocol_ASCII_A_1(char inputChar)
    {
        if (inputChar >= 'A' && inputChar <= 'Z')
        {
            return Convert.ToInt32(inputChar) - Convert.ToInt32('A') + 1;
        }
        else
        {
            return -1;
        }
    }
}
