using UnityEngine;

public class _09Root : BaseRoot
{
    public _09Window _09Window;

    private void Awake()
    {
        SecurityCheckServiceConfig config = new SecurityCheckServiceConfig
        {
            apiKey = "SangoSecurityRegistKey",
            apiSecret = "SangoSecurityRegistSecret",
            defaultRegistLimitTimestamp = "",
            secretTimestamp = "1645467742",
            registInfoCode = RegistInfoCode.Timestamp,
            signMethodCode = SignMethodCode.Md5
        };
        
        InitSecurityCheckService(config);
        bool res = CheckRegistValidation();
        Debug.Log("当前注册情况: [ " + res + " ]");
        if (res)
        {
            _09Window.SetWindowState(false);
        }
        else
        {
            _09Window.SetRoot(this);
            _09Window.SetWindowState();            
        }

        GetNewRegistInfo("1700859600");
    }

    public override bool UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        bool res = base.UpdateRegistInfo(registLimitTimestampNew, signData);
        Debug.Log("是否更新成功注册信息: [ " + res + " ]");
        if (res)
        {
            _09Window.SetWindowState(false);
        }
        return res;
    }
}
