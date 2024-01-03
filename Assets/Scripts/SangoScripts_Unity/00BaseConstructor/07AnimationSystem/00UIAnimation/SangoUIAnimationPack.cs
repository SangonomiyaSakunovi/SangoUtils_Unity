using System;
using UnityEngine;
public enum SangoUIAnimationType
{
    Timer,
}

public abstract class SangoUIBaseAnimation : MonoBehaviour
{
    public SangoUIAnimationType AnimationType { get; set; }
    public float DurationTime { get; set; }

    public abstract void InitAnimation(params string[] commands);
    public abstract void PlayAnimation(params string[] commands);
    public abstract void StopAnimation();
    public abstract void ResetAnimation();
}

public class SangoUIAnimationPack
{
    public string Id { get; private set; }
    public bool IsPlaying { get; set; }
    public string[] Commands { get; set; }
    public SangoUIBaseAnimation SangoUIAnimation { get; set; }
    public Action OnAnimationPlayedCompleted { get; set; }
    public Action OnAnimationPlayCanceled { get; set; }

    public SangoUIAnimationPack(string id, SangoUIBaseAnimation sangoUIAnimation, Action completeAnimatorCallBack, Action cancelAnimatorCallBack)
    {
        Id = id;
        IsPlaying = false;
        SangoUIAnimation = sangoUIAnimation;
        OnAnimationPlayedCompleted = completeAnimatorCallBack;
        OnAnimationPlayCanceled = cancelAnimatorCallBack;
    }
}
