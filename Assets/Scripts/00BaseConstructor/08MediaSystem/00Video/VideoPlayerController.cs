using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoPlayerController : BaseController
{
    [Header("VideoPlayer�����")]
    public VideoPlayer videoPlayer;
    [Header("��Ƶ�϶�����")]
    public Slider video_Slider;
    [Header("VideoBG_TRF�����")]
    public RectTransform VideoBG_TRF;
    [Header("��Ƶ������ʾ��")]
    public Image pause_Img;
    [Header("��Ƶ�رհ�ť��")]
    public Button close_Btn;
    //���̧�����
    bool mouseUp = true;
    bool isPause = false;

    [Header("ȫ����ť��")]
    public Button fullScreen_Btn;
    [Header("ȫ����ť���飺")]
    public Sprite fullScreen_Sprite;
    [Header("��ȫ����ť���飺")]
    public Sprite notFullScreen_Sprite;
    [Header("ȫ����ť��ʾ�ı���")]
    public TMP_Text fullScreen_Txt;
    bool isFullScreen = false;

    [Header("������ť��")]
    public Button mute_Btn;
    [Header("������ť����:")]
    public Sprite mute_Sprite;
    [Header("�Ǿ�����ť���飺")]
    public Sprite notMute_Sprite;
    [Header("������ť��ʾ�ı���")]
    public TMP_Text mute_Txt;
    bool isMute = false;

    [Header("��Ƶʱ����")]
    public TMP_Text videoTime_Txt;
    //��Ƶʱ��
    int clipHour, clipMinute, clipSecond, currentHour, currentMinute, currentSecond;

    void Start()
    {
        close_Btn.onClick.AddListener(OnClickCloseBtn);

        video_Slider.onValueChanged.AddListener(SliderValueChangeEvent);

        fullScreen_Btn.onClick.AddListener(OnClickFullScreenBtn);
        mute_Btn.onClick.AddListener(OnClickMuteBtn);
        videoPlayer.targetTexture.Release();

        isFullScreen = false;
        isMute = false;

        clipMinute = (int)(videoPlayer.clip.length) / 60;
        clipSecond = (int)(videoPlayer.clip.length - clipMinute * 60);
    }

    void OnEnable()
    {
        pause_Img.gameObject.SetActive(false);
    }

    void Update()
    {
        SetVideoTime();
    }

    void FixedUpdate()
    {
        if (mouseUp)
            video_Slider.value = videoPlayer.frame / (videoPlayer.frameCount * 1.0f);

    }
    //������Ƶʱ�� 
    void SetVideoTime()
    {
        currentMinute = (int)(videoPlayer.time) / 60;
        currentSecond = (int)(videoPlayer.time - currentMinute * 60);
        videoTime_Txt.text = string.Format("{0:D2}:{1:D2} / {2:D2}:{3:D2}", currentMinute, currentSecond, clipMinute, clipSecond);
    }

    //�ر���Ƶ���
    void OnClickCloseBtn()
    {
        gameObject.SetActive(false);

        fullScreen_Txt.text = "ȫ ��";
        fullScreen_Btn.GetComponent<Image>().sprite = fullScreen_Sprite;
        VideoBG_TRF.sizeDelta = new Vector2(1511, 950);
        isFullScreen = false;

        videoPlayer.SetDirectAudioMute(0, false);
        mute_Btn.GetComponent<Image>().sprite = notMute_Sprite;
        mute_Txt.text = "�� ��";
        isMute = false;
    }

    //ȫ��
    void OnClickFullScreenBtn()
    {
        if (isFullScreen)
        {
            VideoBG_TRF.sizeDelta = new Vector2(1511, 950);
            fullScreen_Btn.GetComponent<Image>().sprite = fullScreen_Sprite;
            fullScreen_Txt.text = "ȫ ��";
        }
        else
        {
            VideoBG_TRF.sizeDelta = new Vector2(Screen.width, Screen.height);
            fullScreen_Btn.GetComponent<Image>().sprite = notFullScreen_Sprite;
            fullScreen_Txt.text = "ȡ��ȫ��";
        }
        isFullScreen = !isFullScreen;
    }

    //����
    void OnClickMuteBtn()
    {
        if (isMute)
        {
            videoPlayer.SetDirectAudioMute(0, false);
            mute_Btn.GetComponent<Image>().sprite = notMute_Sprite;
            mute_Txt.text = "�� ��";
        }
        else
        {
            videoPlayer.SetDirectAudioMute(0, true);
            mute_Btn.GetComponent<Image>().sprite = mute_Sprite;
            mute_Txt.text = "ȡ������";
        }
        isMute = !isMute;
    }

    #region EventTrigger����¼�
    //��갴��
    public void PointDown()
    {
        videoPlayer.Pause();
        videoPlayer.frame = long.Parse((video_Slider.value * videoPlayer.frameCount).ToString("0."));
        mouseUp = false;
        isPause = false;
    }

    //���̧��
    public void PointUp()
    {
        videoPlayer.Play();
        mouseUp = true;
        pause_Img.gameObject.SetActive(false);
    }

    //�϶���ʼ
    public void PointDragBegin()
    {
        mouseUp = false;
    }
    //�϶�����
    public void PointDragEnd()
    {
        mouseUp = true;
    }

    //������Ƶ��ͣ
    public void SetPauseImg()
    {
        isPause = !isPause;
        pause_Img.gameObject.SetActive(isPause);
        if (!isPause)
            videoPlayer.Play();
        else
            videoPlayer.Pause();
    }
    #endregion

    void SliderValueChangeEvent(float value)
    {
        if (!mouseUp)
        {
            videoPlayer.frame = long.Parse((value * videoPlayer.frameCount).ToString("0."));
        }
    }
}
