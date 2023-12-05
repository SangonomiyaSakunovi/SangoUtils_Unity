using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpperCharKeyboardPanel : BasePanel
{
    public TMP_FontAsset _standardKeyFont;
    public TMP_FontAsset _specialKeyFont;

    private UpperCharKeyboardSystem _upperCharKeyboardSystem = null;

    private Transform _keyboardLine1;
    private Transform _keyboardLine2;
    private Transform _keyboardLine3;

    private string[] _line1_KeyValue = { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
    private string[] _line2_KeyValue = { "J", "K", "L", "M", "N", "O", "P", "Q" };
    private string[] _line3_KeyValue = { "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
    }

    public void SetSystem(UpperCharKeyboardSystem system)
    {
        if (_upperCharKeyboardSystem == null)
        {
            _upperCharKeyboardSystem = system;
            _keyboardLine1 = transform.GetChild(0);
            _keyboardLine2 = transform.GetChild(1);
            _keyboardLine3 = transform.GetChild(2);
        }
    }

    public void ShowKeyboard()
    {
        InitStandardKeysWord();
        InitSpecialKeysWord();
        UpdateStandardKeysWord(false);
        UpdateSpecialKeysWord(false);
    }

    public void HideKeyboard()
    {
        UpdateStandardKeysWord(true);
        UpdateSpecialKeysWord(true);
    }

    private void InitStandardKeysWord()
    {
        for (int i = 0; i < _keyboardLine1.childCount; i++)
        {
            _keyboardLine1.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
        }
        for (int i = 0; i < _keyboardLine2.childCount; i++)
        {
            _keyboardLine2.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
        }
        for (int i = 0; i < _keyboardLine3.childCount; i++)
        {
            _keyboardLine3.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
        }
    }

    private void InitSpecialKeysWord()
    {       
        for (int i = 0; i < _keyboardLine2.childCount; i++)
        {
            if (i == _keyboardLine2.childCount - 1)
            {
                _keyboardLine2.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitSpecialKey(_specialKeyFont);
            }
        }
    }

    private void UpdateStandardKeysWord(bool isDispose)
    {
        for (int i = 0; i < _keyboardLine1.childCount; i++)
        {
            _keyboardLine1.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_line1_KeyValue[i], isDispose);
        }
        for (int i = 0; i < _keyboardLine2.childCount; i++)
        {
            if (i != _keyboardLine2.childCount - 1)
            {
                _keyboardLine2.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_line2_KeyValue[i], isDispose);
            }
        }
        for (int i = 0; i < _keyboardLine3.childCount; i++)
        {
            _keyboardLine3.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_line3_KeyValue[i], isDispose);
        }
    }

    private void UpdateSpecialKeysWord(bool isDispose)
    {
        for (int i = 0; i < _keyboardLine2.childCount; i++)
        {
            if (i == _keyboardLine2.childCount - 1)
            {
                _keyboardLine2.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateSpecialButtonListener(OnSpecialButtonClickedCallBack, isDispose);
            }
        }
    }

    private void OnSpecialButtonClickedCallBack(string buttonName)
    {
        _upperCharKeyboardSystem.OnSpecialButtonClickedCallBack(buttonName);
    }
}
