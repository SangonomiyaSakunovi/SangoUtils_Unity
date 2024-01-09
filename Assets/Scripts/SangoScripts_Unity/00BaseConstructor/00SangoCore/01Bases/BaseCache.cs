public abstract class BaseCache
{
    public string CacheId { get; protected set; }

    public string Id { get; protected set; }

    public CacheLevelCode CacheLevelCode { get; protected set; }
}

public enum CacheLevelCode
{
    Root,
}
