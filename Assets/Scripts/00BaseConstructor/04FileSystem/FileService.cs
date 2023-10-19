using System;
using System.IO;
using UnityEngine;

public class FileService : BaseService<FileService>
{
    private string _screenshotName;
    private string _screenshotPath;
    private string _desktopPath;

    private void Start()
    {
        _desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F12))
    //    {
    //        CaptureScreenshot();
    //    }
    //}

    public void CaptureScreenshot()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        _screenshotName = "Screenshot_" + timestamp + ".png";
        _screenshotPath = Path.Combine(_desktopPath, _screenshotName);
        ScreenCapture.CaptureScreenshot(_screenshotPath);
        Debug.Log("截图已保存到桌面：" + _screenshotPath);
    }
}
