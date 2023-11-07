using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class _05Window : MonoBehaviour
{
    public static bool IsCanClickBtn = true;

    private Vector3 startPos;
    private Vector3 stayPos;
    private Vector3 endPos;
    private Vector3 direction;

    public List<Sprite> Sprites = new List<Sprite>();
    Transform Imgs;
    int imgId = 0;
    public int CurrentID = 0;
     
    void Update()
    {
        //平板端
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    {
                        stayPos = touch.position;
                        if ((startPos - stayPos).magnitude >= 100)
                        {
                            IsCanClickBtn = false;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    {
                        endPos = touch.position;
                        direction = (startPos - endPos);
                        Debug.Log(direction.magnitude);
                        if (direction.magnitude >= 100)
                        {

                            IsCanClickBtn = false;

                            if (Math.Abs(direction.x) > Math.Abs(direction.y))
                            {

                                if (startPos.x > endPos.x)
                                {
                                    print("左移");
                                    LeftMoveToRight(false);
                                }
                                else
                                {
                                    print("右移");
                                    LeftMoveToRight(true);
                                }
                            }
                        }
                    }

                    break;
            }
        }
        else
        {
            //桌面端
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                stayPos = Input.mousePosition;
                if ((startPos - stayPos).magnitude >= 100)
                {
                    IsCanClickBtn = false;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {

                endPos = Input.mousePosition;
                direction = (startPos - endPos);
                if (direction.magnitude >= 100)
                {
                    IsCanClickBtn = false;

                    if (Math.Abs(direction.x) > Math.Abs(direction.y))
                    {
                        if (startPos.x > endPos.x)
                        {
                            LeftMoveToRight(false);
                        }
                        else
                        {
                            LeftMoveToRight(true);
                        }
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
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


