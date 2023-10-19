using System;

public abstract class FSMTransCommandBase { }

public class FSMTransCommandEnum<T> : FSMTransCommandBase where T : struct
{
    public T _enumId { get; private set; }

    public FSMTransCommandEnum(T enumId)
    {
        _enumId = enumId;
    }

    public override bool Equals(object obj)
    {
        if (obj is FSMTransCommandEnum<T>)
        {
            FSMTransCommandEnum<T> commandEnum = obj as FSMTransCommandEnum<T>;
            return commandEnum._enumId.Equals(_enumId);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return _enumId.GetHashCode();
    }
}

public class FSMTransCommandData : FSMTransCommandBase
{
    public byte[] _data { get; private set; }

    public FSMTransCommandData(byte[] data)
    {
        _data = data;
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