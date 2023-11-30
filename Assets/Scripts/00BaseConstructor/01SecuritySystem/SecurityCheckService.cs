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

    private Action<RegistInfoCheckResult> _resultActionCallBack = null;

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
            _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo);
            return;
        }
        if (!long.TryParse(registLimitTimestampNew, out long result))
        {
            _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_SyntexError);
            return;
        }
        SecurityCheckMapSango.CheckProtocl_SIGNDATA(registLimitTimestampNew, signData, _securityCheckServiceConfig, WriteRegistInfo);        
    }

    public void UpdateRegistInfo(string mixSignData)
    {
        if (string.IsNullOrEmpty(mixSignData))
        {
            _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateError_NullInfo);
            return;
        }
        switch (_securityCheckServiceConfig.registMixSignDataProtocol)
        {
            case RegistMixSignDataProtocol.AA_B_SIGNDATA:
                SecurityCheckMapSango.CheckProtocol_AA_B_SIGNDATA(mixSignData, _securityCheckServiceConfig, WriteRegistInfo);
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
                _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateOK_Success);
            }
            else
            {
                _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError);
            }
        }
        else
        {
            Debug.Log("RegistFaild, the NewRegistLimitTimestamp should newer than NowTimestamp.");
            _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_OutData);
        }
    }

    public void CheckRegistValidation()
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
        long defaultRegistLimitTimestamp = TimeUtils.GetUnixDateTimeSeconds(_securityCheckServiceConfig.defaultRegistLimitDateTime);

        string registLimitTimestampData = PersistDataService.Instance.GetPersistData(_limitTimestampKey);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
        Debug.Log("Now is time to Find the RegistInfo, please wait....................................");
        Debug.Log("The RegistLimitTimestampInfo Found: [ " + registLimitTimestampData + " ]");
        Debug.Log("The LastRunTimestampInfo Found: [ " + registLastRunTimestampData + " ]");

        if (string.IsNullOrEmpty(registLimitTimestampData) || string.IsNullOrEmpty(registLastRunTimestampData))
        {
            bool res = false;
            registLimitTimestampData = TimeCryptoUtils.EncryptTimestamp(defaultRegistLimitTimestamp);
            registLastRunTimestampData = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            Debug.Log("That`s the First Time open this software, we give the default registLimitTimestamp is: [ " + defaultRegistLimitTimestamp + " ]");
            bool res1 = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampData);
            bool res2 = PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampData);
            if (res1 && res2)
            {
                res = true;
                _resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckOK_FirstRun);
            }
            else
            {
                _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_WriteInfoError);
            }
            Debug.Log("Is first regist OK? [ " + res + " ]");
        }
        else
        {
            long registLimitTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLimitTimestampData));
            long registLastRunTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
            Debug.Log("We DeCrypt the RegistInfo, please wait....................................");
            Debug.Log("The RegistLimitTimestamp is: [ " + registLimitTimestamp + " ]");
            Debug.Log("The LastRunTimestamp is: [ " + registLastRunTimestamp + " ]");
            Debug.Log("The NowTimestamp is: [ " + nowTimestamp + " ]");
            if (nowTimestamp < registLastRunTimestamp)
            {
                Debug.LogError("Error: SystemTime has in Changed");
                _resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckError_SystemTimeChanged);
            }
            else
            {
                if (nowTimestamp < registLimitTimestamp)
                {
                    _isApplicationRunValid = true;
                    _resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckOK_Valid);
                }
                else
                {
                    _resultActionCallBack?.Invoke(RegistInfoCheckResult.CheckFailed_OutData);
                }
            }
        }
    }

    public void GetNewRegistInfo(string rawData)
    {
        string signData = "";
        switch (_securityCheckServiceConfig.signMethodCode)
        {
            case SignMethodCode.Md5:
                signData = Md5SignatureUtils.GenerateMd5SignData(rawData, _securityCheckServiceConfig.secretTimestamp, _securityCheckServiceConfig.apiKey, _securityCheckServiceConfig.apiSecret);
                break;
        }
        Debug.Log("Generate New SignRegistInfo, please wait....................................");
        Debug.Log("====================SignRawData====================");
        Debug.Log(rawData);
        Debug.Log("==================================================");
        Debug.Log("SignMethod: [ " + _securityCheckServiceConfig.signMethodCode + " ]");
        Debug.Log("====================SignedData====================");
        Debug.Log(signData);
        Debug.Log("==================================================");
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

public class SecurityCheckServiceConfig
{
    public string apiKey;
    public string apiSecret;
    public string secretTimestamp;
    public DateTime defaultRegistLimitDateTime;
    public SignMethodCode signMethodCode;
    public RegistInfoCode registInfoCode;
    public int checkLength;
    public RegistMixSignDataProtocol registMixSignDataProtocol;
    public Action<RegistInfoCheckResult> resultActionCallBack;
}