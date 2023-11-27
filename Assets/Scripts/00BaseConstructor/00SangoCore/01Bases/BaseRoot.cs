using UnityEngine;

public class BaseRoot : MonoBehaviour
{    
    public virtual void InitRoot()
    {

    }

    protected void CheckRegistValidation()
    {
        SecurityCheckService.Instance.CheckRegistValidation();
    }

    public void UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        SecurityCheckService.Instance.UpdateRegistInfo(registLimitTimestampNew, signData);
    }

    protected void GetNewRegistInfo(string rawData)
    {
        SecurityCheckService.Instance.GetNewRegistInfo(rawData);
    }

    protected void InitSecurityCheckService(SecurityCheckServiceConfig config)
    {
        SecurityCheckService.Instance.OnInit(config);
    }

    public virtual void RegistInfoCheckResultActionCallBack(RegistInfoCheckResult result)
    {

    }

    protected virtual void OnSecurityCheckResultValid()
    {

    }
}
