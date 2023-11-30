using System;

public static class SecurityCheckMapSango
{
    private static bool CheckSignDataValid(string rawData, string signData, SecurityCheckServiceConfig config)
    {
        bool res = false;
        switch (config.signMethodCode)
        {
            case SignMethodCode.Md5:
                res = Md5SignatureUtils.CheckMd5SignDataValid(rawData, signData, config.secretTimestamp, config.apiKey, config.apiSecret, config.checkLength);
                break;
        }
        return res;
    }
    private static bool CheckSignDataValid(long rawData, string signData, SecurityCheckServiceConfig config)
    {
        return CheckSignDataValid(rawData.ToString(), signData, config);
    }

    public static void CheckProtocl_SIGNDATA(string registLimitTimestampNew, string signData, SecurityCheckServiceConfig config, Action<string> writeRegistInfoCallBack)
    {
        if (CheckSignDataValid(registLimitTimestampNew, signData, config))
        {
            writeRegistInfoCallBack?.Invoke(registLimitTimestampNew);
        }
        else
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_SignError);
        }
    }

    public static void CheckProtocol_AA_B_SIGNDATA(string mixSignData, SecurityCheckServiceConfig config, Action<string> writeRegistInfoCallBack)
    {
        if (mixSignData.Length != 3 + config.checkLength)
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_LenghthError);
            return;
        }
        bool decodeRes = true;
        int numYearIndex3 = NumberUtilsSango.GetNumberFormNumberMapChar(mixSignData[0], NumberConvertProtocol.ASCII_A_1);
        if (numYearIndex3 < 0 || numYearIndex3 > 9) { decodeRes = false; }
        int numYearIndex4 = NumberUtilsSango.GetNumberFormNumberMapChar(mixSignData[1], NumberConvertProtocol.ASCII_A_1);
        if (numYearIndex4 < 0 || numYearIndex4 > 9) { decodeRes = false; }
        int numMonth = NumberUtilsSango.GetNumberFormNumberMapChar(mixSignData[2], NumberConvertProtocol.ASCII_A_1);
        if (numMonth < 1 || numMonth > 12) { decodeRes = false; }
        int numDay = config.defaultRegistLimitDateTime.Day;
        if (!decodeRes)
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError);
            return;
        }
        string yearStr = "20" + numYearIndex3 + numYearIndex4;
        int numYear = int.Parse(yearStr);
        DateTime newRegistLimitDateTime = TimeUtils.GetDateTimeFromDateNumer(numYear, numMonth, numDay);
        string md5DataStr = mixSignData.Substring(3, config.checkLength);
        long registLimitTimestampNew = TimeUtils.GetUnixDateTimeSeconds(newRegistLimitDateTime);
        if (CheckSignDataValid(registLimitTimestampNew, md5DataStr, config))
        {
            writeRegistInfoCallBack?.Invoke(registLimitTimestampNew.ToString());
        }
        else
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_SignError);
        }
    }
}


public enum RegistMixSignDataProtocol
{
    SIGNDATA,
    AA_B_SIGNDATA
}
