using System;

public static class SpecialCharMapSango
{
    public static char GetCharConventerProtocol_ASCII_Aspace(char inputChar)
    {
        if (Convert.ToInt32(inputChar) >= 32 && Convert.ToInt32(inputChar) <= 47)
        {
            //space to A && / to P
            return Convert.ToChar(Convert.ToInt32(inputChar) + 33);
        }
        else if (Convert.ToInt32(inputChar) >= 58 && Convert.ToInt32(inputChar) <= 64)
        {
            //: to Q && @ to W
            return Convert.ToChar(Convert.ToInt32(inputChar) + 23);
        }
        else if (Convert.ToInt32(inputChar) >= 91 && Convert.ToInt32(inputChar) <= 93)
        {
            //[ to X && ] to Z
            return Convert.ToChar(Convert.ToInt32(inputChar) - 3);
        }
        else if (Convert.ToInt32(inputChar) >= 94 && Convert.ToInt32(inputChar) <= 96)
        {
            //^ to A && \ to C
            return Convert.ToChar(Convert.ToInt32(inputChar) - 29);
        }
        else if (Convert.ToInt32(inputChar) >= 123 && Convert.ToInt32(inputChar) <= 126)
        {
            //{ to D && ~ to G
            return Convert.ToChar(Convert.ToInt32(inputChar) - 55);
        }
        else
        {
            return '\0';
        }
    }
}
