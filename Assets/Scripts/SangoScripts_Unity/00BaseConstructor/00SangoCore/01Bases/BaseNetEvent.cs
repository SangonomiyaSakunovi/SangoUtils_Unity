using SangoNetProtol;
using System;
using UnityEngine;

public abstract class BaseNetEvent
{
    public NetOperationCode NetOperationCode { get; protected set; } = NetOperationCode.Default;

    public abstract void OnEventData(string message);

    public virtual void OnInit<T>(NetOperationCode netOperationCode, BaseNetService<T> instance) where T : MonoBehaviour
    {
        NetOperationCode = netOperationCode;
        instance.AddNetEvent(this);
    }

    public virtual void OnDispose<T>(BaseNetService<T> instance) where T : MonoBehaviour
    {
        instance.RemoveNetEvent(this);
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
