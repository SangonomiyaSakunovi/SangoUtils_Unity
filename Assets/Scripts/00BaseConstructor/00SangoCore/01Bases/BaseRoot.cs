using UnityEngine;

public class BaseRoot<T> : UnitySingleton<T> where T : MonoBehaviour
{
    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnInit()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    public virtual void OnDispose()
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

    public void UpdateRegistInfo(string mixSignData)
    {
        SecurityCheckService.Instance.UpdateRegistInfo(mixSignData);
    }

    protected void InitSecurityCheckService(SecurityCheckServiceConfig config)
    {
        SecurityCheckService.Instance.OnInit(config);
    }
}
