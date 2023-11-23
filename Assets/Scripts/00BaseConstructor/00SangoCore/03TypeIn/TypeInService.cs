using System;
using UnityEngine;

public class TypeInService : BaseService<TypeInService>
{
    private TypeInMode _currentTypeInMode = TypeInMode.FloatableKeyboard;
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

    public void ShowKeyboard(Action<TypeInCommand, string> onTypeInWordCallBack)
    {
        _onTypeInWordCallBack = onTypeInWordCallBack;
        if (_currentKeyboardObject == null)
        {
            GameObject prefab = null;
            switch (_currentTypeInMode)
            {
                case TypeInMode.FloatableKeyboard:
                    prefab = ResourceService.Instance.LoadPrefab(TypeInConstant.TypeInPanelPrefabPath, false);
                    _currentKeyboardObject = Instantiate(prefab, _keyboardDefaultTransform);
                    _currentKeyboardSystem = _currentKeyboardObject.GetComponent<FloatableKeyboardSystem>();
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