using SangoNetProtol;
using System;

public abstract class BaseNetEvent
{
    public NetOperationCode NetOperationCode { get; private set; }

    public abstract void OnOperationEvent(string message);

    public virtual void OnInit(NetOperationCode netOperationCode)
    {
        NetOperationCode = netOperationCode;
        NetService.Instance.AddNetEvent(this);
    }

    public virtual void OnDispose()
    {
        NetService.Instance.RemoveNetEvent(this);
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
