using System;
using UnityEngine;

public class _05CardMove : BaseWindow
{
    public Transform _parentTrans;
    public float _duration = 0.2f;

    public string _id = "sangoTest";

    public Transform _slideControl;

    private void Start()
    {
        SangoUIBaseAnimation animation = SangoUIAnimationAssets.SangoUISlideBroadCastImageAnimation(_parentTrans, _duration);
        AddUIAnimation(_id, animation);
        SetPointerSlideListener(_slideControl.gameObject, OnPointerHorizontalSlided, OnPointerHorizontalClicked);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("输入了A");
            PlayUIAnimation(_id, "-1");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("输入了D");
            PlayUIAnimation(_id, "1");
        }
    }

    private void OnPointerHorizontalSlided(GameObject gameObject, object[] commands)
    {
        if (commands != null)
        {
            Vector2 direction = (Vector2)commands[0];
            //Debug.Log("输出了滑动相对位移X为" + direction.x);
            //Debug.Log("输出了滑动相对位移Y为" + direction.y);
            if (direction.x > 2)
            {
                PlayUIAnimation(_id, "1");
                Debug.Log("向右滑动了");
            }
            else if (direction.x < -2)
            {
                PlayUIAnimation(_id, "-1");
                Debug.Log("向左滑动了");
            }
        }
    }
    private void OnPointerHorizontalClicked(GameObject gameObject, object[] commands)
    {
        if (commands != null)
        {
            Vector2 direction = (Vector2)commands[0];
            //Debug.Log("输出了相对位移X为" + direction.x);
            //Debug.Log("输出了相对位移Y为" + direction.y);
            if (Math.Abs(direction.x) < 2)
            {
                Debug.Log("我们认为用户进行的是点击而不是滑动操作");
            }
        }
    }
}
