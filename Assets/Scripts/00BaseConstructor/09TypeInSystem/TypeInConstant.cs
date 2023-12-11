public class TypeInConstant : BaseConstant
{
    public const string TypeInPanel_FloatableKeyboard_PrefabPath = "Prefabs/TypeInSystemPrefab/FloatableKeyboradPanel";
    public const string TypeInPanel_UpperCharKeyboard_PrefabPath = "Prefabs/TypeInSystemPrefab/UpperCharKeyboradPanel";
    public const string TypeInPanel_UpperCharKeyboard_4K_PrefabPath = "Prefabs/TypeInSystemPrefab/UpperCharKeyboradPanel_4K";
    public const string TypeInPanel_UpperCharKeyboard_Vertical_4K_PrefabPath = "Prefabs/TypeInSystemPrefab/UpperCharKeyboradPanel_Vertical_4K";
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