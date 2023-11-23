using System;
using UnityEngine;

public class SecurityCheckService : BaseService<SecurityCheckService>
{
    private const string newRegistLimitTimeStamp = "1645467742";
    private const string _defaultRegistLimitTimestamp = "1645467742";

    private const string apiKey = "SangoSecurityRegistKey";
    private const string apiSecret = "SangoSecurityRegistSecret";

    private SignMethodCode _signMethodCode = SignMethodCode.Md5;

    private const string limitTimestampKey = "key1";
    private const string lastRunTimestampKey = "key2";

    private float _currentTickTime = 1;
    private float _maxTickTime = 60;

    private void Awake()
    {
        PublishNewSignData("");
    }

    public override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
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

    public bool UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        bool res = false;
        if (CheckSignDataValid(registLimitTimestampNew, signData))
        {
            string registLastRunTimestampDataNew = TimeCryptoUtils.EncryptTimestamp(registLimitTimestampNew);
            res = PersistDataService.Instance.AddPersistData(limitTimestampKey, registLastRunTimestampDataNew);
        }
        return res;
    }

    public bool CheckRegistValidation()
    {
        long nowTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now);

        string registLimitTimestampData = PersistDataService.Instance.GetPersistData(limitTimestampKey);
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(lastRunTimestampKey);

        if (String.IsNullOrEmpty(registLimitTimestampData) || String.IsNullOrEmpty(registLastRunTimestampData))
        {
            registLimitTimestampData = TimeCryptoUtils.EncryptTimestamp(_defaultRegistLimitTimestamp);
            registLastRunTimestampData = TimeCryptoUtils.EncryptTimestamp(nowTimestamp);
            PersistDataService.Instance.AddPersistData(limitTimestampKey, registLimitTimestampData);
            PersistDataService.Instance.AddPersistData(lastRunTimestampKey, registLastRunTimestampData);
            return true;
        }
        else
        {
            long registLimitTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLimitTimestampData));
            long registLastRunTimestamp = Convert.ToInt64(TimeCryptoUtils.DecryptTimestamp(registLastRunTimestampData));
            if (nowTimestamp < registLastRunTimestamp)
            {
                Debug.LogError("Error: SystemTime has in Changed");
                return false;
            }
            else
            {
                if (nowTimestamp < registLimitTimestamp)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void PublishNewSignData(string rawData)
    {
        string signData = "";
        switch (_signMethodCode)
        {
            case SignMethodCode.Md5:
                signData = Md5SignatureUtils.GenerateMd5SignData(rawData, newRegistLimitTimeStamp, apiKey, apiSecret);
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
        string registLastRunTimestampData = PersistDataService.Instance.GetPersistData(lastRunTimestampKey);
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
            PersistDataService.Instance.AddPersistData(lastRunTimestampKey, registLastRunTimestampDataNew);
        }
    }

    private bool CheckSignDataValid(string rawData, string signData)
    {
        bool res = false;
        switch (_signMethodCode)
        {
            case SignMethodCode.Md5:
                res = Md5SignatureUtils.CheckMd5SignDataValid(rawData, signData, newRegistLimitTimeStamp, apiKey, apiSecret);
                break;
        }
        return res;
    }
}

public enum SignMethodCode
{
    Md5
}
