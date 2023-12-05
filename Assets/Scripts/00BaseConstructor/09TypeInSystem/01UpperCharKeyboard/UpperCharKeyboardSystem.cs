public class UpperCharKeyboardSystem : TypeInBaseSystem
{
    public UpperCharKeyboardPanel _upperCharKeyboardPanel;

    public override void InitSystem()
    {
        base.InitSystem();
    }

    public override void SetSystem()
    {
        base.SetSystem();
        _upperCharKeyboardPanel.SetSystem(this);
    }

    public override void ShowKeyboard()
    {
        _upperCharKeyboardPanel.ShowKeyboard();
    }

    public override void HideKeyboard()
    {
        _upperCharKeyboardPanel.HideKeyboard();
    }

    public void OnSpecialButtonClickedCallBack(string buttonName)
    {
        switch (buttonName)
        {
            case "Delet":
                TypeInService.Instance.OnTypedInWord(TypeInCommand.Delet);
                break;
        }
    }
}
