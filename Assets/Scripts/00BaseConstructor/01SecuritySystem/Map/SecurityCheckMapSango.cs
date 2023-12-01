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

    public static void CheckProtocol_A_B_C_SIGN(string mixSignData, SecurityCheckServiceConfig config, Action<string> writeRegistInfoCallBack)
    {
        if (mixSignData.Length != 3 + config.checkLength)
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_LenghthError);
            return;
        }
        int numYearPostNum = NumberUtilsSango.GetNumberFormNumberMapChar(mixSignData[0], NumberConvertProtocol.ASCII_A0a26);
        if (numYearPostNum == -1)
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_LenghthError);
            return;
        }
        int numYear = 2000 + numYearPostNum;
        int numMonth = NumberUtilsSango.GetNumberFormNumberMapChar(mixSignData[1], NumberConvertProtocol.ASCII_A0a26);
        int numDay = NumberUtilsSango.GetNumberFormNumberMapChar(mixSignData[2], NumberConvertProtocol.ASCII_A0a26);        
        DateTime newRegistLimitDateTime = TimeUtils.GetDateTimeFromDateNumer(numYear, numMonth, numDay);
        if (newRegistLimitDateTime == DateTime.MinValue)
        {
            config.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError);
            return;
        }
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
    SIGN,
    A_B_C_SIGN
}
