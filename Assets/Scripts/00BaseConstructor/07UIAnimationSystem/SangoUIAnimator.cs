using System.Collections.Generic;
using UnityEngine;

public class SangoUIAnimator : MonoBehaviour
{
    private List<SangoUIAnimationPack> _sangoUIAnimationPacks = new List<SangoUIAnimationPack>();

    public void Init() { }
    public void Clear() { }

    public void UpdateAnimator()
    {
        HandleAnimation();
    }

    public void AddAnimation(SangoUIAnimationPack sangoUIAnimationPack)
    {
        _sangoUIAnimationPacks.Add(sangoUIAnimationPack);
    }

    public void HandleAnimation()
    {
        if (_sangoUIAnimationPacks.Count == 0) return;

        for (int i = 0; i < _sangoUIAnimationPacks.Count; i++)
        {
            SangoUIAnimationPack pack = _sangoUIAnimationPacks[i];
            switch (pack.animationType)
            {
                case SangoUIAnimationType.Timer:
                    if (pack.sangoUIAnimation.durationTime > 0)
                    {
                        pack.sangoUIAnimation.durationTime -= Time.deltaTime;
                    }
                    else
                    {
                        pack.completeAnimatorCallBack?.Invoke();
                        _sangoUIAnimationPacks.Remove(pack);
                    }
                    break;
            }
        }
    }

    public void AddAnimationImmedietly(SangoUIAnimationPack sangoUIAnimationPack)
    {
        _sangoUIAnimationPacks.Add(sangoUIAnimationPack);
        HandleAnimation();
    }
}
