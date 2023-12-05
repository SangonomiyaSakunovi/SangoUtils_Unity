using System;
using Unity.VisualScripting;
using UnityEngine;

public class TypeInService : BaseService<TypeInService>
{
    private KeyboardTypeCode _currentKeyboardType = KeyboardTypeCode.FloatableKeyboard;
    private TypeInLanguage _currentTypeInLanguage = TypeInLanguage.English;

    private GameObject _currentKeyboardObject = null;
    private TypeInBaseSystem _currentKeyboardSystem = null;
    private Transform _keyboardDefaultTransform = null;

    private Action<TypeInCommand, string> _onTypeInWordCallBack = null;

    public override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    public void SetKeyboardDefaultTransform(Transform parentTrans)
    {
        _keyboardDefaultTransform = parentTrans;
    }

    public void ShowKeyboard(KeyboardTypeCode type, Action<TypeInCommand, string> onTypeInWordCallBack)
    {
        _onTypeInWordCallBack = onTypeInWordCallBack;
        _currentKeyboardType = type;
        if (_currentKeyboardObject == null)
        {
            switch (_currentKeyboardType)
            {
                case KeyboardTypeCode.FloatableKeyboard:
                    _currentKeyboardObject = InstantiateGameObject(_keyboardDefaultTransform, TypeInConstant.TypeInPanel_FloatableKeyboard_PrefabPath);
                    _currentKeyboardSystem = _currentKeyboardObject.GetComponent<FloatableKeyboardSystem>();
                    _currentKeyboardSystem.SetSystem();
                    break;
                case KeyboardTypeCode.UpperCharKeyboard:
                    _currentKeyboardObject = InstantiateGameObject(_keyboardDefaultTransform, TypeInConstant.TypeInPanel_UpperCharKeyboard_PrefabPath);
                    _currentKeyboardSystem = _currentKeyboardObject.GetComponent<UpperCharKeyboardSystem>();
                    _currentKeyboardSystem.SetSystem();
                    break;
            }
        }
        _currentKeyboardObject.SetActive(true);
        _currentKeyboardSystem.ShowKeyboard();
    }

    public void HideKeyboard()
    {
        _currentKeyboardSystem.HideKeyboard();
        _currentKeyboardObject.SetActive(false);
        _onTypeInWordCallBack = null;
    }

    public void OnTypedInWord(TypeInCommand typeInCommand, string words = "")
    {
        _onTypeInWordCallBack?.Invoke(typeInCommand, words);
    }

}