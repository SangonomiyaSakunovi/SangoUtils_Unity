using System.Text;

public static class SecuritySignConvertUtilsSango
{
    public static string GetSecuritySignInfoFromSrcuritySignConvertProtocol(string rawSignData, SecuritySignConvertProtocol protocol)
    {
        string res = "";
        switch (protocol)
        {
            case SecuritySignConvertProtocol.AllToUpperChar:
                res = GetSecuritySignInfoFromSrcuritySignConvertProtocol_AllToUpperChar(rawSignData);
                break;
            case SecuritySignConvertProtocol.RawData:
                res = rawSignData;
                break;
        }
        return res;
    }

    private static string GetSecuritySignInfoFromSrcuritySignConvertProtocol_AllToUpperChar(string rawSignData)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < rawSignData.Length; i++)
        {
            char inputChar = rawSignData[i];
            if (char.IsDigit(inputChar))
            {
                int digit = (int)char.GetNumericValue(inputChar);
                char digitToUpperOrLower = NumberUtilsSango.GetCharFromNumberConvertProtocol(digit, NumberConvertProtocol.ASCII_A0a26);
                char digitToUpper = CharUtilsSango.GetCharFromCharToCharProrocol(digitToUpperOrLower, CharConvertProtocol.ToUpper);
                sb.Append(digitToUpper);
            }
            else if (char.IsLower(inputChar))
            {
                char lowerToUpper = CharUtilsSango.GetCharFromCharToCharProrocol(inputChar, CharConvertProtocol.ToUpper);
                sb.Append(lowerToUpper);
            }
            else if (char.IsUpper(inputChar))
            {
                sb.Append(inputChar);
            }
            else
            {
                char specialCharToUpper = SpecialCharUtilsSango.GetCharFromCharConvertProtocol(inputChar, SpecialCharConvertProtocol.ASCII_Aspace);
                sb.Append(specialCharToUpper);
            }
        }
        return sb.ToString();
    }
}

public enum SecuritySignConvertProtocol
{
    RawData,
    AllToUpperChar,
}
