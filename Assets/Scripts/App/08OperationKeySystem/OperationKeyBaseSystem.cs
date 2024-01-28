using SangoUtils_Bases_Universal;
using SangoUtils_Common.Messages;
using SangoUtils_Extensions_Universal.Utils;
using System;

namespace SangoUtils_Unity_App.Operation
{
    public abstract class OperationKeyBaseSystem<T> : BaseSystem<T> where T : class, new()
    {
        protected OperationKeyType OperationKeyType { get; set; } = OperationKeyType.None;

        protected void SetAndSendOperationKey(string jsonString)
        {
            SystemRoot.Instance.OperationKeyCoreSystem.SetAndSendOperationKey(OperationKeyType, jsonString);
        }

        public abstract void OnMessageReceived(OperationKey operationKey);

        protected string SetJsonString(object ob)
        {
            return JsonUtils.ToJson(ob);
        }

        protected K DeJsonString<K>(string str) where K : class 
        {
            K t;
            try
            {
                t = JsonUtils.FromJson<K>(str);
            }
            catch (Exception)
            {
                throw;
            }
            return t;
        }
    }
}