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

    public bool UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        bool res = false;
        if (CheckSignDataValid(registLimitTimestampNew, signData))
        {
            if (Convert.ToInt64(registLimitTimestampNew) > TimeUtils.GetUnixDateTimeSeconds(DateTime.Now))
            {
                string registLastRunTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(registLimitTimestampNew);
                res = PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLastRunTimestampDataNew);
                if (res)
                {
                    _isApplicationRunValid = true;
                }
            }
            else
            {
                Debug.Log("RegistFaild, the NewRegistLimitTimestamp should newer than NowTimestamp.");
            }
        }
        return res;
    }

    public bool CheckRegistValidation()
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);

        string registLimitTimestampData = PersistDataService.Instance.GetPersistData(_limitTimestampKey);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(_lastRunTimestampKey);

        if (string.IsNullOrEmpty(registLimitTimestampData) || string.IsNullOrEmpty(registLastRunTimestampData))
        {
            registLimitTimestampData = TimeCryptoUtils.EncryptTimestamp(_defaultRegistLimitTimestamp);
            registLastRunTimestampData = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            Debug.Log("That`s the First Time open this software, we give the default registLimitTimestamp is: [ " + _defaultRegistLimitTimestamp + " ]");
            PersistDataService.Instance.AddPersistData(_limitTimestampKey, registLimitTimestampData);
            PersistDataService.Instance.AddPersistData(_lastRunTimestampKey, registLastRunTimestampData);
            return true;
        }
        else
        {
            long registLimitTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLimitTimestampData));
            long registLastRunTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
            Debug.Log("The RegistLimitTimestamp is: [ " + registLimitTimestamp + " ]");
            Debug.Log("The LastRunTimestamp is: [ " + registLastRunTimestamp + " ]");
            Debug.Log("The NowTimestamp is: [ " + nowTimestamp + " ]");
            if (nowTimestamp < registLastRunTimestamp)
            {
                Debug.LogError("Error: SystemTime has in Changed");
                return false;
            }
            else
            {
                if (nowTimestamp < registLimitTimestamp)
                {
                    _isApplicationRunValid = true;
                    return true;
                }
                else
                {
                    return false;
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

public class SecurityCheckServiceConfig
{
    public string apiKey;
    public string apiSecret;
    public string secretTimestamp;
    public string defaultRegistLimitTimestamp;
    public SignMethodCode signMethodCode;
    public RegistInfoCode registInfoCode;
}