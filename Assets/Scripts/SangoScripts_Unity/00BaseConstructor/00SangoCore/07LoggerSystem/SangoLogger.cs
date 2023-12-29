public static class SangoLogger
{
    public static void InitLogger(LoggerConfig_Sango cfg)
    {
        LoggerUtils_Sango.InitSettings(cfg);
    }

    public static void Log(string log, params object[] args)
    {
        LoggerUtils_Sango.LogInfo(log, args);
    }

    public static void Warning(string log, params object[] args)
    {
        LoggerUtils_Sango.LogWarn(log, args);
    }

    public static void Error(string log, params object[] args)
    {
        LoggerUtils_Sango.LogError(log, args);
    }

    public static void TraceInfo(string log, params object[] args)
    {
        LoggerUtils_Sango.LogTraceInfo(log, args);
    }

    public static void Processing(string log, params object[] args)
    {
        LoggerUtils_Sango.LogProcessing(log, args);
    }

    public static void Done(string log, params object[] args)
    {
        LoggerUtils_Sango.LogDone(log, args);
    }

    public static void ColorLog(LoggerColor color, string log, params object[] args)
    {
        LoggerUtils_Sango.ColorLog(color, log, args);
    }
}
