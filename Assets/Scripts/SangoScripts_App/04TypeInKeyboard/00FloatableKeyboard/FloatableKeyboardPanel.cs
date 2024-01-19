using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace SangoUtils_Unity_App.InputSystem
{
    public class FloatableKeyboardPanel : BasePanel
    {
        public TMP_FontAsset _standardKeyFont;
        public TMP_FontAsset _specialKeyFont;

        private FloatableKeyboardSystem _floatableKeyboardSystem = null;

        private Transform _keyboardLine1;
        private Transform _keyboardLine2;
        private Transform _keyboardLine3;
        private Transform _keyboardLine4;
        private Transform _keyboardLine5;

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
                _keyboardLine1 = transform.GetChild(0);
                _keyboardLine2 = transform.GetChild(1);
                _keyboardLine3 = transform.GetChild(2);
                _keyboardLine4 = transform.GetChild(3);
                _keyboardLine5 = transform.GetChild(4);
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

        public void OnShiftButtonClickedCallBack()
        {
            UpdateStandardKeysWord(false);
        }

        private void InitStandardKeysWord()
        {
            for (int i = 0; i < _keyboardLine1.childCount; i++)
            {
                _keyboardLine1.GetChild(i).GetOrAddComponent<FloatableKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
            }
            for (int i = 0; i < _keyboardLine2.childCount; i++)
            {
                _keyboardLine2.GetChild(i).GetOrAddComponent<FloatableKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
            }
            for (int i = 0; i < _keyboardLine3.childCount; i++)
            {
                _keyboardLine3.GetChild(i).GetOrAddComponent<FloatableKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
            }
            for (int i = 1; i < _keyboardLine4.childCount - 1; i++)
            {
                _keyboardLine4.GetChild(i).GetOrAddComponent<FloatableKeyboardKeyPrefab>().InitStandardKey(_standardKeyFont);
            }
        }

        private void InitSpecialKeysWord()
        {
            for (int i = 0; i < _keyboardLine4.childCount; i++)
            {
                if (i == 0 || i == _keyboardLine4.childCount - 1)
                {
                    _keyboardLine4.GetChild(i).GetOrAddComponent<FloatableKeyboardKeyPrefab>().InitSpecialKey(_specialKeyFont);
                }
            }
            for (int i = 0; i < _keyboardLine5.childCount; i++)
            {
                _keyboardLine5.GetChild(i).GetOrAddComponent<FloatableKeyboardKeyPrefab>().InitSpecialKey(_specialKeyFont);
            }
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
            for (int i = 1; i < _keyboardLine4.childCount - 1; i++)
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
}