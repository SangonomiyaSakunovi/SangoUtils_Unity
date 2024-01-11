public abstract class BaseSystem<T> where T : class
{
    public string SystemID { get; protected set; } = "";
    
    public void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    public virtual void OnDispose()
    {

    }
}
