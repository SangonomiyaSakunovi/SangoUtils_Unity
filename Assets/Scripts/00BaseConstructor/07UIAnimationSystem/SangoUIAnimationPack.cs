using System;
using UnityEngine;
public enum SangoUIAnimationType
{
    Timer,
}

public abstract class SangoUIBaseAnimation : MonoBehaviour
{
    public float durationTime;
}

public abstract class SangoUIAnimationPack
{
    public SangoUIAnimationType animationType;
    public SangoUIBaseAnimation sangoUIAnimation;

    public Action completeAnimatorCallBack;
    public Action cancelAnimatorCallBack;
}
