using System.IO;
using System.Text.Json;

public class JsonUtils
{
    public static T LoadJsonFile<T>(string filePath)
    {
        string readData;
        using (StreamReader sr = File.OpenText(filePath))
        {
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return JsonSerializer.Deserialize<T>(readData);
    }

    public static string SetJsonString(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T DeJsonString<T>(string str)
    {
        return JsonSerializer.Deserialize<T>(str);
    }
}
