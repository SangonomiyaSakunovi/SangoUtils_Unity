using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SangoPatchWnd : BaseWindow
{
    private SangoPatchRoot _sangoHotFixRoot;
    private Transform _messageBoxTrans;
    private TMP_Text _tips;

    private Action _clickMessageBoxOkCB;
    private Button _messageBoxOkBtn;
    private TMP_Text _messageBoxContent;

    protected override void OnInit()
    {
        base.OnInit();
        _messageBoxTrans = transform.Find("MessageBox");
        _tips = transform.Find("tips").GetComponent<TMP_Text>();

        _messageBoxOkBtn = _messageBoxTrans.Find("messageBoxOkBtn").GetComponent<Button>();
        _messageBoxContent = _messageBoxTrans.Find("messageBoxContent").GetComponent<TMP_Text>();

        SetActive(_messageBoxTrans, false);
        SetText(_tips, "欢迎使用热更新系统");
    }

    public override void SetRoot<T>(BaseRoot<T> baseRoot)
    {
        _sangoHotFixRoot = baseRoot as SangoPatchRoot;
    }

    public void ShowMessageBox(string content, Action onMessageBoxOKBtnClickedCB)
    {
        RemoveAllListeners(_messageBoxOkBtn);
        SetText(_messageBoxContent, content);
        _clickMessageBoxOkCB = onMessageBoxOKBtnClickedCB;
        SetButtonListener(_messageBoxOkBtn, OnMessageBoxOKBtnClicked);
        SetActive(_messageBoxTrans);
        _messageBoxTrans.SetAsLastSibling();
    }

    public void UpdateTips(string content)
    {
        SetText(_tips, content);
    }

    public void UpdateSliderValue(float value)
    {

    }

    private void OnMessageBoxOKBtnClicked(Button button)
    {
        _clickMessageBoxOkCB?.Invoke();
        SetActive(_messageBoxTrans, false);
    }
}
