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

    protected bool UpdateRegistInfo(string registLimitTimestampNew, string signData)
    {
        return SecurityCheckService.Instance.UpdateRegistInfo(registLimitTimestampNew, signData);
    }
}
