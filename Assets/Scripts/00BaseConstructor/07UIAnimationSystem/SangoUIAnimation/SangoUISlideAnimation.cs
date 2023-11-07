using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class SangoUISlideBroadCastImageAnimation : SangoUIBaseAnimation
{
    private int _midPos = 0;
    private List<RectTransform> _imageRects;

    public void InvokeAnimation(Transform parentTrans, bool isLeftSlide, float aniDurationTime)
    {
        _imageRects = new List<RectTransform>();
        for (int i = 0; i < parentTrans.childCount; i++)
        {
            _imageRects.Add(parentTrans.GetChild(i).GetComponent<RectTransform>());
        }
        durationTime = aniDurationTime;
        _midPos = _imageRects.Count / 2;
        if (isLeftSlide)
        {
            LeftSlideAnimation();
        }
        else
        {
            RightSlideAnimation();
        }
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
        for (int i = 0; i <= _imageRects.Count; i++)
        {
            if (i == _imageRects.Count)
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
