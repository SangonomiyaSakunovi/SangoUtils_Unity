using SangoNetProtol;
using System;
using UnityEngine;

public abstract class BaseNetBroadcast
{
    public NetOperationCode NetOperationCode { get; protected set; } = NetOperationCode.Default;

    public abstract void DefaultOperationBroadcast();

    public abstract void OnBroadcast(string message);

    public virtual void OnInit<T>(NetOperationCode netOperationCode, BaseNetService<T> instance) where T : MonoBehaviour
    {
        NetOperationCode = netOperationCode;
        instance.AddNetBroadcast(this);
    }

    public virtual void OnDispose<T>(BaseNetService<T> instance) where T : MonoBehaviour
    {
        instance.RemoveNetBroadcast(this);
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
