using UnityEngine;

public static class SangoUIAnimationAssets
{
    public static SangoUISlideBroadCastImageAnimation SangoUISlideBroadCastImageAnimation(Transform parentTrans, float aniDurationTime)
    {
        SangoUISlideBroadCastImageAnimation animation = new SangoUISlideBroadCastImageAnimation(parentTrans, aniDurationTime);
        animation.InitAnimation();
        return animation;
    }
}
