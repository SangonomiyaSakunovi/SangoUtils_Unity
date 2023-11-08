using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ResSangoUISlideBroadCastImageAnimation : SangoUIBaseAnimation
{
    private float _defalutDuration;
    private int _midPos = 0;
    private Transform _parentTrans;
    private List<RectTransform> _imageRects;

    public ResSangoUISlideBroadCastImageAnimation(Transform parentTrans, float aniDurationTime)
    {
        _parentTrans = parentTrans;
        _defalutDuration = aniDurationTime;
    }

    public override void InitAnimation(params string[] commands)
    {
        _imageRects = new List<RectTransform>();

        for (int i = 0; i < _parentTrans.childCount; i++)
        {
            _imageRects.Add(_parentTrans.GetChild(i).GetComponent<RectTransform>());
        }
        durationTime = _defalutDuration;
        _midPos = _imageRects.Count / 2;
        ChangeSibling();
    }

    public override void PlayAnimation(params string[] commands)
    {

        if (commands == null) return;
        Debug.Log(commands.Length);
        if (commands[0] == "-1")
        {
            LeftSlideAnimation();
        }
        else if (commands[0] == "1")
        {
            RightSlideAnimation();
        }
    }

    public override void StopAnimation()
    {

    }

    public override void ResetAnimation()
    {
        durationTime = _defalutDuration;
    }

    private void LeftSlideAnimation()
    {
        for (int i = _imageRects.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                _imageRects[i].DOAnchorPos(_imageRects[i + _imageRects.Count - 1].anchoredPosition, durationTime);
                _imageRects[i].DOSizeDelta(_imageRects[i + _imageRects.Count - 1].sizeDelta, durationTime);
            }
            else
            {
                _imageRects[i].DOAnchorPos(_imageRects[i - 1].anchoredPosition, durationTime);
                _imageRects[i].DOSizeDelta(_imageRects[i - 1].sizeDelta, durationTime);
            }
        }
        if (_midPos < _imageRects.Count - 1)
        {
            _midPos++;
        }
        else
        {
            _midPos = 0;
        }
        ChangeSibling();
    }

    private void RightSlideAnimation()
    {
        for (int i = 0; i < _imageRects.Count; i++)
        {
            if (i == _imageRects.Count - 1)
            {
                _imageRects[i].DOAnchorPos(_imageRects[i - _imageRects.Count + 1].anchoredPosition, durationTime);
                _imageRects[i].DOSizeDelta(_imageRects[i - _imageRects.Count + 1].sizeDelta, durationTime);
            }
            else
            {
                _imageRects[i].DOAnchorPos(_imageRects[i + 1].anchoredPosition, durationTime);
                _imageRects[i].DOSizeDelta(_imageRects[i + 1].sizeDelta, durationTime);
            }
        }
        if (_midPos > 0)
        {
            _midPos--;
        }
        else
        {
            _midPos = _imageRects.Count - 1;
        }
        ChangeSibling();
    }

    private void ChangeSibling()
    {
        int lastSibling = _imageRects.Count / 2 + 1;
        _imageRects[_midPos].transform.SetAsLastSibling();
        for (int i = 0; i <= _imageRects.Count / 2; i++)
        {
            lastSibling--;
            if (_midPos - i >= 0)
            {
                _imageRects[_midPos - i].transform.SetSiblingIndex(lastSibling);
            }
            else
            {
                _imageRects[_midPos + _imageRects.Count - i].transform.SetSiblingIndex(lastSibling);
            }
            if (_midPos + i <= _imageRects.Count - 1)
            {
                _imageRects[_midPos + i].transform.SetSiblingIndex(lastSibling);
            }
            else
            {
                _imageRects[_midPos - _imageRects.Count + i].transform.SetSiblingIndex(lastSibling);
            }
        }
    }
}
