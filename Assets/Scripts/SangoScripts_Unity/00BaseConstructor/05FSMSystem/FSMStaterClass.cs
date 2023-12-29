using System;

public abstract class FSMTransCommandBase { }

public class FSMTransCommandEnum<T> : FSMTransCommandBase where T : struct
{
    public T EnumId { get; private set; }

    public FSMTransCommandEnum(T enumId)
    {
        EnumId = enumId;
    }

    public override bool Equals(object obj)
    {
        if (obj is FSMTransCommandEnum<T>)
        {
            FSMTransCommandEnum<T> commandEnum = obj as FSMTransCommandEnum<T>;
            return commandEnum.EnumId.Equals(EnumId);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return EnumId.GetHashCode();
    }
}

public class FSMTransCommandData : FSMTransCommandBase
{
    public byte[] Data { get; private set; }

    public FSMTransCommandData(byte[] data)
    {
        Data = data;
    }

    public override bool Equals(object obj)
    {
        if (obj is FSMTransCommandData)
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return 0;
    }
}

public class FSMStaterItem<T> where T : struct
{
    public FSMTransCommandBase TransCommand { get; private set; }
    public T TargetState { get; private set; }
    public Func<T, FSMTransCommandBase, T, bool> TransCallBack { get; private set; }

    public FSMStaterItem(FSMTransCommandBase command, T targetState, Func<T, FSMTransCommandBase, T, bool> callBack)
    {
        TransCommand = command;
        TargetState = targetState;
        TransCallBack = callBack;
    }
}

public class FSMLinkedStaterItemBase
{
    protected FSMLinkedStater _fsmLinkedStater;

    public virtual void OnInit(FSMLinkedStater fsmLinkedStater)
    {
        _fsmLinkedStater = fsmLinkedStater;
    }
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}