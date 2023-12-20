using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class LoggerUtils_Sango : MonoBehaviour
{
    public static LoggerConfig_Sango config;
    private static ILogger_Sango logger;
    private static StreamWriter logFileWriter = null;

    public static void InitSettings(LoggerConfig_Sango cfg = null)
    {
        if (cfg == null)
        {
            cfg = new LoggerConfig_Sango();
        }
        config = cfg;
        switch (config.loggerType)
        {
            case LoggerType.OnEditorConsole:
                logger = new UnityLogger();
                break;
        }
        if (config.enableSaveLog == false) { return; }
        if (config.enableCoverLog)
        {
            string path = config.saveLogPath + config.saveLogName;
            try
            {
                if (Directory.Exists(config.saveLogPath))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                else
                {
                    Directory.CreateDirectory(config.saveLogPath);
                }
                logFileWriter = File.AppendText(path);
                logFileWriter.AutoFlush = true;
            }
            catch
            {
                logFileWriter = null;
            }
        }
        else
        {
            string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-ss");
            string path = config.saveLogPath + prefix + config.saveLogName;
            try
            {
                if (Directory.Exists(config.saveLogPath) == false)
                {
                    Directory.CreateDirectory(config.saveLogPath);
                }
                logFileWriter = File.AppendText(path);
                logFileWriter.AutoFlush = true;
            }
            catch
            {

            }
        }
    }

    #region Log
    public static void LogInfo(string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args));
        logger.Log(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogInfo]{0}", log));
        }
    }

    public static void LogInfo(object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString());
        logger.Log(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogInfo]{0}", log));
        }
    }

    public static void ColorLog(LoggerColor color, string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args));
        logger.Log(log, color);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogInfo]{0}", log));
        }
    }

    public static void ColorLog(LoggerColor color, object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString());
        logger.Log(log, color);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogInfo]{0}", log));
        }
    }

    public static void LogTraceInfo(string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args), true);
        logger.Log(log, LoggerColor.Magenta);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogTraceInfo]{0}", log));
        }
    }

    public static void LogTraceInfo(object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString(), true);
        logger.Log(log, LoggerColor.Magenta);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogTraceInfo]{0}", log));
        }
    }

    public static void LogWarn(string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args));
        logger.Warn(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogWarn]{0}", log));
        }
    }

    public static void LogWarn(object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString());
        logger.Warn(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogWarn]{0}", log));
        }
    }

    public static void LogError(string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args));
        logger.Error(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogError]{0}", log));
        }
    }

    public static void LogError(object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString());
        logger.Error(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogError]{0}", log));
        }
    }

    public static void LogProcessing(string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args));
        logger.Processing(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogProcessing]{0}", log));
        }
    }

    public static void LogProcessing(object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString());
        logger.Processing(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogProcessing]{0}", log));
        }
    }

    public static void LogDone(string log, params object[] args)
    {
        if (config.enableSangoLog == false) { return; }
        log = DecorateLog(string.Format(log, args));
        logger.Done(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogDone]{0}", log));
        }
    }

    public static void LogDone(object logObj)
    {
        if (config.enableSangoLog == false) { return; }
        string log = DecorateLog(logObj.ToString());
        logger.Done(log);
        if (config.enableSaveLog)
        {
            WriteToFile(string.Format("[LogDone]{0}", log));
        }
    }
    #endregion

    #region Decorate
    private static string DecorateLog(string log, bool isTraceInfo = false)
    {
        StringBuilder sb = new StringBuilder(config.logPrefix, 100);
        if (config.enableTimestamp)
        {
            sb.AppendFormat(" {0}", DateTime.Now.ToString("hh:mm:ss--fff"));
        }
        if (config.enableThreadID)
        {
            sb.AppendFormat(" {0}", GetThreadID());
        }
        sb.AppendFormat(" {0} {1}", config.logSeparate, log);
        if (isTraceInfo)
        {
            sb.AppendFormat(" \nStackTrace: {0}", GetTraceInfo());
        }
        return sb.ToString();
    }

    private static string GetThreadID()
    {
        return string.Format("ThreadID:{0}", Thread.CurrentThread.ManagedThreadId);
    }

    private static string GetTraceInfo()
    {
        StackTrace st = new StackTrace(3, true);    //The method called DecorateLog has 3 calls should be ignore
        string traceInfo = "";
        for (int i = 0; i < st.FrameCount; i++)
        {
            StackFrame sf = st.GetFrame(i);
            traceInfo += string.Format("\n    {0}::{1}  line:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber());
        }
        return traceInfo;
    }
    #endregion

    private static void WriteToFile(string log)
    {
        if (logFileWriter != null)
        {
            try
            {
                logFileWriter.WriteLine(log);
            }
            catch
            {
                logFileWriter = null;
            }
        }
    }

    private class UnityLogger : ILogger_Sango
    {
        public void Log(string log, LoggerColor color = LoggerColor.None)
        {
            log = GetUnityLogColorString(log, color);
            UnityEngine.Debug.Log(log);
        }

        public void Processing(string log)
        {
            log = GetUnityLogColorString(log, LoggerColor.Cyan);
            UnityEngine.Debug.Log(log);
        }

        public void Done(string log)
        {
            log = GetUnityLogColorString(log, LoggerColor.Green);
            UnityEngine.Debug.Log(log);
        }

        public void Warn(string log)
        {
            UnityEngine.Debug.LogWarning(log);
        }

        public void Error(string log)
        {
            UnityEngine.Debug.LogError(log);
        }

        private string GetUnityLogColorString(string log, LoggerColor color)
        {
            switch (color)
            {
                case LoggerColor.None:
                    break;
                case LoggerColor.Yellow:
                    log = string.Format("<color=#FFFF00>{0}</color>", log);
                    break;
                case LoggerColor.Red:
                    log = string.Format("<color=#FF0000>{0}</color>", log);
                    break;
                case LoggerColor.Green:
                    log = string.Format("<color=#00FF00>{0}</color>", log);
                    break;
                case LoggerColor.Blue:
                    log = string.Format("<color=#0000FF>{0}</color>", log);
                    break;
                case LoggerColor.Magenta:
                    log = string.Format("<color=#FF00FF>{0}</color>", log);
                    break;
                case LoggerColor.Cyan:
                    log = string.Format("<color=#00FFFF>{0}</color>", log);
                    break;
            }
            return log;
        }
    }
}