using UnityEngine;

public class FloatableKeyboardPanel : BasePanel
{
    private FloatableKeyboardSystem _floatableKeyboardSystem = null;

    [Header("KeyboardTransRoot")]
    public Transform _keyboardLine1;
    public Transform _keyboardLine2;
    public Transform _keyboardLine3;
    public Transform _keyboardLine4;
    public Transform _keyboardLine5;

    private bool _currentIsShiftMode = true;

    private string[][] _line1_KeyValue = {
            new string[]{"`","1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "="},
            new string[]{"¡¤", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+" }};

    private string[][] _line2_KeyValue = {
            new string[]{"q","w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\"},
            new string[]{"Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "{", "}", "|" }};

    private string[][] _line3_KeyValue = {
            new string[]{"a","s", "d", "f", "g", "h", "j", "k", "l", ";", "'"},
            new string[]{"A", "S", "D", "F", "G", "H", "J", "K", "L", ":", "\""}};

    private string[][] _line4_KeyValue = {
            new string[]{"shift","z","x", "c", "v", "b", "n", "m", ",", ".", "/","delet"},
            new string[]{"Shift","Z", "X", "C", "V", "B", "N", "M", "<", ">", "?","Delet"}};

    protected override void OnInit()
    {
        base.OnInit();        
    }

    protected override void OnDispose()
    {
        base.OnDispose();
    }

    public void SetSystem(FloatableKeyboardSystem system)
    {
        if (_floatableKeyboardSystem == null)
        {
            _floatableKeyboardSystem = system;
        }
    }

    public void ShowKeyboard()
    {
        UpdateStandardKeysWord(false);
        UpdateSpecialKeysWord(false);
    }

    public void HideKeyboard()
    {
        UpdateStandardKeysWord(true);
        UpdateSpecialKeysWord(true);
    }

    public void OnShiftButtonClickedCallBack()
    {
        UpdateStandardKeysWord(false);
    }

    private void UpdateStandardKeysWord(bool isDispose)
    {
        _currentIsShiftMode = !_currentIsShiftMode;
        for (int i = 0; i < _keyboardLine1.childCount; i++)
        {
            if (!_currentIsShiftMode)
            {
                _keyboardLine1.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line1_KeyValue[0][i], _line1_KeyValue[1][i], isDispose);
            }
            else
            {
                _keyboardLine1.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line1_KeyValue[1][i], _line1_KeyValue[0][i], isDispose);
            }
        }
        for (int i = 0; i < _keyboardLine2.childCount; i++)
        {
            if (!_currentIsShiftMode)
            {
                _keyboardLine2.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line2_KeyValue[0][i], _line2_KeyValue[1][i], isDispose);
            }
            else
            {
                _keyboardLine2.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line2_KeyValue[1][i], _line2_KeyValue[0][i], isDispose);
            }
        }
        for (int i = 0; i < _keyboardLine3.childCount; i++)
        {
            if (!_currentIsShiftMode)
            {
                _keyboardLine3.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line3_KeyValue[0][i], _line3_KeyValue[1][i], isDispose);
            }
            else
            {
                _keyboardLine3.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line3_KeyValue[1][i], _line3_KeyValue[0][i], isDispose);
            }
        }
        for (int i = 1; i < _keyboardLine4.childCount-1; i++)
        {
            if (!_currentIsShiftMode)
            {
                _keyboardLine4.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line4_KeyValue[0][i], _line4_KeyValue[1][i], isDispose);
            }
            else
            {
                _keyboardLine4.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateStandardButtonListener(_line4_KeyValue[1][i], _line4_KeyValue[0][i], isDispose);
            }
        }        
    }

    private void UpdateSpecialKeysWord(bool isDispose)
    {        
        for (int i = 0; i < _keyboardLine4.childCount; i++)
        {
            if (i == 0 || i == _keyboardLine4.childCount - 1)
            {
                _keyboardLine4.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateSpecialButtonListener(OnSpecialButtonClickedCallBack, isDispose);
            }           
        }
        for (int i = 0; i < _keyboardLine5.childCount; i++)
        {
            _keyboardLine5.GetChild(i).GetComponent<FloatableKeyboardKeyPrefab>().UpdateSpecialButtonListener(OnSpecialButtonClickedCallBack, isDispose);
        }
    }

    private void OnSpecialButtonClickedCallBack(string buttonName)
    {
        _floatableKeyboardSystem.OnSpecialButtonClickedCallBack(buttonName);        
    }
}
