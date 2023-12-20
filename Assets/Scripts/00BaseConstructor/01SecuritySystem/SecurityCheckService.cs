using System;
using UnityEngine;

public class SecurityCheckService : BaseService<SecurityCheckService>
{
    private SecurityCheckServiceConfig _securityCheckServiceConfig = null;

    private string _limitTimestampKey = "key1";
    private string _lastRunTimestampKey = "key2";

    private float _currentTickTime = 1;
    private float _maxTickTime = 60;

    private bool _isApplicationRunValid = false;

    public void OnInit(SecurityCheckServiceConfig config)
    {
        base.OnInit();
        if (config != null)
        {
            _securityCheckServiceConfig = config;
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_isApplicationRunValid)
        {
            if (_currentTickTime > 0)
            {
                _currentTickTime -= Time.deltaTime;
            }
            else
            {
                _currentTickTime = _maxTickTime;
                TickUpdateRunTime();
            }
        }
    }

    public void UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        if (string.IsNullOrEmpty(registLimitTimestampNew) || string.IsNullOrEmpty(signData))
        {
            _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo, "");
            return;
        }
        if (!long.TryParse(registLimitTimestampNew, out long result))
        {
            _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError, "");
            return;
        }
        switch (_securityCheckServiceConfig.registMixSignDataProtocol)
        {
            case RegistMixSignDataProtocol.SIGN:
                SecurityCheckMapSango.CheckProtocl_SIGNDATA(registLimitTimestampNew, signData, _securityCheckServiceConfig, WriteRegistInfo);
                break;
        }
    }

    public void UpdateRegistInfo(string mixSignData)
    {
        if (string.IsNullOrEmpty(mixSignData))
        {
            _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo, "");
            return;
        }
        switch (_securityCheckServiceConfig.registMixSignDataProtocol)
        {
            case RegistMixSignDataProtocol.A_B_C_SIGN:
                SecurityCheckMapSango.CheckProtocol_A_B_C_SIGN(mixSignData, _securityCheckServiceConfig, WriteRegistInfo);
                break;
        }
    }

    private void WriteRegistInfo(string registLimitTimestampNew)
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
        if (Convert.ToInt64(registLimitTimestampNew) > nowTimestamp)
        {
            string registLimitTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(registLimitTimestampNew);
            string registLastRunTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampDataNew);
            bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampDataNew);
            if (res1 && res2)
            {
                _isApplicationRunValid = true;
                DateTime registNewLimitDateTime = TimeUtils.GetDateTimeFromTimestamp(registLimitTimestampNew);
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateOK_Success, registNewLimitDateTime.ToString("yyyy-MM-dd"));
            }
            else
            {
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError, "");
            }
        }
        else
        {
            SangoLogger.Warning("RegistFaild, the NewRegistLimitTimestamp should newer than NowTimestamp.");
            _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_OutData, "");
        }
    }

    public void CheckRegistValidation()
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
        long defaultRegistLimitTimestamp = TimeUtils.GetUnixDateTimeSeconds(_securityCheckServiceConfig.defaultRegistLimitDateTime);

        string registLimitTimestampData = PersistDataService.Instance.GetPersistData(_limitTimestampKey);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
        SangoLogger.Processing("Now is time to Find the RegistInfo, please wait....................................");
        SangoLogger.Log("The RegistLimitTimestampInfo Found: [ " + registLimitTimestampData + " ]");
        SangoLogger.Log("The LastRunTimestampInfo Found: [ " + registLastRunTimestampData + " ]");

        if (string.IsNullOrEmpty(registLimitTimestampData) || string.IsNullOrEmpty(registLastRunTimestampData))
        {
            bool res = false;
            registLimitTimestampData = TimeCryptoUtils.EncryptTimestamp(defaultRegistLimitTimestamp);
            registLastRunTimestampData = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            SangoLogger.Warning("That`s the First Time open this software, we give the default registLimitTimestamp is: [ " + defaultRegistLimitTimestamp + " ]");
            bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampData);
            bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampData);
            if (res1 && res2)
            {
                res = true;
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckOK_FirstRun, "");
            }
            else
            {
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError, "");
            }
            SangoLogger.Done("Is first regist OK? [ " + res + " ]");
        }
        else
        {
            long registLimitTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLimitTimestampData));
            long registLastRunTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
            SangoLogger.Processing("We DeCrypt the RegistInfo, please wait....................................");
            SangoLogger.Log("The RegistLimitTimestamp is: [ " + registLimitTimestamp + " ]");
            SangoLogger.Log("The LastRunTimestamp is: [ " + registLastRunTimestamp + " ]");
            SangoLogger.Log("The NowTimestamp is: [ " + nowTimestamp + " ]");
            if (nowTimestamp < registLastRunTimestamp)
            {
                SangoLogger.Error("Error: SystemTime has in Changed");
                _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckError_SystemTimeChanged, "");
            }
            else
            {
                if (nowTimestamp < registLimitTimestamp)
                {
                    long timestampSpan = Math.Abs(registLimitTimestamp - nowTimestamp);
                    int daySpan = Convert.ToInt32(timestampSpan / (24 * 60 * 60));
                    if (daySpan <= 3)
                    {
                        _isApplicationRunValid = true;
                        _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckWarnning_ValidationLessThan3Days, daySpan.ToString());
                    }
                    else
                    {
                        _isApplicationRunValid = true;
                        _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckOK_Valid, "");
                    }
                }
                else
                {
                    _securityCheckServiceConfig.resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckFailed_OutData, "");
                }
            }
        }
    }

    private void TickUpdateRunTime()
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
        if (string.IsNullOrEmpty(registLastRunTimestampData))
        {
            return;
        }
        long lastRunTimetamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
        if (lastRunTimetamp > nowTimestamp)
        {
            return;
        }
        else
        {
            string registLastRunTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampDataNew);
        }
    }
}

public enum SignMethodCode
{
    Md5
}

public enum RegistInfoCode
{
    Timestamp
}

public enum RegistInfoCheckResult
{
    CheckOK_Valid,
    CheckOK_FirstRun,
    CheckWarnning_ValidationLessThan3Days,
    CheckFailed_OutData,
    CheckError_SystemTimeChanged,
    UpdateOK_Success,
    UpdateFailed_OutData,
    UpdateFailed_SignError,
    UpdateFailed_WriteInfoError,
    UpdateError_NullInfo,
    UpdateError_SyntexError,
    UpdateError_LenghthError
}

public class SecurityCheckServiceConfig : BaseConfig
{
    public string apiKey;
    public string apiSecret;
    public string secretTimestamp;
    public DateTime defaultRegistLimitDateTime;
    public SignMethodCode signMethodCode;
    public RegistInfoCode registInfoCode;
    public int checkLength;
    public RegistMixSignDataProtocol registMixSignDataProtocol;
    public Action<RegistInfoCheckResult, string> resultActionCallBack;
}