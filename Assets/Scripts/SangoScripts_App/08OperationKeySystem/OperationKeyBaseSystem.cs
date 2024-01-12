using SangoUtils_Common.Messages;
using System;

namespace SangoScripts_App.Operation
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
}