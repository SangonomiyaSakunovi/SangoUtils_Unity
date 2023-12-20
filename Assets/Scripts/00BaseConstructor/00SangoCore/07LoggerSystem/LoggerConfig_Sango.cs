using System;

public class LoggerConfig_Sango
{
    public bool enableSangoLog = true;
    public string logPrefix = "#";
    public bool enableTimestamp = true;
    public string logSeparate = ">>";
    public bool enableThreadID = true;
    public bool enableTraceInfo = true;
    public bool enableSaveLog = true;
    public bool enableCoverLog = true;
    public string saveLogPath = string.Format("{0}Logs\\", AppDomain.CurrentDomain.BaseDirectory);
    public string saveLogName = "SangoLog.txt";
    public LoggerType loggerType = LoggerType.OnEditorConsole;
}

public enum LoggerType
{
    OnEditorConsole,
    OnScreen
}

public enum LoggerColor
{
    None,
    Red,
    Green,
    Blue,
    Cyan,
    Magenta,
    Yellow
}

public interface ILogger_Sango
{
    void Log(string message, LoggerColor color = LoggerColor.None);
    void Processing(string message);
    void Done(string message);
    void Warn(string message);
    void Error(string message);
}