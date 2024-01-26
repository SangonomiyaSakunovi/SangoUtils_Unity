using SangoUtils_Bases_UnityEngine;
using System;
using UnityEngine;

public class TypeInService : BaseService<TypeInService>
{
    private KeyboardTypeCode _currentKeyboardType = KeyboardTypeCode.FloatableKeyboard;
    //private TypeInLanguage _currentTypeInLanguage = TypeInLanguage.English;
    private KeyboradDirectionCode _currentKeyboradDirection = KeyboradDirectionCode.Horizontal;

    private GameObject _currentKeyboardObject = null;
    private TypeInBaseSystem _currentKeyboardSystem = null;
    private Transform _keyboardDefaultTransform = null;

    private Action<TypeInCommand, string> _onTypeInWordCallBack = null;

    private string _keyboradPrefabPath = "";

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

    public void SetKeyboardPrefabPath(string keyboradPrefabPath)
    {
        _keyboradPrefabPath = keyboradPrefabPath;
    }

    public void ShowKeyboard<T>(TypeInConfig config, Action<TypeInCommand, string> onTypeInWordCallBack) where T : TypeInBaseSystem
    {
        _onTypeInWordCallBack = onTypeInWordCallBack;
        _currentKeyboardType = config.keyboardTypeCode;
        _currentKeyboradDirection = config.keyboradDirectionCode;
        if (_currentKeyboardObject == null)
        {
            _currentKeyboardObject = ResourceService.Instance.InstantiatePrefab(_keyboardDefaultTransform, _keyboradPrefabPath);
            _currentKeyboardSystem = _currentKeyboardObject.GetComponent<T>();
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