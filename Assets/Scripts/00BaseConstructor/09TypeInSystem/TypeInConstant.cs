public class TypeInConstant : BaseConstant
{
    public const string TypeInPanel_FloatableKeyboard_PrefabPath = "Prefabs/TypeInSystemPrefab/FloatableKeyboradPanel";
    public const string TypeInPanel_UpperCharKeyboard_PrefabPath = "Prefabs/TypeInSystemPrefab/UpperCharKeyboradPanel";
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
    UpperCharKeyboard
}
