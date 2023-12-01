using System;

public static class NumberMapSango
{
    public static int GetNumberConvertProtocol_ASCII_A0a26(char inputChar)
    {
        if (inputChar >= 'A' && inputChar <= 'Z')
        {
            return Convert.ToInt32(inputChar) - Convert.ToInt32('A');
        }
        else if(inputChar >= 'a' && inputChar <= 'z')
        {
            return Convert.ToInt32(inputChar) - Convert.ToInt32('a') + 26;
        }
        else
        {
            return -1;
        }
    }

    public static char GetCharConverterProtocol_ASCII_A0a26(int inputNum)
    {
        if (inputNum >= 0 && inputNum <= 25)
        {
            return Convert.ToChar(inputNum + Convert.ToInt32('A'));
        }
        else if (inputNum >= 26 && inputNum <= 51)
        {
            return Convert.ToChar(inputNum - 26 + Convert.ToInt32('a'));
        }
        else
        {
            return '\0';
        }
    }
}
