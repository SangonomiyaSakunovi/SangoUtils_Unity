using System;
using UnityEditor;
using UnityEngine;

public class FileService : BaseService<FileService>
{
    private string _screenshotName;

    public void CaptureScreenshot()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        _screenshotName = "Screenshot_" + timestamp + ".png";
        string filePath = EditorUtility.SaveFilePanel("生成作品", "", _screenshotName, "png");
        if (!string.IsNullOrEmpty(filePath))
        {
            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log("截图已保存到桌面：" + filePath);
        }
    }
}
