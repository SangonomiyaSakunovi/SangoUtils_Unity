public static class CharUtilsSango
{
    public static char GetCharFromCharToCharProrocol(char inputChar, CharConvertProtocol protocol)
    {
        char res = '\0';
        switch (protocol)
        {
            case CharConvertProtocol.ToUpper:
                res = CharMapSango.GetCharConvertProtocol_ToUpper(inputChar);
                break;
            case CharConvertProtocol.ToLower:
                res = CharMapSango.GetCharConvertProtocol_ToLower(inputChar);
                break;
        }
        return res;
    }
}

public enum CharConvertProtocol
{
    ToLower,
    ToUpper
}
