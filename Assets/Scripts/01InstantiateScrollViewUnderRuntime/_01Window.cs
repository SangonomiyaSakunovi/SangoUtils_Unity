using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class _01Window : MonoBehaviour
{
    [Header("ScrollPoint")]
    public Transform _scrollParentTrans;

    private string _prefabPath = "01InstantiateScrollViewUnderRuntime/SelfitPrefab";

    public void InstantiatePrefab()
    {
        List<_01ClassInfo> infos = _01System.Instance.GetClassInfo();
        GameObject prefab = Resources.Load<GameObject>(_prefabPath);
        for (int i = 0; i < infos.Count; i++)
        {
            GameObject instantiatedPrefab = Instantiate(prefab, _scrollParentTrans);
            Transform headTrans = instantiatedPrefab.transform.GetChild(0);
            headTrans.GetChild(1).GetComponent<TMP_Text>().text = infos[i].Head;
            Transform selfitTrans = instantiatedPrefab.transform.GetChild(1);
            selfitTrans.GetComponent<TMP_Text>().text = infos[i].Content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(selfitTrans.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(instantiatedPrefab.GetComponent<RectTransform>());
        }
    }
}
