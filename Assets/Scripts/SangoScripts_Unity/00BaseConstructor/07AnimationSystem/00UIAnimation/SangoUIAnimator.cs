using System.Collections.Generic;
using UnityEngine;

public class SangoUIAnimator : MonoBehaviour
{
    private Dictionary<string, SangoUIAnimationPack> _sangoUIAnimationDict = new Dictionary<string, SangoUIAnimationPack>();
    private List<SangoUIAnimationPack> _sangoUIAnimationToDos = new List<SangoUIAnimationPack>();
    private List<SangoUIAnimationPack> _sangoUIAnimationPlayings = new List<SangoUIAnimationPack>();

    public void Init() { }
    public void Clear() { }

    public void UpdateAnimator()
    {
        HandleAnimationPlayings();
        HandleAnimationTodos();
    }

    public void AddAnimation(SangoUIAnimationPack sangoUIAnimationPack)
    {
        _sangoUIAnimationDict.Add(sangoUIAnimationPack.Id, sangoUIAnimationPack);
    }

    public void PlayAnimation(string id, params string[] commands)
    {
        _sangoUIAnimationDict.TryGetValue(id, out SangoUIAnimationPack value);
        if (value != null)
        {
            value.Commands = commands;
            _sangoUIAnimationToDos.Add(value);
        }
    }

    public void PlayAnimationImmediately(string id, params string[] commands)
    {
        _sangoUIAnimationDict.TryGetValue(id, out SangoUIAnimationPack value);
        if (value != null && !value.IsPlaying)
        {
            value.IsPlaying = true;
            value.SangoUIAnimation.PlayAnimation(commands);
            _sangoUIAnimationPlayings.Add(value);
        }
    }

    private void HandleAnimationPlayings()
    {
        if (_sangoUIAnimationPlayings.Count == 0) return;

        for (int i = 0; i < _sangoUIAnimationPlayings.Count; i++)
        {
            SangoUIAnimationPack pack = _sangoUIAnimationPlayings[i];
            switch (pack.SangoUIAnimation.AnimationType)
            {
                case SangoUIAnimationType.Timer:
                    if (pack.SangoUIAnimation.DurationTime > -0.1)
                    {
                        pack.SangoUIAnimation.DurationTime -= Time.deltaTime;
                    }
                    else
                    {
                        pack.OnAnimationPlayedCompleted?.Invoke();
                        _sangoUIAnimationPlayings.Remove(pack);
                        pack.SangoUIAnimation.ResetAnimation();
                        pack.IsPlaying = false;
                    }
                    break;
            }
        }
    }

    private void HandleAnimationTodos()
    {
        if (_sangoUIAnimationToDos.Count == 0) return;

        for (int i = 0; i < _sangoUIAnimationToDos.Count; i++)
        {
            SangoUIAnimationPack pack = _sangoUIAnimationToDos[i];
            if (!pack.IsPlaying)
            {
                pack.IsPlaying = true;
                pack.SangoUIAnimation.PlayAnimation(pack.Commands);
                _sangoUIAnimationPlayings.Add(pack);
                _sangoUIAnimationToDos.Remove(pack);
            }
        }
    }
}
