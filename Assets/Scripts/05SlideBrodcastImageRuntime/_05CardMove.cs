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
            Debug.Log("������A");
            PlayUIAnimation(_id, "-1");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("������D");
            PlayUIAnimation(_id, "1");
        }
    }

    private void OnPointerHorizontalSlided(GameObject gameObject, object[] commands)
    {
        if (commands != null)
        {
            Vector2 direction = (Vector2)commands[0];
            //Debug.Log("����˻������λ��XΪ" + direction.x);
            //Debug.Log("����˻������λ��YΪ" + direction.y);
            if (direction.x > 2)
            {
                PlayUIAnimation(_id, "1");
                Debug.Log("���һ�����");
            }
            else if (direction.x < -2)
            {
                PlayUIAnimation(_id, "-1");
                Debug.Log("���󻬶���");
            }
        }
    }
    private void OnPointerHorizontalClicked(GameObject gameObject, object[] commands)
    {
        if (commands != null)
        {
            Vector2 direction = (Vector2)commands[0];
            //Debug.Log("��������λ��XΪ" + direction.x);
            //Debug.Log("��������λ��YΪ" + direction.y);
            if (Math.Abs(direction.x) < 2)
            {
                Debug.Log("������Ϊ�û����е��ǵ�������ǻ�������");
            }
        }
    }
}
