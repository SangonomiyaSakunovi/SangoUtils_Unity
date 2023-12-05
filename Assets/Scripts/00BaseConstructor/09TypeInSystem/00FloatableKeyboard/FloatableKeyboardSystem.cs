public class FloatableKeyboardSystem : TypeInBaseSystem
{
    public FloatableKeyboardPanel _floatableKeyboardPanel;

    public override void InitSystem()
    {
        base.InitSystem();
    }

    public override void SetSystem()
    {
        base.SetSystem();
        _floatableKeyboardPanel.SetSystem(this);
    }

    public override void ShowKeyboard()
    {
        _floatableKeyboardPanel.ShowKeyboard();
    }

    public override void HideKeyboard()
    {
        _floatableKeyboardPanel.HideKeyboard();
    }

    public void OnSpecialButtonClickedCallBack(string buttonName)
    {
        switch (buttonName)
        {
            case "Shift":
                _floatableKeyboardPanel.OnShiftButtonClickedCallBack();
                break;
            case "Delet":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.Delet);
                break;
            case "Clear":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.Clear);
                break;
            case "En":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.EnAlt);
                break;
            case "Space":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.Space);
                break;
            case "Cancel":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.Cancel);
                break;
            case "Confirm":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.Confirm);
                break;
        }
    }
}
