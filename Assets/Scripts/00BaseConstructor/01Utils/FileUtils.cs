using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;

public static class JsonUtils
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

public static class TextureUtils
{
    private static string _texture2DType = "*.PNG|*.JPG";

    public static List<Texture> LoadTextureFolder(string folderPath, int width, int length)
    {       
        if (!Directory.Exists(folderPath))
        {
            return null;
        }
        List<string> texturePaths = new List<string>();
        string[] textureTypes = _texture2DType.Split('|');
        for (int i = 0; i < textureTypes.Length; i++)
        {
            string[] textureDirs = Directory.GetFiles(folderPath, textureTypes[i]);
            for (int j = 0; j < textureDirs.Length; j++)
            {
                texturePaths.Add(textureDirs[j]);
            }
        }
        List<Texture> textureResults = new List<Texture>();
        for (int k = 0; k < texturePaths.Count; k++)
        {
            Texture2D texture = new Texture2D(width, length);
            byte[] textureByte = FileStreamUtils.GetBytes(texturePaths[k]);
            texture.LoadImage(textureByte);
            textureResults.Add(texture);
        }
        return textureResults;
    }
}

public static class FileStreamUtils
{
    public static byte[] GetBytes(string filePath)
    {
        FileStream fileStream = new FileStream(filePath, FileMode.Open);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, bytes.Length);
        fileStream.Close();
        return bytes;
    }
}