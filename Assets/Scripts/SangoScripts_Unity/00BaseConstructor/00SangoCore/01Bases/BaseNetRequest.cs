using SangoNetProtol;
using System;

public abstract class BaseNetRequest
{
    public NetOperationCode NetOperationCode { get; private set; }

    public abstract void DefaultOperationRequest();

    public abstract void OnOperationResponse(string message);

    public virtual void OnInit(NetOperationCode netOperationCode)
    {
        NetOperationCode = netOperationCode;
        NetService.Instance.AddNetRequest(this);
    }

    public virtual void OnDispose()
    {
        NetService.Instance.RemoveNetRequest(this);
    }

    protected static string SetJsonString(object ob)
    {
        return JsonUtils.SetJsonString(ob);
    }

    public static T DeJsonString<T>(string str)
    {
        T t;
        try
        {
            t = JsonUtils.DeJsonString<T>(str);
        }
        catch (Exception)
        {
            throw;
        }
        return t;
    }
}
