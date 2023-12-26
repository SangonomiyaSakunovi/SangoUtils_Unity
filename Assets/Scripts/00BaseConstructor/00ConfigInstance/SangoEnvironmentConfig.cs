public class NetEnvironmentConfig : BaseConfig
{
    public NetEnvMode netEnvMode = NetEnvMode.Offline;
    public string serverAddress = "127.0.0.1";
    public int serverPort = 52037;
}

public enum NetEnvMode
{
    Offline,
    Online_IOCP,
    Online_Http
}
