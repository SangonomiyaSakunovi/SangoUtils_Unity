using UnityEngine;

public class _05CardMove : BaseWindow
{
    public Transform _parentTrans;
    public float _duration = 0.2f;

    public string _id = "sangoTest";

    private void Start()
    {
        SangoUIBaseAnimation animation = SangoUIAnimationAssets.SangoUISlideBroadCastImageAnimation(_parentTrans, _duration);
        AddUIAnimation(_id, animation);
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
}
