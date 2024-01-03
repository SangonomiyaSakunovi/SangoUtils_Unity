using System;
using UnityEngine;

public class IOCPLogger : MonoBehaviour
{
    public static Action<string> LogInfoCallBack { get; set; }
    public static Action<string> LogErrorCallBack { get; set; }
    public static Action<string> LogWarningCallBack { get; set; }

    public static void Info(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogInfoCallBack != null)
        {
            LogInfoCallBack(message);
        }
        else
        {
            SangoLogger.Log(message);
        }
    }

    public static void Start(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogInfoCallBack != null)
        {
            LogInfoCallBack(message);
        }
        else
        {
            SangoLogger.Done(message);
        }
    }

    public static void Special(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogInfoCallBack != null)
        {
            LogInfoCallBack(message);
        }
        else
        {
            SangoLogger.Processing(message);
        }
    }

    public static void Done(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogInfoCallBack != null)
        {
            LogInfoCallBack(message);
        }
        else
        {
            SangoLogger.Done(message);
        }
    }

    public static void Processing(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogInfoCallBack != null)
        {
            LogInfoCallBack(message);
        }
        else
        {
            SangoLogger.Processing(message);
        }
    }

    public static void Error(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogErrorCallBack != null)
        {
            LogErrorCallBack(message);
        }
        else
        {
            SangoLogger.Error(message);
        }
    }

    public static void Warning(string message, params object[] arguments)
    {
        message = string.Format(message, arguments);
        if (LogWarningCallBack != null)
        {
            LogWarningCallBack(message);
        }
        else
        {
            SangoLogger.Warning(message);
        }
    }   
}

public enum IOCPLogColor
{
    None,
    Red,
    Green,
    Blue,
    Cyan,
    Magenta,
    Yellow
}
