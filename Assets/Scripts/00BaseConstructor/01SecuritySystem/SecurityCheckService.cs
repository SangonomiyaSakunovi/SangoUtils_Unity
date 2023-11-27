using System;
using UnityEngine;

public class SecurityCheckService : BaseService<SecurityCheckService>
{
    private string _secretTimestamp = "1645467742";
    private string _defaultRegistLimitTimestamp = "1645467742";

    private string _apiKey = "SangoSecurityRegistKey";
    private string _apiSecret = "SangoSecurityRegistSecret";

    private SignMethodCode _signMethodCode = SignMethodCode.Md5;
    private RegistInfoCode _registInfoCode = RegistInfoCode.Timestamp;

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
            _secretTimestamp = config.secretTimestamp;
            _defaultRegistLimitTimestamp = config.defaultRegistLimitTimestamp;
            _apiKey = config.apiKey;
            _apiSecret = config.apiSecret;
            _signMethodCode = config.signMethodCode;
            _registInfoCode = config.registInfoCode;
            _resultActionCallBack = config.resultActionCallBack;
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
        
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);
        if (CheckSignDataValid(registLimitTimestampNew, signData))
        {
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
        else
        {
            _resultActionCallBack?.Invoke(RegistInfoCheckResult.UpdateFailed_SignError);
        }
    }

    public void CheckRegistValidation()
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);

        string registLimitTimestampData = PersistDataService.Instance.GetPersistData(_limitTimestampKey);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);
        Debug.Log("Now is time to Find the RegistInfo, please wait....................................");
        Debug.Log("The RegistLimitTimestampInfo Found: [ " + registLimitTimestampData + " ]");
        Debug.Log("The LastRunTimestampInfo Found: [ " + registLastRunTimestampData + " ]");

        if (string.IsNullOrEmpty(registLimitTimestampData) || string.IsNullOrEmpty(registLastRunTimestampData))
        {
            bool res = false;
            registLimitTimestampData = TimeCryptoUtils.EncryptTimestamp(_defaultRegistLimitTimestamp);
            registLastRunTimestampData = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            Debug.Log("That`s the First Time open this software, we give the default registLimitTimestamp is: [ " + _defaultRegistLimitTimestamp + " ]");
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
        switch (_signMethodCode)
        {
            case SignMethodCode.Md5:
                signData = Md5SignatureUtils.GenerateMd5SignData(rawData, _secretTimestamp, _apiKey, _apiSecret);
                break;
        }
        Debug.Log("Generate New SignRegistInfo, please wait....................................");
        Debug.Log("====================SignRawData====================");
        Debug.Log(rawData);
        Debug.Log("==================================================");
        Debug.Log("SignMethod: [ " + _signMethodCode + " ]");
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

    private bool CheckSignDataValid(string rawData, string signData)
    {
        bool res = false;
        switch (_signMethodCode)
        {
            case SignMethodCode.Md5:
                res = Md5SignatureUtils.CheckMd5SignDataValid(rawData, signData, _secretTimestamp, _apiKey, _apiSecret);
                break;
        }
        return res;
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
    UpdateError_SyntexError
}

public class SecurityCheckServiceConfig
{
    public string apiKey;
    public string apiSecret;
    public string secretTimestamp;
    public string defaultRegistLimitTimestamp;
    public SignMethodCode signMethodCode;
    public RegistInfoCode registInfoCode;
    public Action<RegistInfoCheckResult> resultActionCallBack;
}