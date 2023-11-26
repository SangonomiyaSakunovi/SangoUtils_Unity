using UnityEngine;

public class BaseRoot : MonoBehaviour
{    
    public virtual void InitRoot()
    {

    }

    protected bool CheckRegistValidation()
    {
        return SecurityCheckService.Instance.CheckRegistValidation();
    }

    public virtual bool UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        return SecurityCheckService.Instance.UpdateRegistInfo(registLimitTimestampNew, signData);
    }

    protected void GetNewRegistInfo(string rawData)
    {
        SecurityCheckService.Instance.GetNewRegistInfo(rawData);
    }

    protected void InitSecurityCheckService(SecurityCheckServiceConfig config)
    {
        SecurityCheckService.Instance.OnInit(config);
    }
}
