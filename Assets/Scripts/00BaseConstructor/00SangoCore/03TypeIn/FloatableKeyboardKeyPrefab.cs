using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatableKeyboardKeyPrefab : BasePrefab
{
    [Header("StandardKey")]
    public Button _keyButton;
    public TMP_Text _keyTextMiddle;
    public TMP_Text _keyTextLeftUp;

    [Header("SpecialKey")]
    public Button _specialKeyButton;

    private string _typeInWord;
    private Action<string> _specialButtonClickedCallBack;

    public void UpdateStandardButtonListener(string middleText, string leftUpText, bool isDispose)
    {
        if (!isDispose)
        {
            RemoveAllListeners(_keyButton);
            _typeInWord = middleText;
            UpdateButtonName(middleText, leftUpText);
            SetButtonListener(_keyButton, OnStandardButtonClicked);
        }
        else
        {
            RemoveAllListeners(_keyButton);
        }
    }

    public void UpdateSpecialButtonListener(Action<string> specialButtonClickedCallBack, bool isDispose)
    {
        if (!isDispose)
        {
            _specialButtonClickedCallBack = specialButtonClickedCallBack;
            SetButtonListener(_specialKeyButton, OnSpecialButtonClicked);
        }
        else
        {
            RemoveAllListeners(_specialKeyButton);
            _specialButtonClickedCallBack = null;
        }
    }

    private void UpdateButtonName(string middleText, string leftUpText)
    {
        SetText(_keyTextMiddle, middleText);
        SetText(_keyTextLeftUp, leftUpText);
    }

    private void OnStandardButtonClicked(Button button)
    {
        TypeInService.Instance.OnTypedInWord(TypeInCommand.TypeIn, _typeInWord);
    }

    public void OnSpecialButtonClicked(Button button)
    {
        _specialButtonClickedCallBack?.Invoke(button.name);
    }
}
