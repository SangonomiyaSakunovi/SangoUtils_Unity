using System;
using System.Collections.Generic;

namespace SangoUtils_Unity_Scripts.Net
{
    public class HttpId
    {
        [HttpApiKey("Register")]
        public const int registerId = 10001;
        [HttpApiKey("Login")]
        public const int loginId = 10002;
        [HttpApiKey("testPath/path1/path2")]
        public const int testId = 10003;

        private static Dictionary<int, string> idDict = new();

        static HttpId()
        {
            System.Reflection.FieldInfo[] fields = typeof(HttpId).GetFields();

            Type attributeType = typeof(HttpApiKey);
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].IsDefined(attributeType, false))
                {
                    int id = (int)fields[i].GetValue(null);
                    object attribute = fields[i].GetCustomAttributes(attributeType, false)[0];
                    string api = (attribute as HttpApiKey).httpApi;
                    idDict[id] = api;
                }
            }
        }

        public static string GetHttpApi(int httpId)
        {
            idDict.TryGetValue(httpId, out var api);
            return api;
        }
    }


    public class HttpApiKey : Attribute
    {
        public string httpApi;

        public HttpApiKey(string httpApi)
        {
            this.httpApi = httpApi;
        }
    }
}