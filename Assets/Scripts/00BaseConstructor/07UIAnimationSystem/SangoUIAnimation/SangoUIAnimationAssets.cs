using UnityEngine;

public static class SangoUIAnimationAssets
{
    public static ResSangoUISlideBroadCastImageAnimation SangoUISlideBroadCastImageAnimation(Transform parentTrans, float aniDurationTime)
    {
        ResSangoUISlideBroadCastImageAnimation animation = new ResSangoUISlideBroadCastImageAnimation(parentTrans, aniDurationTime);
        animation.InitAnimation();
        return animation;
    }
}
