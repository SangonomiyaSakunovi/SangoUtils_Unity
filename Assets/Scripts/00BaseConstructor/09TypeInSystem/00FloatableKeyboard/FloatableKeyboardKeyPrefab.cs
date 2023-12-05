using System;
using TMPro;
using UnityEngine.UI;

public class FloatableKeyboardKeyPrefab : BasePrefab
{
    private Button _keyButton;
    private TMP_Text _keyTextMiddle;
    private TMP_Text _keyTextLeftUp;

    private Button _specialKeyButton;

    private string _typeInWord;
    private Action<string> _specialButtonClickedCallBack;

    public void InitStandardKey(TMP_FontAsset fontAsset)
    {
        _keyButton = GetComponent<Button>();
        _keyTextLeftUp = transform.GetChild(0).GetComponent<TMP_Text>();
        _keyTextMiddle = transform.GetChild(1).GetComponent<TMP_Text>();
        _keyTextLeftUp.font = fontAsset;
        _keyTextMiddle.font = fontAsset;
    }

    public void InitSpecialKey(TMP_FontAsset fontAsset)
    {
        _specialKeyButton = GetComponent<Button>();
        transform.GetChild(0).GetComponent<TMP_Text>().font = fontAsset;
    }

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
