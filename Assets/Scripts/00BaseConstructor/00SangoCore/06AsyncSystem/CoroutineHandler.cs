using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class CoroutineExtensions
{
    public static CoroutineHandler Start(this IEnumerator enumerator)
    {
        CoroutineHandler handler = new CoroutineHandler(enumerator);
        handler.Start();
        return handler;
    }
}

public class CoroutineHandler
{
    public IEnumerator Coroutine { get; private set; } = null;

    public bool Paused { get; private set; } = false;

    public bool Running { get; private set; } = false;

    public bool Stopped { get; private set; } = false;

    public class FinishedHandler : UnityEvent<bool> { }

    public FinishedHandler OnCompleted = new FinishedHandler();

    public CoroutineHandler(IEnumerator coroutine)
    {
        Coroutine = coroutine;
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Resume()
    {
        Paused = false;
    }

    public void Start()
    {
        if (null != Coroutine)
        {
            Running = true;
            CoroutineDriver.Run(CallWrapper());
        }
        else
        {
            SangoLogger.Log("Coroutine is Null.");
        }
    }

    public void Stop()
    {
        Stopped = true;
        Running = false;
    }

    private void Complete()
    {
        OnCompleted?.Invoke(Stopped);
        OnCompleted.RemoveAllListeners();
        Coroutine = null;
    }

    public CoroutineHandler OnComplete(UnityAction<bool> action)
    {
        OnCompleted.AddListener(action);
        return this;
    }

    private IEnumerator CallWrapper()
    {
        yield return null;
        IEnumerator enumerator = Coroutine;
        while (Running)
        {
            if (Paused)
            {
                yield return null;
            }
            else
            {
                if (enumerator != null && enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                else
                {
                    Running = false;
                }
            }
        }
        Complete();
    }

    internal class CoroutineDriver : MonoBehaviour
    {
        internal static CoroutineDriver _driver;

        internal static CoroutineDriver Driver
        {
            get
            {
                if (null == _driver)
                {
                    _driver = FindObjectOfType(typeof(CoroutineDriver)) as CoroutineDriver;
                    if (null == _driver)
                    {
                        GameObject gameObject = new GameObject("[CoroutineDriver]");
                        _driver = gameObject.AddComponent<CoroutineDriver>();
                        DontDestroyOnLoad(gameObject);
                        gameObject.hideFlags = HideFlags.HideAndDontSave;
                    }
                }
                return _driver;
            }
        }

        private void Awake()
        {
            if (null != _driver && _driver != this)
            {
                Destroy(gameObject);
            }
        }

        public static Coroutine Run(IEnumerator target)
        {
            return Driver.StartCoroutine(target);
        }
    }
}
