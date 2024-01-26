using SangoUtils_Bases_UnityEngine;
using System;

public class UIAnimationService : BaseService<UIAnimationService>
{
    private SangoUIAnimator _sangoUIAnimator = new();

    public override void OnInit()
    {
        base.OnInit();
        _sangoUIAnimator.Init();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _sangoUIAnimator.UpdateAnimator();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        _sangoUIAnimator.Clear();
    }

    public void AddAnimation(string id, SangoUIBaseAnimation sangoUIAnimation, Action completeCallBack = null, Action cancelCallBack = null)
    {
        SangoUIAnimationPack pack = new SangoUIAnimationPack(id, sangoUIAnimation, completeCallBack, cancelCallBack);
        _sangoUIAnimator.AddAnimation(pack);
    }

    public void PlayAnimation(string id, params string[] commands)
    {        
        _sangoUIAnimator.PlayAnimationImmediately(id, commands);
    }

    public void PlayAnimationAsync(string id, params string[] commands)
    {
        _sangoUIAnimator.PlayAnimation(id, commands);
    }
}
