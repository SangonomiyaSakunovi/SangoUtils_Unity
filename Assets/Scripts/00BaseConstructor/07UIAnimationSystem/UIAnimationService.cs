public class UIAnimationService : BaseService<UIAnimationService>
{
    private SangoUIAnimator _sangoUIAnimator;

    public override void OnInit()
    {
        base.OnInit();
        _sangoUIAnimator = new SangoUIAnimator();
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

    public void AddAnimation(SangoUIAnimationPack sangoUIAnimationPack)
    {
        _sangoUIAnimator.AddAnimation(sangoUIAnimationPack);
    }

    public void AddAnimationImmedietly(SangoUIAnimationPack sangoUIAnimationPack)
    {
        _sangoUIAnimator.AddAnimation(sangoUIAnimationPack);
    }
}
