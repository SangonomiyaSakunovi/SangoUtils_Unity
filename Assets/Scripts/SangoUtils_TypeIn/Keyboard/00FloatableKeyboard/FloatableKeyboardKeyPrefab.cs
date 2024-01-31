using SangoUtils_Bases_UnityEngine;
using SangoUtils_Extensions_UnityEngine.Core;
using SangoUtils_Extensions_UnityEngine.Service;
using System;
using TMPro;
using UnityEngine.UI;

namespace SangoUtils_TypeIn
{
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
                _keyButton.onClick.RemoveAllListeners();
                _typeInWord = middleText;
                UpdateButtonName(middleText, leftUpText);
                _keyButton.AddListener_OnClick(OnStandardButtonClicked);
            }
            else
            {
                _keyButton.onClick.RemoveAllListeners();
            }
        }

        public void UpdateSpecialButtonListener(Action<string> specialButtonClickedCallBack, bool isDispose)
        {
            if (!isDispose)
            {
                _specialButtonClickedCallBack = specialButtonClickedCallBack;
                _specialKeyButton.AddListener_OnClick(OnSpecialButtonClicked);
            }
            else
            {
                _specialKeyButton.onClick.RemoveAllListeners();
                _specialButtonClickedCallBack = null;
            }
        }

        private void UpdateButtonName(string middleText, string leftUpText)
        {
            _keyTextMiddle.SetText(middleText);
            _keyTextLeftUp.SetText(leftUpText);
        }

        private void OnStandardButtonClicked()
        {
            TypeInService.Instance.OnTypedInWord(TypeInCommand.TypeIn, _typeInWord);
        }

        public void OnSpecialButtonClicked(Button button)
        {
            _specialButtonClickedCallBack?.Invoke(button.name);
        }
    }
}