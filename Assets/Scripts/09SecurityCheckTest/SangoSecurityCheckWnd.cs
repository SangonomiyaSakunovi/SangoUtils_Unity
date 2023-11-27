using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SangoSecurityCheckWnd : BaseWindow
{
    private SangoSecurityCheckRoot _09Root = null;

    public Transform _keyboardTrans;

    public TMP_InputField _timestamp;
    public TMP_InputField _signData;

    public Button _callKeyboardBtn;
    public Button _closeKeyboardBtn;
    public Button _registBtn;

    public TMP_Text _resultShow;

    private TMP_InputField _currentTypeInField;

    public void SetRoot(SangoSecurityCheckRoot root)
    {
        _09Root = root;
    }

    public void UpdateResult(string result)
    {
        SetText(_resultShow, result);
    }

    protected override void OnInit()
    {
        base.OnInit();
        SetButtonListener(_callKeyboardBtn, OnCallKeyboardBtnClicked);
        SetButtonListener(_closeKeyboardBtn, OnCloseKeyboardBtnClicked);
        SetButtonListener(_registBtn, OnRegistSoftwareBtnClicked);
        AddGameObjectClickEvent(_timestamp, OnInputFieldClicked);
        AddGameObjectClickEvent(_signData, OnInputFieldClicked);
    }

    private void OnCallKeyboardBtnClicked(Button button)
    {
        TypeInService.Instance.SetKeyboardDefaultTransform(_keyboardTrans);
        TypeInService.Instance.ShowKeyboard(OnTypedInWordCallBack);
    }
    private void OnCloseKeyboardBtnClicked(Button button)
    {
        TypeInService.Instance.HideKeyboard();
    }
    private void OnRegistSoftwareBtnClicked(Button button)
    {
        _09Root.UpdateRegistInfo(_timestamp.text, _signData.text);
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
            case TypeInCommand.Space:
                if (_currentTypeInField == null) return;
                _currentTypeInField.text += " ";
                break;
            case TypeInCommand.Clear:
                if (_currentTypeInField == null) return;
                _currentTypeInField.text = "";
                break;
            case TypeInCommand.Confirm:
                OnCloseKeyboardBtnClicked(null);
                break;
            case TypeInCommand.Cancel:
                OnCloseKeyboardBtnClicked(null);
                break;
            case TypeInCommand.EnAlt:

                break;
        }
    }

    private void OnInputFieldClicked(BaseEventData data)
    {
        if (data.selectedObject.name == _timestamp.name)
        {
            _currentTypeInField = _timestamp;
        }
        else
        {
            _currentTypeInField = _signData;
        }
    }
}
