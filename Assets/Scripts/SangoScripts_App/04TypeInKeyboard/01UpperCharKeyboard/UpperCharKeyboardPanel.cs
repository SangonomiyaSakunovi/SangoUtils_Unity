using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpperCharKeyboardPanel : BasePanel
{
    public TMP_FontAsset _standardKeyFont;
    public TMP_FontAsset _specialKeyFont;

    private UpperCharKeyboardSystem _upperCharKeyboardSystem = null;

    private KeyboradDirectionCode _keyboradDirection;

    private Transform _keyboardLine1;
    private Transform _keyboardLine2;
    private Transform _keyboardLine3;
    private Transform _keyboardLine4;

    private string[] _H_line1_KeyValue = { "A", "B", "C", "D", "E", "F", "G", "H", "I" };
    private string[] _H_line2_KeyValue = { "J", "K", "L", "M", "N", "O", "P", "Q" };
    private string[] _H_line3_KeyValue = { "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    private string[] _V_line1_KeyValue = { "A", "B", "C", "D", "E", "F", "G" };
    private string[] _V_line2_KeyValue = { "H", "I", "J", "K", "L", "M", };
    private string[] _V_line3_KeyValue = { "N", "O", "P", "Q", "R", "S", "T" };
    private string[] _V_line4_KeyValue = { "U", "V", "W", "X", "Y", "Z" };

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
            switch (_keyboradDirection)
            {
                case KeyboradDirectionCode.Horizontal:
                    _keyboardLine1 = transform.GetChild(0);
                    _keyboardLine2 = transform.GetChild(1);
                    _keyboardLine3 = transform.GetChild(2);
                    break;
                case KeyboradDirectionCode.Vertical:
                    _keyboardLine1 = transform.GetChild(0);
                    _keyboardLine2 = transform.GetChild(1);
                    _keyboardLine3 = transform.GetChild(2);
                    _keyboardLine4 = transform.GetChild(3);
                    break;
            }
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

    public void SetKeyboradDirection(KeyboradDirectionCode keyboradDirection)
    {
        _keyboradDirection = keyboradDirection;
    }

    private void InitStandardKeysWord()
    {
        switch (_keyboradDirection)
        {
            case KeyboradDirectionCode.Horizontal:
                for (int i = 0; i < _keyboardLine1.childCount; i++)
                {
                    _keyboardLine1.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
                }
                for (int i = 0; i < _keyboardLine2.childCount - 1; i++)
                {
                    _keyboardLine2.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
                }
                for (int i = 0; i < _keyboardLine3.childCount; i++)
                {
                    _keyboardLine3.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
                }
                break;
            case KeyboradDirectionCode.Vertical:
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
                for (int i = 0; i < _keyboardLine4.childCount - 1; i++)
                {
                    _keyboardLine4.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
                }
                break;
        }
    }

    private void InitSpecialKeysWord()
    {
        switch (_keyboradDirection)
        {
            case KeyboradDirectionCode.Horizontal:
                for (int i = 0; i < _keyboardLine2.childCount; i++)
                {
                    if (i == _keyboardLine2.childCount - 1)
                    {
                        _keyboardLine2.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitSpecialKey(_specialKeyFont);
                    }
                }
                break;
            case KeyboradDirectionCode.Vertical:
                for (int i = 0; i < _keyboardLine4.childCount; i++)
                {
                    if (i == _keyboardLine4.childCount - 1)
                    {
                        _keyboardLine4.GetChild(i).GetOrAddComponent<UpperCharKeyboardKeyPrefab>().InitSpecialKey(_specialKeyFont);
                    }
                }
                break;
        }
    }

    private void UpdateStandardKeysWord(bool isDispose)
    {
        switch (_keyboradDirection)
        {
            case KeyboradDirectionCode.Horizontal:
                for (int i = 0; i < _keyboardLine1.childCount; i++)
                {
                    _keyboardLine1.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_H_line1_KeyValue[i], isDispose);
                }
                for (int i = 0; i < _keyboardLine2.childCount; i++)
                {
                    if (i != _keyboardLine2.childCount - 1)
                    {
                        _keyboardLine2.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_H_line2_KeyValue[i], isDispose);
                    }
                }
                for (int i = 0; i < _keyboardLine3.childCount; i++)
                {
                    _keyboardLine3.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_H_line3_KeyValue[i], isDispose);
                }
                break;
            case KeyboradDirectionCode.Vertical:
                for (int i = 0; i < _keyboardLine1.childCount; i++)
                {
                    _keyboardLine1.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_V_line1_KeyValue[i], isDispose);
                }
                for (int i = 0; i < _keyboardLine2.childCount; i++)
                {
                    _keyboardLine2.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_V_line2_KeyValue[i], isDispose);
                }
                for (int i = 0; i < _keyboardLine3.childCount; i++)
                {
                    _keyboardLine3.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_V_line3_KeyValue[i], isDispose);
                }
                for (int i = 0; i < _keyboardLine4.childCount; i++)
                {
                    if (i != _keyboardLine4.childCount - 1)
                    {
                        _keyboardLine4.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateStandardButtonListener(_V_line4_KeyValue[i], isDispose);
                    }
                }
                break;
        }
    }

    private void UpdateSpecialKeysWord(bool isDispose)
    {
        switch (_keyboradDirection)
        {
            case KeyboradDirectionCode.Horizontal:
                for (int i = 0; i < _keyboardLine2.childCount; i++)
                {
                    if (i == _keyboardLine2.childCount - 1)
                    {
                        _keyboardLine2.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateSpecialButtonListener(OnSpecialButtonClickedCallBack, isDispose);
                    }
                }
                break;
            case KeyboradDirectionCode.Vertical:
                for (int i = 0; i < _keyboardLine4.childCount; i++)
                {
                    if (i == _keyboardLine4.childCount - 1)
                    {
                        _keyboardLine4.GetChild(i).GetComponent<UpperCharKeyboardKeyPrefab>().UpdateSpecialButtonListener(OnSpecialButtonClickedCallBack, isDispose);
                    }
                }
                break;
        }
    }

    private void OnSpecialButtonClickedCallBack(string buttonName)
    {
        _upperCharKeyboardSystem.OnSpecialButtonClickedCallBack(buttonName);
    }
}
