using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SangoSecurityCheckWnd : BaseWindow
{
    private SangoSecurityCheckRoot _sangoSecurityCheckRoot = null;
    private TypeInConfig _currentTypeInfConfig = null;

    private Transform _keyboardTrans;
    private Transform _inputShowParentTrans;
    private Button _registBtn;
    private Button _skipBtn;
    private TMP_Text _resultShow;

    private int _maxInputStrLenth = 8;
    private string _inputStr = "";

    private TMP_Text[] _inputShowTexts = null;

    public override void SetRoot<T>(BaseRoot<T> baseRoot)
    {
        _sangoSecurityCheckRoot = baseRoot as SangoSecurityCheckRoot;
    }

    public void SetInfo(TypeInConfig config)
    {
        _currentTypeInfConfig = config;
    }

    public void UpdateResult(string result)
    {
        SetText(_resultShow, result);
    }

    protected override void OnInit()
    {
        base.OnInit();

        _keyboardTrans = transform.Find("KeyboradTrans");
        _inputShowParentTrans = transform.Find("SignData/InputShowParent");
        _registBtn = transform.Find("registBtn").GetComponent<Button>();
        _skipBtn = transform.Find("skipBtn").GetComponent<Button>();
        _resultShow = transform.Find("SignData/resultShow").GetComponent<TMP_Text>();

        SetButtonListener(_registBtn, OnRegistSoftwareBtnClicked);
        SetActive(_skipBtn, false);
        ShowKeyboard(_currentTypeInfConfig);
        UpdateBtnInfo("RegistBtn", "����");
        _inputShowTexts = _inputShowParentTrans.GetComponentsInChildren<TMP_Text>();
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        HideKeyboard();
        RemoveAllListeners(_registBtn);
    }

    public void UpdateBtnInfo(string btnName, string commands)
    {
        switch (btnName)
        {
            case "RegistBtn":
                if (commands == "ȷ��")
                {
                    RemoveAllListeners(_registBtn);
                    SetButtonListener(_registBtn, OnRegistOKBtnClicked);
                    SetActive(_skipBtn, false);
                }
                SetText(_registBtn, commands);
                break;
            case "SkipBtn":
                SetActive(_skipBtn);
                SetButtonListener(_skipBtn, OnRegistOKBtnClicked);
                break;
        }
    }

    private void ShowKeyboard(TypeInConfig config)
    {
        TypeInService.Instance.SetKeyboardDefaultTransform(_keyboardTrans);
        switch (config.keyboardTypeCode)
        {
            case KeyboardTypeCode.FloatableKeyboard:
                TypeInService.Instance.SetKeyboardPrefabPath(TypeInConstant.TypeInPanel_FloatableKeyboard_PrefabPath);
                break;
            case KeyboardTypeCode.UpperCharKeyboard:
                TypeInService.Instance.SetKeyboardPrefabPath(TypeInConstant.TypeInPanel_UpperCharKeyboard_PrefabPath);
                TypeInService.Instance.ShowKeyboard<UpperCharKeyboardSystem>(config, OnTypedInWordCallBack);
                break;
            case KeyboardTypeCode.UpperCharKeyboard_4K:
                TypeInService.Instance.SetKeyboardPrefabPath(TypeInConstant.TypeInPanel_UpperCharKeyboard_4K_PrefabPath);
                TypeInService.Instance.ShowKeyboard<UpperCharKeyboardSystem>(config, OnTypedInWordCallBack);
                break;
            case KeyboardTypeCode.UpperCharKeyboard_Vertical_4K:
                TypeInService.Instance.SetKeyboardPrefabPath(TypeInConstant.TypeInPanel_UpperCharKeyboard_Vertical_4K_PrefabPath);
                TypeInService.Instance.ShowKeyboard<UpperCharKeyboardSystem>(config, OnTypedInWordCallBack);
                break;
        }
    }
    private void HideKeyboard()
    {
        TypeInService.Instance.HideKeyboard();
    }
    private void OnRegistSoftwareBtnClicked(Button button)
    {
        _sangoSecurityCheckRoot.UpdateRegistInfo(_inputStr);
    }

    private void OnRegistOKBtnClicked(Button button)
    {
        _sangoSecurityCheckRoot.OnSecurityCheckResultValid();
    }

    private void OnTypedInWordCallBack(TypeInCommand typeInCommand, string words)
    {
        switch (typeInCommand)
        {
            case TypeInCommand.TypeIn:
                if (_inputStr.Length == _maxInputStrLenth) { return; }
                _inputStr += words;
                break;
            case TypeInCommand.Delet:
                if (_inputStr.Length == 0) { return; }
                _inputStr = _inputStr.Remove(_inputStr.Length - 1, 1);
                break;
        }
        UpdateInputShow();
    }

    private void UpdateInputShow()
    {
        for (int i = 0; i < _inputShowTexts.Length; i++)
        {
            _inputShowTexts[i].text = " ";
        }
        for (int k = 0; k < _inputStr.Length; k++)
        {
            _inputShowTexts[k].text = _inputStr[k].ToString();
        }
    }
}
