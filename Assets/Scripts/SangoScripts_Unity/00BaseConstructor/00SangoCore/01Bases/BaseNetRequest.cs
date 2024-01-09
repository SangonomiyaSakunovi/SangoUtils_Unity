using SangoNetProtol;
using System;
using UnityEngine;

public abstract class BaseNetRequest
{
    public NetOperationCode NetOperationCode { get; protected set; } = NetOperationCode.Default;

    protected abstract void DefaultOperationRequest();

    public abstract void OnOperationResponse(string message);

    public virtual void OnInit<T>(NetOperationCode netOperationCode, BaseNetService<T> instance) where T : MonoBehaviour
    {
        NetOperationCode = netOperationCode;
        instance.AddNetRequest(this);
    }

    public virtual void OnDispose<T>(BaseNetService<T> instance) where T : MonoBehaviour
    {
        instance.RemoveNetRequest(this);
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
