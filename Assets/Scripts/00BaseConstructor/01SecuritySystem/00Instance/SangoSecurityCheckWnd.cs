using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SangoSecurityCheckWnd : BaseWindow
{
    private SangoSecurityCheckRoot _sangoSecurityCheckRoot = null;

    public Transform _keyboardTrans;
    public TMP_InputField _signData;
    public Button _registBtn;
    public TMP_Text _resultShow;

    private TMP_InputField _currentTypeInField;

    public void SetRoot(SangoSecurityCheckRoot root)
    {
        _sangoSecurityCheckRoot = root;
    }

    public void UpdateResult(string result)
    {
        SetText(_resultShow, result);
    }

    protected override void OnInit()
    {
        base.OnInit();
        SetButtonListener(_registBtn, OnRegistSoftwareBtnClicked);
        _currentTypeInField = _signData;
        ShowKeyboard(KeyboardTypeCode.UpperCharKeyboard);
        UpdateRegistBtnText("¼¤»î");
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        HideKeyboard();
        RemoveAllListeners(_registBtn);
    }

    public void UpdateRegistBtnText(string text)
    {
        if (text == "È·¶¨")
        {
            RemoveAllListeners(_registBtn);
            SetButtonListener(_registBtn, OnRegistOKBtnClicked);
        }
        SetText(_registBtn, text);
    }

    private void ShowKeyboard(KeyboardTypeCode type)
    {
        TypeInService.Instance.SetKeyboardDefaultTransform(_keyboardTrans);
        TypeInService.Instance.ShowKeyboard(type, OnTypedInWordCallBack);
    }
    private void HideKeyboard()
    {
        TypeInService.Instance.HideKeyboard();
    }
    private void OnRegistSoftwareBtnClicked(Button button)
    {
        _sangoSecurityCheckRoot.UpdateRegistInfo(_signData.text);
    }

    private void OnRegistOKBtnClicked(Button button)
    {
        _sangoSecurityCheckRoot.OnSecurityCheckResultValid();
    }

    private void OnTypedInWordCallBack(TypeInCommand typeInCommand, string words)
    {
        switch (typeInCommand)
        {
            case TypeInCommand.TypeIn:
                if (_currentTypeInField == null) return;
                _currentTypeInField.text += words;
                break;
            case TypeInCommand.Delet:
                if (_currentTypeInField == null) return;
                if (_currentTypeInField.text.Length == 0) return;
                _currentTypeInField.text = _currentTypeInField.text.Remove(_currentTypeInField.text.Length - 1, 1);
                break;
        }
    }
}
