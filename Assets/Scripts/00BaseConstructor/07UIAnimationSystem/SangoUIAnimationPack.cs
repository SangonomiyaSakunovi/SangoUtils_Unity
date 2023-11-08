using System;
using UnityEngine;
public enum SangoUIAnimationType
{
    Timer,
}

public abstract class SangoUIBaseAnimation : MonoBehaviour
{
    public SangoUIAnimationType animationType;
    public float durationTime;

    public abstract void InitAnimation(params string[] commands);
    public abstract void PlayAnimation(params string[] commands);
    public abstract void StopAnimation();
    public abstract void ResetAnimation();
}

public class SangoUIAnimationPack
{
    public string id;
    public bool isPlaying;
    public string[] commands;
    public SangoUIBaseAnimation sangoUIAnimation;
    public Action completeAnimatorCallBack;
    public Action cancelAnimatorCallBack;

    public SangoUIAnimationPack(string id, SangoUIBaseAnimation sangoUIAnimation, Action completeAnimatorCallBack, Action cancelAnimatorCallBack)
    {
        this.id = id;
        this.isPlaying = false;
        this.sangoUIAnimation = sangoUIAnimation;
        this.completeAnimatorCallBack = completeAnimatorCallBack;
        this.cancelAnimatorCallBack = cancelAnimatorCallBack;
    }
}
