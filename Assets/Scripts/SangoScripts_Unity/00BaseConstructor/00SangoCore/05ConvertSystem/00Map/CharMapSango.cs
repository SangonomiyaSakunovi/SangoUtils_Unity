public static class CharMapSango
{
    public static char GetCharConvertProtocol_ToLower(char inputChar)
    {
        if (char.IsUpper(inputChar))
        {
            return char.ToLower(inputChar);
        }
        return inputChar;
    }

    public static char GetCharConvertProtocol_ToUpper(char inputChar)
    {
        if (char.IsLower(inputChar))
        {
            return char.ToUpper(inputChar);
        }
        return inputChar;
    }
}
