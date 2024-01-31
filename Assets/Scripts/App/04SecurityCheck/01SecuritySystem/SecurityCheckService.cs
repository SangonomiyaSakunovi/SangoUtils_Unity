using SangoUtils_Bases_UnityEngine;
using SangoUtils_Bases_Universal;
using SangoUtils_Extensions_Universal.Utils;
using SangoUtils_Logger;
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

    public override void OnInit()
    {
    }

    public void InitService(SecurityCheckServiceConfig config)
    {
        if (config != null)
        {
            _securityCheckServiceConfig = config;
        }
    }

    protected override void OnUpdate()
    {
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
            _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo, "");
            return;
        }
        if (!long.TryParse(registLimitTimestampNew, out long result))
        {
            _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError, "");
            return;
        }
        switch (_securityCheckServiceConfig.RegistMixSignDataProtocol)
        {
            case RegistMixSignDataProtocol.SIGN:
                SecurityCheckUtils.CheckProtocl_SIGNDATA(registLimitTimestampNew, signData, _securityCheckServiceConfig, WriteRegistInfo);
                break;
        }
    }

    public void UpdateRegistInfo(string mixSignData)
    {
        if (string.IsNullOrEmpty(mixSignData))
        {
            _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo, "");
            return;
        }
        switch (_securityCheckServiceConfig.RegistMixSignDataProtocol)
        {
            case RegistMixSignDataProtocol.A_B_C_SIGN:
                SecurityCheckUtils.CheckProtocol_A_B_C_SIGN(mixSignData, _securityCheckServiceConfig, WriteRegistInfo);
                break;
        }
    }

    private void WriteRegistInfo(string registLimitTimestampNew)
    {
        long nowTimestamp = DateTime.Now.ToUnixTimestamp();
        if (Convert.ToInt64(registLimitTimestampNew) > nowTimestamp)
        {
            string registLimitTimestampDataNew = DateTimeUtils.ToBase64(registLimitTimestampNew);
            string registLastRunTimestampDataNew = DateTimeUtils.ToBase64(nowTimestamp);
            bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampDataNew);
            bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampDataNew);
            if (res1 && res2)
            {
                _isApplicationRunValid = true;
                DateTime registNewLimitDateTime = DateTimeUtils.FromUnixTimestampString(registLimitTimestampNew);
                _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateOK_Success, registNewLimitDateTime.ToString("yyyy-MM-dd"));
            }
            else
            {
                _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError, "");
            }
        }
        else
        {
            SangoLogger.Warning("RegistFaild, the NewRegistLimitTimestamp should newer than NowTimestamp.");
            _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateFailed_OutData, "");
        }
    }

    public void CheckRegistValidation()
    {
        long nowTimestamp = DateTime.Now.ToUnixTimestamp();
        long defaultRegistLimitTimestamp = _securityCheckServiceConfig.DefaultRegistLimitDateTime.ToUnixTimestamp();

        string registLimitTimestampData = PersistDataService.Instance.GetPersistData(_limitTimestampKey);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
        SangoLogger.Processing("Now is time to Find the RegistInfo, please wait....................................");
        SangoLogger.Log("The RegistLimitTimestampInfo Found: [ " + registLimitTimestampData + " ]");
        SangoLogger.Log("The LastRunTimestampInfo Found: [ " + registLastRunTimestampData + " ]");

        if (string.IsNullOrEmpty(registLimitTimestampData) || string.IsNullOrEmpty(registLastRunTimestampData))
        {
            bool res = false;
            registLimitTimestampData = DateTimeUtils.ToBase64(defaultRegistLimitTimestamp);
            registLastRunTimestampData = DateTimeUtils.ToBase64(nowTimestamp);
            SangoLogger.Warning("That`s the First Time open this software, we give the default registLimitTimestamp is: [ " + defaultRegistLimitTimestamp + " ]");
            bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampData);
            bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampData);
            if (res1 && res2)
            {
                res = true;
                _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.CheckOK_FirstRun, "");
            }
            else
            {
                _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError, "");
            }
            SangoLogger.Done("Is first regist OK? [ " + res + " ]");
        }
        else
        {
            long registLimitTimestamp = Convert.ToInt64(DateTimeUtils.FromBase64ToString(registLimitTimestampData));
            long registLastRunTimestamp = Convert.ToInt64(DateTimeUtils.FromBase64ToString(registLastRunTimestampData));
            SangoLogger.Processing("We DeCrypt the RegistInfo, please wait....................................");
            SangoLogger.Log("The RegistLimitTimestamp is: [ " + registLimitTimestamp + " ]");
            SangoLogger.Log("The LastRunTimestamp is: [ " + registLastRunTimestamp + " ]");
            SangoLogger.Log("The NowTimestamp is: [ " + nowTimestamp + " ]");
            if (nowTimestamp < registLastRunTimestamp)
            {
                SangoLogger.Error("Error: SystemTime has in Changed");
                _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.CheckError_SystemTimeChanged, "");
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
                        _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.CheckWarnning_ValidationLessThan3Days, daySpan.ToString());
                    }
                    else
                    {
                        _isApplicationRunValid = true;
                        _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.CheckOK_Valid, "");
                    }
                }
                else
                {
                    _securityCheckServiceConfig.OnCheckedResult?.Invoke(RegistInfoCheckResult.CheckFailed_OutData, "");
                }
            }
        }
    }

    private void TickUpdateRunTime()
    {
        long nowTimestamp = DateTime.Now.ToUnixTimestamp();
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
        if (string.IsNullOrEmpty(registLastRunTimestampData))
        {
            return;
        }
        long lastRunTimetamp = Convert.ToInt64(DateTimeUtils.FromBase64ToString(registLastRunTimestampData));
        if (lastRunTimetamp > nowTimestamp)
        {
            return;
        }
        else
        {
            string registLastRunTimestampDataNew = DateTimeUtils.ToBase64(nowTimestamp);
            PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampDataNew);
        }
    }

    public override void OnDispose()
    {
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
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string SecretTimestamp { get; set; }
    public DateTime DefaultRegistLimitDateTime { get; set; }
    public SignMethodCode SignMethodCode { get; set; }
    public RegistInfoCode RegistInfoCode { get; set; }
    public int CheckLength { get; set; }
    public RegistMixSignDataProtocol RegistMixSignDataProtocol { get; set; }
    public Action<RegistInfoCheckResult, string> OnCheckedResult { get; set; }
}