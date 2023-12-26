public class UpperCharKeyboardSystem : TypeInBaseSystem
{
    public UpperCharKeyboardPanel _upperCharKeyboardPanel;

    public override void OnInit() 
    {
        base.OnInit();
    }

    public override void SetSystem()
    {
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

    public override void SetKeyboardDirection(KeyboradDirectionCode directionCode)
    {
        base.SetKeyboardDirection(directionCode);
        _upperCharKeyboardPanel.SetKeyboradDirection(directionCode);
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
