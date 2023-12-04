public static class SpecialCharUtilsSango
{
    public static char GetCharFromCharConvertProtocol(char inputChar, SpecialCharConvertProtocol protocol)
    {
        char res = '\0';
        switch (protocol)
        {
            case SpecialCharConvertProtocol.ASCII_Aspace:
                res = SpecialCharMapSango.GetCharConventerProtocol_ASCII_Aspace(inputChar);
                break;
        }
        return res;
    }
}

public enum SpecialCharConvertProtocol
{
    ASCII_Aspace,

}
