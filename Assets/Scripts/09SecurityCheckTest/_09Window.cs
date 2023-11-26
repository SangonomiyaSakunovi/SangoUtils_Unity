using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class _09Window : BaseWindow
{
    private _09Root _09Root = null;

    public Transform _keyboardTrans;

    public TMP_InputField _timestamp;
    public TMP_InputField _signData;

    public Button _callKeyboardBtn;
    public Button _closeKeyboardBtn;
    public Button _registBtn;

    private TMP_InputField _currentTypeInField;

    public void SetRoot(_09Root root)
    {
        _09Root = root;
    }

    protected override void OnInit()
    {
        base.OnInit();
        SetButtonListener(_callKeyboardBtn, OnCallKeyboardBtnClicked);
        SetButtonListener(_closeKeyboardBtn, OnCloseKeyboardBtnClicked);
        SetButtonListener(_registBtn, OnRegistSoftwareBtnClicked);
        AddInputNameClickEvent(_timestamp.gameObject);
        AddInputNameClickEvent(_signData.gameObject);
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
        bool res = _09Root.UpdateRegistInfo(_timestamp.text, _signData.text);
    }

    private void OnTypedInWordCallBack(TypeInCommand typeInCommand, string words)
    {
        switch (typeInCommand)
        {
            case TypeInCommand.TypeIn:
                _currentTypeInField.text += words;
                break;
            case TypeInCommand.Delet:
                if (_currentTypeInField.text.Length == 0) return;
                _currentTypeInField.text = _currentTypeInField.text.Remove(_currentTypeInField.text.Length - 1, 1);
                break;
            case TypeInCommand.Space:
                _currentTypeInField.text += " ";
                break;
            case TypeInCommand.Clear:
                _currentTypeInField.text = "";
                break;
            case TypeInCommand.Confirm:

                break;
            case TypeInCommand.Cancel:

                break;
            case TypeInCommand.EnAlt:

                break;
        }
    }

    private void AddInputNameClickEvent(GameObject gameObject)
    {
        var eventTrigger = GetOrAddComponent<EventTrigger>(gameObject);
        UnityAction<BaseEventData> selectEvent = OnInputFieldClicked;
        EventTrigger.Entry onClick = new EventTrigger.Entry()
        {
            eventID = EventTriggerType.PointerClick
        };

        onClick.callback.AddListener(selectEvent);
        eventTrigger.triggers.Add(onClick);
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
