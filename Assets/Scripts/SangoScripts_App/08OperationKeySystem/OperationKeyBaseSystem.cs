using SangoUtils_Common.Messages;
using System;
using UnityEngine;

public abstract class OperationKeyBaseSystem<T> : BaseSystem<T> where T : MonoBehaviour
{
    protected OperationKeyType OperationKeyType { get; set; } = OperationKeyType.None;

    protected void SetAndSendOperationKey(string jsonString)
    {
        OperationKeyCoreSystem.Instance.SetAndSendOperationKey(OperationKeyType, jsonString);
    }

    public abstract void OnMessageReceived(OperationKeyReqMessage reqMessage);

    protected string SetJsonString(object ob)
    {
        return JsonUtils.SetJsonString(ob);
    }

    protected K DeJsonString<K>(string str)
    {
        K t;
        try
        {
            t = JsonUtils.DeJsonString<K>(str);
        }
        catch (Exception)
        {
            throw;
        }
        return t;
    }
}
