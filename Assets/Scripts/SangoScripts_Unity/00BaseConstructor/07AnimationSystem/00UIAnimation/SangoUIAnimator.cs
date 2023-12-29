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
        _sangoUIAnimationDict.Add(sangoUIAnimationPack.id, sangoUIAnimationPack);
    }

    public void PlayAnimation(string id, params string[] commands)
    {
        _sangoUIAnimationDict.TryGetValue(id, out SangoUIAnimationPack value);
        if (value != null)
        {
            value.commands = commands;
            _sangoUIAnimationToDos.Add(value);
        }
    }

    public void PlayAnimationImmediately(string id, params string[] commands)
    {
        _sangoUIAnimationDict.TryGetValue(id, out SangoUIAnimationPack value);
        if (value != null && !value.isPlaying)
        {
            value.isPlaying = true;
            value.sangoUIAnimation.PlayAnimation(commands);
            _sangoUIAnimationPlayings.Add(value);
        }
    }

    private void HandleAnimationPlayings()
    {
        if (_sangoUIAnimationPlayings.Count == 0) return;

        for (int i = 0; i < _sangoUIAnimationPlayings.Count; i++)
        {
            SangoUIAnimationPack pack = _sangoUIAnimationPlayings[i];
            switch (pack.sangoUIAnimation.animationType)
            {
                case SangoUIAnimationType.Timer:
                    if (pack.sangoUIAnimation.durationTime > -0.1)
                    {
                        pack.sangoUIAnimation.durationTime -= Time.deltaTime;
                    }
                    else
                    {
                        pack.completeAnimatorCallBack?.Invoke();
                        _sangoUIAnimationPlayings.Remove(pack);
                        pack.sangoUIAnimation.ResetAnimation();
                        pack.isPlaying = false;
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
            if (!pack.isPlaying)
            {
                pack.isPlaying = true;
                pack.sangoUIAnimation.PlayAnimation(pack.commands);
                _sangoUIAnimationPlayings.Add(pack);
                _sangoUIAnimationToDos.Remove(pack);
            }
        }
    }
}
