using System;
using UnityEngine;

public class TypeInService : BaseService<TypeInService>
{
    private KeyboardTypeCode _currentKeyboardType = KeyboardTypeCode.FloatableKeyboard;
    private TypeInLanguage _currentTypeInLanguage = TypeInLanguage.English;
    private KeyboradDirectionCode _currentKeyboradDirection = KeyboradDirectionCode.Horizontal;

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

    public void ShowKeyboard(TypeInConfig config, Action<TypeInCommand, string> onTypeInWordCallBack)
    {
        _onTypeInWordCallBack = onTypeInWordCallBack;
        _currentKeyboardType = config.keyboardTypeCode;
        _currentKeyboradDirection = config.keyboradDirectionCode;
        if (_currentKeyboardObject == null)
        {
            switch (_currentKeyboardType)
            {
                case KeyboardTypeCode.FloatableKeyboard:
                    _currentKeyboardObject = InstantiateGameObject(_keyboardDefaultTransform, TypeInConstant.TypeInPanel_FloatableKeyboard_PrefabPath);                 
                    break;
                case KeyboardTypeCode.UpperCharKeyboard:
                    _currentKeyboardObject = InstantiateGameObject(_keyboardDefaultTransform, TypeInConstant.TypeInPanel_UpperCharKeyboard_PrefabPath);
                    break;
                case KeyboardTypeCode.UpperCharKeyboard_4K:
                    _currentKeyboardObject = InstantiateGameObject(_keyboardDefaultTransform, TypeInConstant.TypeInPanel_UpperCharKeyboard_4K_PrefabPath);
                    break;
                case KeyboardTypeCode.UpperCharKeyboard_Vertical_4K:
                    _currentKeyboardObject = InstantiateGameObject(_keyboardDefaultTransform, TypeInConstant.TypeInPanel_UpperCharKeyboard_Vertical_4K_PrefabPath);
                    break;
            }
            _currentKeyboardSystem = _currentKeyboardObject.GetComponent<UpperCharKeyboardSystem>();
            _currentKeyboardSystem.SetKeyboardDirection(_currentKeyboradDirection);
            _currentKeyboardSystem.SetSystem();
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

public enum TypeInCommand
{
    TypeIn,
    Delet,
    Clear,
    EnAlt,
    Space,
    Cancel,
    Confirm
}

public enum TypeInLanguage
{
    English,
}

public enum KeyboardTypeCode
{
    FloatableKeyboard,
    UpperCharKeyboard,
    UpperCharKeyboard_4K,
    UpperCharKeyboard_Vertical_4K
}

public enum KeyboradDirectionCode
{
    Horizontal,
    Vertical
}


public class TypeInConfig
{
    public TypeInLanguage typeInLanguage;
    public KeyboardTypeCode keyboardTypeCode;
    public KeyboradDirectionCode keyboradDirectionCode;
}