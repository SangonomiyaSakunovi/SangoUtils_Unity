using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _05CardMove : MonoBehaviour
{
    public List<Sprite> Sprites = new List<Sprite>();
    Transform Imgs;
    int imgId = 0;
    public int CurrentID = 0;

    void Start()
    {
        Imgs = transform.Find("Imgs");
        transform.Find("LeftBtn").GetComponent<Button>().onClick.AddListener(() => { LeftMoveToRight(true); });
        transform.Find("RightBtn").GetComponent<Button>().onClick.AddListener(() => { LeftMoveToRight(false); });
    }

    void LeftMoveToRight(bool IsLeft)
    {
        imgId = IsLeft == true ? GetImgId(0) : GetImgId(1);
        for (int i = 0; i < Imgs.childCount; i++)
        {
            int k = i;
            Transform img = Imgs.GetChild(k).GetChild(0);
            float posX = Imgs.GetChild(i).localPosition.x;
            if (posX == 500)
            {
                if (IsLeft)
                {
                    SetHoming(img, 0, false);
                    if (imgId == 0) { imgId = Sprites.Count - 1; } else { imgId--; }
                    img.GetComponent<Image>().sprite = Sprites[imgId];
                }
                else { SetHoming(img, 3, true); }
            }
            else if (posX == -500)
            {
                if (IsLeft) { SetHoming(img, 2, true); }
                else
                {
                    SetHoming(img, 1, false);
                    if (imgId == Sprites.Count - 1) { imgId = 0; } else { imgId++; }
                    img.GetComponent<Image>().sprite = Sprites[imgId];
                }
            }
            else if (posX == 250) { if (IsLeft) { SetHoming(img, 1, true); } else { SetHoming(img, 4, true); } }
            else if (posX == -250) { if (IsLeft) { SetHoming(img, 4, true); } else { SetHoming(img, 0, true); } }
            else if (posX == 0) { if (IsLeft) { SetHoming(img, 3, true); } else { SetHoming(img, 2, true); } }
        }
        CurrentID = GetImgId(4);
        //Debug.Log(Sprites[CurrentID].name);//居中图片名称
    }

    void SetHoming(Transform img, int index, bool isbreak)
    {
        img.SetParent(Imgs.GetChild(index));
        img.DOLocalMoveX(0, isbreak == true ? 0.2f : 0).SetEase(Ease.Linear);
        img.DOScale(1f, 0.2f).SetEase(Ease.Linear);
        //斜卡片界面需要，不然可以注销
        {
            img.DOLocalMoveZ(0, 0).SetEase(Ease.Linear);
            img.localRotation = Quaternion.identity;
        }
    }

    int GetImgId(int childId)
    {
        for (int i = 0; i < Sprites.Count; i++)
        {
            if (Imgs.GetChild(childId).GetChild(0).GetComponent<Image>().sprite.name == Sprites[i].name)
            {
                CurrentID = i;
            }
        }
        return CurrentID;
    }
}
