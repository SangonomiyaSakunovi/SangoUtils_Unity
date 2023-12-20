using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class BaseUIElements : MonoBehaviour
{
    List<GameObject> _childrenObjectList = new List<GameObject>();

    public void SetWindowState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive)
        {
            gameObject.SetActive(isActive);
            if (isActive)
            {
                OnInit();
            }
            else
            {
                OnDispose();
            }
        }
    }

    protected virtual void OnInit() { }

    protected virtual void OnDispose() { }

    #region SetResources
    protected void SetSprite(Image image, string path, bool isCache = false)
    {
        Sprite sprite = ResourceService.Instance.LoadSprite(path, isCache);
        image.sprite = sprite;
    }

    protected void SetSprite(GameObject gameObject, string path, bool isCache = false)
    {
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
        {
            SetSprite(image, path, isCache);
        }
    }

    protected void SetSprite(Transform transform, string path, bool isCache = false)
    {
        SetSprite(transform.gameObject, path, isCache);
    }

    protected void SetSprite(Button button, string path, bool isCache = false)
    {
        SetSprite(button.gameObject, path, isCache);
    }

    protected void SetAudio(AudioSource audioSource, string path, bool isCache = false)
    {
        AudioClip clip = ResourceService.Instance.LoadAudioClip(path, isCache);
        audioSource.clip = clip;
    }

    protected void SetAudio(GameObject gameObject, string path, bool isCache = false)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            SetAudio(audioSource, path, isCache);
        }
    }

    protected void SetAudio(Transform transform, string path, bool isCache = false)
    {
        SetAudio(transform.gameObject, path, isCache);
    }

    protected void SetFont(TMP_Text text, string path, bool isChache = true)
    {
        TMP_FontAsset font = ResourceService.Instance.LoadFont(path, isChache);
        text.font = font;
    }

    protected void SetFont(GameObject gameObject, string path, bool isChache = true)
    {
        TMP_Text text = gameObject.GetComponent<TMP_Text>();
        if (text != null)
        {
            SetFont(text, path, isChache);
        }
    }

    protected void SetFont(Transform transform, string path, bool isChache = true)
    {
        SetFont(transform.gameObject, path, isChache);
    }

    protected GameObject InstantiateGameObject(Transform parentTrans, string path, bool isCache = false)
    {
        GameObject instantiatedPrefab = ResourceService.Instance.InstantiatePrefab(parentTrans, path, isCache);
        return instantiatedPrefab;
    }

    protected GameObject InstantiateGameObject(GameObject parentObject, string path, bool isCache = false)
    {
        return InstantiateGameObject(parentObject.transform, path, isCache);
    }
    #endregion

    #region SetOnlineResource
    protected uint SetRawImageOnlineAsync(RawImage rawImage, string urlPath, bool isCache = true, Action<object[]> completeCallBack = null, Action<object[]> canceledCallBack = null, Action<object[]> erroredCallBack = null)
    {
        return ResourceService.Instance.LoadAndSetRawImageOnlineAsync(rawImage, urlPath, isCache, completeCallBack, canceledCallBack, erroredCallBack);
    }

    protected uint SetRawImageOnlineAsync(GameObject gameObject, string urlPath, bool isCache = true, Action<object[]> completeCallBack = null, Action<object[]> canceledCallBack = null, Action<object[]> erroredCallBack = null)
    {
        RawImage rawImage = gameObject.GetComponent<RawImage>();
        if (rawImage != null)
        {
            return SetRawImageOnlineAsync(rawImage, urlPath, isCache, completeCallBack, canceledCallBack, erroredCallBack);
        }
        return 0;
    }

    protected uint SetRawImageOnlineAsync(Transform transform, string urlPath, bool isCache = true, Action<object[]> completeCallBack = null, Action<object[]> canceledCallBack = null, Action<object[]> erroredCallBack = null)
    {
        return SetRawImageOnlineAsync(transform.gameObject, urlPath, isCache, completeCallBack, canceledCallBack, erroredCallBack);
    }

    protected bool RemoveRawImageOnlineAsyncPack(uint packId)
    {
        return ResourceService.Instance.RemoveRawImageOnlineAsyncPack(packId);
    }
    #endregion

    #region SetToggleListener
    protected void SetToggleListeners(Transform toggleRootTrans, UnityAction<Toggle> callBack)
    {
        ToggleGroup toggleGroup = GetOrAddComponent<ToggleGroup>(toggleRootTrans);
        for (int i = 0; i < toggleRootTrans.childCount; i++)
        {
            Transform transform = toggleRootTrans.GetChild(i);
            Toggle toggle = GetOrAddComponent<Toggle>(transform);
            toggle.group = toggleGroup;
            toggle.onValueChanged.AddListener((bool v) => callBack(toggle));
        }
    }

    protected void SetToggleListeners(GameObject toggleRootObject, UnityAction<Toggle> callBack)
    {
        SetToggleListeners(toggleRootObject.transform, callBack);
    }

    protected void SetToggleListener(Transform toggleTrans, UnityAction<Toggle> callBack, Transform toggleRootTrans = null)
    {
        Toggle toggle = GetOrAddComponent<Toggle>(toggleTrans);
        if (toggleRootTrans != null)
        {
            ToggleGroup toggleGroup = GetOrAddComponent<ToggleGroup>(toggleRootTrans);
            toggle.group = toggleGroup;
        }
        toggle.onValueChanged.AddListener((bool v) => callBack(toggle));
    }

    protected void SetToggleListener(GameObject toggleObject, UnityAction<Toggle> callBack, Transform toggleRootTrans = null)
    {
        SetToggleListener(toggleObject.transform, callBack, toggleRootTrans);
    }

    protected void SetToggleListener(Transform toggleTrans, UnityAction<Toggle> callBack, GameObject toggleRootObject = null)
    {
        SetToggleListener(toggleTrans, callBack, toggleRootObject.transform);
    }

    protected void SetToggleListener(GameObject toggleObject, UnityAction<Toggle> callBack, GameObject toggleRootObject = null)
    {
        SetToggleListener(toggleObject.transform, callBack, toggleRootObject.transform);
    }

    protected void SetToggleListener(Toggle toggle, UnityAction<Toggle> callBack, Transform toggleRootTrans = null)
    {
        if (toggleRootTrans != null)
        {
            ToggleGroup toggleGroup = GetOrAddComponent<ToggleGroup>(toggleRootTrans);
            toggle.group = toggleGroup;
        }
        toggle.onValueChanged.AddListener((bool v) => callBack(toggle));
    }
    #endregion

    #region SetButtonListerner
    protected void SetButtonListener(Button button, UnityAction<Button> callBack)
    {
        button.onClick.AddListener(() => callBack(button));
    }

    protected void SetButtonListener(GameObject gameObject, UnityAction<Button> callBack)
    {
        Button button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            SetButtonListener(button, callBack);
        }
    }

    protected void SetButtonListeners(Transform buttonTrans, UnityAction<Button> callBack)
    {
        for (int i = 0; i < buttonTrans.childCount; i++)
        {
            Button button = buttonTrans.GetChild(i).GetComponent<Button>();
            button.onClick.AddListener(() => callBack(button));
        }
    }

    protected void SetButtonListeners(GameObject buttonObj, UnityAction<Button> callBack)
    {
        SetButtonListeners(buttonObj.transform, callBack);
    }
    #endregion

    #region RemoveAllListeners
    protected void RemoveAllListeners(Button button)
    {
        button.onClick.RemoveAllListeners();
    }

    protected void RemoveAllListeners(Toggle toggle)
    {
        toggle.onValueChanged.RemoveAllListeners();
    }

    protected void RemoveAllListeners(Slider slider)
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    protected void RemoveTransListenersButton(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    protected void RemoveTransListenersToggle(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        }
    }

    protected void RemoveTransListenersSlider(Transform transform)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Slider>().onValueChanged.RemoveAllListeners();
        }
    }
    #endregion

    #region Component
    protected T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t == null)
        {
            t = gameObject.AddComponent<T>();
        }
        return t;
    }

    protected T GetOrAddComponent<T>(Transform transform) where T : Component
    {
        return GetOrAddComponent<T>(transform.gameObject);
    }

    protected void RemoveComponent<T>(GameObject gameObject) where T : Component
    {
        T t = gameObject.GetComponent<T>();
        if (t != null)
        {
            Destroy(t);
        }
    }

    protected void RemoveComponent<T>(Transform transform) where T : Component
    {
        RemoveComponent<T>(transform.gameObject);
    }
    #endregion

    #region ButtonInteractable
    protected void SetButtonInteractable(Button button, bool isInteractable = true)
    {
        button.interactable = isInteractable;
    }

    protected void SetButtonInteractable(GameObject gameObject, bool isInteractable = true)
    {
        Button button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            SetButtonInteractable(button, isInteractable);
        }
    }

    protected void SetButtonInteractable(Transform transform, bool isInteractable = true)
    {
        SetButtonInteractable(transform.gameObject, isInteractable);
    }
    #endregion

    #region SetActive
    protected void SetActive(GameObject gameObject, bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }

    protected void SetActive(Transform transform, bool isActive = true)
    {
        SetActive(transform.gameObject, isActive);
    }

    protected void SetActive(Button button, bool isActive = true)
    {
        SetActive(button.gameObject, isActive);
    }

    protected void SetActive(TMP_Text tMP_Text, bool isActive = true)
    {
        SetActive(tMP_Text.gameObject, isActive);
    }

    protected void SetActive(TMP_InputField tMP_InputField, bool isActive = true)
    {
        SetActive(tMP_InputField.gameObject, isActive);
    }
    #endregion

    #region SetMutiActive
    protected void SetMultiActive(bool isActive = true, params GameObject[] gameObjects)
    {
        if (gameObjects != null)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                SetActive(gameObjects[i], isActive);
            }
        }
    }

    protected void SetMultiActive(bool isActive = true, params Button[] buttons)
    {
        if (buttons != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                SetActive(buttons[i], isActive);
            }
        }
    }

    protected void SetMultiActive(bool isActive = true, params TMP_Text[] tMP_Texts)
    {
        if (tMP_Texts != null)
        {
            for (int i = 0; i < tMP_Texts.Length; i++)
            {
                SetActive(tMP_Texts[i], isActive);
            }
        }
    }

    protected void SetMultiActive(bool isActive = true, params TMP_InputField[] tMP_InputFields)
    {
        if (tMP_InputFields != null)
        {
            for (int i = 0; i < tMP_InputFields.Length; i++)
            {
                SetActive(tMP_InputFields[i], isActive);
            }
        }
    }

    protected void SetMultiActive<T>(bool isActive = true, params object[] objects) where T : UnityEngine.Object
    {
        if (objects != null)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                System.Reflection.MethodInfo gameObjectMethod = objects[i].GetType().GetMethod("gameObject");
                if (gameObjectMethod != null)
                {
                    GameObject gameObject = gameObjectMethod.Invoke(objects[i], null) as GameObject;
                    if (gameObject != null)
                    {
                        SetActive(gameObject, isActive);
                    }
                }
            }
        }
    }
    #endregion

    #region SetColor
    protected void SetColor(Image image, int RValue, int GValue, int BValue, int AValue = 255)
    {
        Color color = new Color(RValue / 255f, GValue / 255f, BValue / 255f);
        color.a = AValue / 255f;
        image.color = color;
    }

    protected void SetColor(Button button, int RValue, int GValue, int BValue, int AValue = 255)
    {
        Image image = button.GetComponent<Image>();
        if (image != null)
        {
            SetColor(image, RValue, GValue, BValue, AValue);
        }
    }
    #endregion

    #region SetText
    protected void SetText(TMP_Text tMP_Text, string textStr)
    {
        tMP_Text.text = textStr;
    }

    protected void SetText(TMP_InputField tMP_InputField, string textStr)
    {
        tMP_InputField.text = textStr;
    }

    protected void SetText(GameObject gameObject, string textStr)
    {
        TMP_Text tMP_Text = gameObject.GetComponent<TMP_Text>();
        if (tMP_Text == null)
        {
            tMP_Text = gameObject.GetComponentInChildren<TMP_Text>();
        }
        if (tMP_Text != null)
        {
            SetText(tMP_Text, textStr);
        }
    }

    protected void SetText(Transform transform, string textStr)
    {
        SetText(transform.gameObject, textStr);
    }

    protected void SetText(Button button, string textStr)
    {
        SetText(button.gameObject, textStr);
    }
    #endregion

    #region SetMultiText
    protected void SetMultiText(string textStr, params TMP_Text[] tMP_Texts)
    {
        if (tMP_Texts != null)
        {
            for (int i = 0; i < tMP_Texts.Length; i++)
            {
                SetText(tMP_Texts[i], textStr);
            }
        }
    }

    protected void SetMultiText(string textStr, params GameObject[] tMP_Texts)
    {
        if (tMP_Texts != null)
        {
            for (int i = 0; i < tMP_Texts.Length; i++)
            {
                SetText(tMP_Texts[i], textStr);
            }
        }
    }

    protected void SetMultiText(string textStr, params Transform[] tMP_Texts)
    {
        if (tMP_Texts != null)
        {
            for (int i = 0; i < tMP_Texts.Length; i++)
            {
                SetText(tMP_Texts[i], textStr);
            }
        }
    }
    #endregion

    #region DestoryAllChildGameObjects
    protected void DestoryAllChildGameObjects(Transform parentTrans)
    {
        for (int i = 0; i < parentTrans.childCount; i++)
        {
            _childrenObjectList.Add(parentTrans.GetChild(i).gameObject);
        }
        for (int j = 0; j < _childrenObjectList.Count; j++)
        {
            Destroy(_childrenObjectList[j]);
        }
        ObjectPoolService.Instance.Release(_childrenObjectList);
        _childrenObjectList.Clear();
    }

    protected void DestoryAllChildGameObjects(GameObject gameObject)
    {
        DestoryAllChildGameObjects(gameObject.transform);
    }

    protected void DestoryAllChildGameObjectsImmediatly(Transform parentTrans)
    {
        for (int i = 0; i < parentTrans.childCount; i++)
        {
            _childrenObjectList.Add(parentTrans.GetChild(i).gameObject);
        }
        for (int j = 0; j < _childrenObjectList.Count; j++)
        {
            DestroyImmediate(_childrenObjectList[j]);
        }
        _childrenObjectList.Clear();
    }

    protected void DestoryAllChildGameObjectsImmediatly(GameObject gameObject)
    {
        DestoryAllChildGameObjectsImmediatly(gameObject.transform);
    }
    #endregion

    #region SetName
    protected void SetName(GameObject gameObject, string name)
    {
        gameObject.name = name;
    }

    protected void SetName(Transform transform, string name)
    {
        transform.name = name;
    }

    protected void SetName(Toggle toggle, string name)
    {
        toggle.name = name;
    }

    protected void SetName(Button button, string name)
    {
        button.name = name;
    }

    protected void SetName(GameObject gameObject, int name)
    {
        gameObject.name = name.ToString();
    }

    protected void SetName(Transform transform, int name)
    {
        transform.name = name.ToString();
    }

    protected void SetName(Toggle toggle, int name)
    {
        toggle.name = name.ToString();
    }

    protected void SetName(Button button, int name)
    {
        button.name = name.ToString();
    }
    #endregion

    #region StringProcess
    protected string GetPreviousSubString(string fullStr, int trimLength)
    {
        if (fullStr.Length > trimLength)
        {
            return fullStr.Substring(0, fullStr.Length - trimLength);
        }
        return fullStr;
    }

    protected string GetPrefabNameRemovedCloneTag(string prefabName)
    {
        return GetPreviousSubString(prefabName, 7);
    }

    protected string GetPrefabNameRemovedCloneTag(GameObject gameObject)
    {
        return GetPrefabNameRemovedCloneTag(gameObject.name);
    }

    protected string GetPrefabNameRemovedCloneTag(Transform transform)
    {
        return GetPrefabNameRemovedCloneTag(transform.name);
    }

    protected string GetPrefabNameRemovedCloneTag(Button button)
    {
        return GetPrefabNameRemovedCloneTag(button.name);
    }

    protected string GetPrefabNameRemovedCloneTag(Toggle toggle)
    {
        return GetPrefabNameRemovedCloneTag(toggle.name);
    }
    #endregion

    #region NumberProcess
    protected int GetNullableInt32(string str)
    {
        if (!string.IsNullOrEmpty(str))
        {
            return int.Parse(str);
        }
        return 0;
    }
    #endregion

    #region SetUnActiveAllChild
    protected void SetUnActiveAllChild(Transform rootTrans)
    {
        for (int i = 0; i < rootTrans.childCount; i++)
        {
            SetActive(rootTrans.GetChild(i), false);
        }
    }

    protected void SetUnActiveAllChild(GameObject rootObject)
    {
        SetUnActiveAllChild(rootObject.transform);
    }
    #endregion

    #region PlayUIAnimation
    protected void AddUIAnimation(string id, SangoUIBaseAnimation sangoUIAnimation, Action completeCallBack = null, Action cancelCallBack = null)
    {
        UIAnimationService.Instance.AddAnimation(id, sangoUIAnimation, completeCallBack, cancelCallBack);
    }

    protected void PlayUIAnimation(string id, params string[] commands)
    {
        UIAnimationService.Instance.PlayAnimation(id, commands);
    }

    protected void PlayUIAnimationAsync(string id, params string[] commands)
    {
        UIAnimationService.Instance.PlayAnimationAsync(id, commands);
    }
    #endregion

    #region UIPointerListener
    protected void SetPointerClickListener(GameObject gameObject, Action<PointerEventData, GameObject, object[]> onPointerClickCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerClickCallBack1 = onPointerClickCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerDownListener(GameObject gameObject, Action<PointerEventData, GameObject, object[]> onPointerDownCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerDownCallBack1 = onPointerDownCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerUpListener(GameObject gameObject, Action<PointerEventData, GameObject, object[]> onPointerUpCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerUpCallBack1 = onPointerUpCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerDragListener(GameObject gameObject, Action<PointerEventData, GameObject, object[]> onPointerDragCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerDragCallBack1 = onPointerDragCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerClickListener(Transform transform, Action<PointerEventData, GameObject, object[]> onPointerClickCallBack, params object[] commands)
    {
        SetPointerClickListener(transform.gameObject, onPointerClickCallBack, commands);
    }

    protected void SetPointerDownListener(Transform transform, Action<PointerEventData, GameObject, object[]> onPointerDownCallBack, params object[] commands)
    {
        SetPointerDownListener(transform.gameObject, onPointerDownCallBack, commands);
    }

    protected void SetPointerUpListener(Transform transform, Action<PointerEventData, GameObject, object[]> onPointerUpCallBack, params object[] commands)
    {
        SetPointerUpListener(transform.gameObject, onPointerUpCallBack, commands);
    }

    protected void SetPointerDragListener(Transform transform, Action<PointerEventData, GameObject, object[]> onPointerDragCallBack, params object[] commands)
    {
        SetPointerDragListener(transform.gameObject, onPointerDragCallBack, commands);
    }

    protected void SetPointerClickListener(GameObject gameObject, Action<GameObject, object[]> onPointerClickCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerClickCallBack0 = onPointerClickCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerDownListener(GameObject gameObject, Action<GameObject, object[]> onPointerDownCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerDownCallBack0 = onPointerDownCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerUpListener(GameObject gameObject, Action<GameObject, object[]> onPointerUpCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerUpCallBack0 = onPointerUpCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerDragListener(GameObject gameObject, Action<GameObject, object[]> onPointerDragCallBack, params object[] commands)
    {
        UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
        listener.onPointerDragCallBack0 = onPointerDragCallBack;
        listener.listenerObject = gameObject;
        if (commands != null)
        {
            listener.commands = commands;
        }
    }

    protected void SetPointerClickListener(Transform transform, Action<GameObject, object[]> onPointerClickCallBack, params object[] commands)
    {
        SetPointerClickListener(transform.gameObject, onPointerClickCallBack, commands);
    }

    protected void SetPointerDownListener(Transform transform, Action<GameObject, object[]> onPointerDownCallBack, params object[] commands)
    {
        SetPointerDownListener(transform.gameObject, onPointerDownCallBack, commands);
    }

    protected void SetPointerUpListener(Transform transform, Action<GameObject, object[]> onPointerUpCallBack, params object[] commands)
    {
        SetPointerUpListener(transform.gameObject, onPointerUpCallBack, commands);
    }

    protected void SetPointerDragListener(Transform transform, Action<GameObject, object[]> onPointerDragCallBack, params object[] commands)
    {
        SetPointerDragListener(transform.gameObject, onPointerDragCallBack, commands);
    }
    #endregion

    #region PointerSlideListener
    protected void SetPointerSlideListener(GameObject gameObject, Action<GameObject, object[]> onPointerSlideCallBack, Action<GameObject, object[]> onPointerClickDoneCallBack, params object[] commands)
    {
        SetPointerDownListener(gameObject, (PointerEventData eventData, GameObject gameObject, object[] strs) =>
        {
            UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
            listener.clickDownPosition = eventData.position;
        });
        SetPointerDragListener(gameObject, (PointerEventData eventData, GameObject gameObject, object[] strs) =>
        {
            UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
            Vector2 direction = eventData.position - listener.clickDownPosition;
            onPointerSlideCallBack?.Invoke(gameObject, new object[] { direction });
        },
        commands);
        SetPointerUpListener(gameObject, (PointerEventData eventData, GameObject gameObject, object[] strs) =>
        {
            UIPointerListener listener = GetOrAddComponent<UIPointerListener>(gameObject);
            Vector2 direction = eventData.position - listener.clickDownPosition;
            onPointerClickDoneCallBack?.Invoke(gameObject, new object[] { direction });
        });
    }

    protected void SetPointerSlideListener(Transform transform, Action<GameObject, object[]> onPointerSlideCallBack, Action<GameObject, object[]> onPointerClickDoneCallBack, params object[] commands)
    {
        SetPointerSlideListener(transform.gameObject, onPointerSlideCallBack, onPointerClickDoneCallBack, commands);
    }
    #endregion   

    #region EventTrigger
    protected void SetGameObjectClickListener(GameObject gameObject, UnityAction<BaseEventData> actionCallBack)
    {
        EventTrigger eventTrigger = GetOrAddComponent<EventTrigger>(gameObject);
        for (int i = 0; i < eventTrigger.triggers.Count; i++)
        {
            if (eventTrigger.triggers[i].eventID == EventTriggerType.PointerClick)
            {
                return;
            }
        }
        Entry onClick = new Entry()
        {
            eventID = EventTriggerType.PointerClick
        };
        onClick.callback.AddListener(actionCallBack);
        eventTrigger.triggers.Add(onClick);
    }

    protected void SetGameObjectClickListener(Transform transform, UnityAction<BaseEventData> actionCallBack)
    {
        SetGameObjectClickListener(transform.gameObject, actionCallBack);
    }

    protected void SetGameObjectClickListener(TMP_InputField inputField, UnityAction<BaseEventData> actionCallBack)
    {
        SetGameObjectClickListener(inputField.gameObject, actionCallBack);
    }

    protected void SetGameObjectClickListener(Image image, UnityAction<BaseEventData> actionCallBack)
    {
        SetGameObjectClickListener(image.gameObject, actionCallBack);
    }

    protected void SetGameObjectDragBeginListener(GameObject gameObject, UnityAction<BaseEventData> actionCallBack)
    {
        EventTrigger eventTrigger = GetOrAddComponent<EventTrigger>(gameObject);
        for (int i = 0; i < eventTrigger.triggers.Count; i++)
        {
            if (eventTrigger.triggers[i].eventID == EventTriggerType.BeginDrag)
            {
                return;
            }
        }
        Entry onDragBegin = new Entry()
        {
            eventID = EventTriggerType.BeginDrag
        };
        onDragBegin.callback.AddListener(actionCallBack);
        eventTrigger.triggers.Add(onDragBegin);
    }

    protected void SetGameObjectDragBeginListener(Slider slider, UnityAction<BaseEventData> actionCallBack)
    {
        SetGameObjectDragBeginListener(slider.gameObject, actionCallBack);
    }

    protected void SetGameObjectDragEndListener(GameObject gameObject, UnityAction<BaseEventData> actionCallBack)
    {
        EventTrigger eventTrigger = GetOrAddComponent<EventTrigger>(gameObject);
        for (int i = 0; i < eventTrigger.triggers.Count; i++)
        {
            if (eventTrigger.triggers[i].eventID == EventTriggerType.EndDrag)
            {
                return;
            }
        }
        Entry onDragEnd = new Entry()
        {
            eventID = EventTriggerType.EndDrag
        };
        onDragEnd.callback.AddListener(actionCallBack);
        eventTrigger.triggers.Add(onDragEnd);
    }

    protected void SetGameObjectDragEndListener(Slider slider, UnityAction<BaseEventData> actionCallBack)
    {
        SetGameObjectDragEndListener(slider.gameObject, actionCallBack);
    }
    #endregion
}
