public abstract class BaseASyncPackManager
{
    protected uint _packId = 1;

    public abstract bool RemovePack(uint packId);

    public abstract bool RemovePackCallBack(uint packId);

    protected abstract uint GeneratePackId();
}
