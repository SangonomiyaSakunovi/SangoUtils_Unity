using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class _05Window01 : MonoBehaviour
{
    int _midPos = 0;
    public Transform _parentTrans;
    private List<RectTransform> tfs;

    void Start()
    {
        tfs = new List<RectTransform>();
        for (int i = 0; i < _parentTrans.childCount; i++)
        {
            tfs.Add(_parentTrans.GetChild(i).GetComponent<RectTransform>());
        }
        _midPos = tfs.Count / 2;
        //ChangeSibling();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("有输入");
            LeftBtnClickEvent();
            ChangeSibling();
        }
    }

    void LeftBtnClickEvent()
    {
        for (int i = tfs.Count - 1; i >= 0; i--)
        {
            if (i == 0)
            {
                tfs[i].DOAnchorPos(tfs[i + tfs.Count - 1].anchoredPosition, 0.2f);
                tfs[i].DOSizeDelta(tfs[i + tfs.Count - 1].sizeDelta, 0.2f);
            }
            else
            {
                tfs[i].DOAnchorPos(tfs[i - 1].anchoredPosition, 0.2f);
                tfs[i].DOSizeDelta(tfs[i - 1].sizeDelta, 0.2f);
            }
        }
        if (_midPos < tfs.Count - 1)
        {
            _midPos++;
        }
        else
        {
            _midPos = 0;
        }
    }

    void ChangeSibling()
    {
        int lastSibling = tfs.Count / 2 + 1;
        Debug.Log("当前的Sibling" + lastSibling);
        Debug.Log("当前的midPos" + _midPos);
        tfs[_midPos].transform.SetAsLastSibling();
        for (int i = 0; i <= tfs.Count / 2; i++)
        {
            lastSibling--;
            Debug.Log("当前的Sibling" + lastSibling);
            if (_midPos - i >= 0)
            {
                tfs[_midPos - i].transform.SetSiblingIndex(lastSibling);
            }
            else
            {
                tfs[_midPos + tfs.Count - i].transform.SetSiblingIndex(lastSibling);
            }
            if (_midPos + i <= tfs.Count - 1)
            {
                tfs[_midPos + i].transform.SetSiblingIndex(lastSibling);
            }
            else
            {
                tfs[_midPos - tfs.Count + i].transform.SetSiblingIndex(lastSibling);
            }
        }
    }
}
