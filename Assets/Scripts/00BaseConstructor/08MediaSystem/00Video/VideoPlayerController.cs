using UnityEngine.Video;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoPlayerController : BaseController
{
    [Header("VideoPlayer组件：")]
    public VideoPlayer videoPlayer;
    [Header("视频拖动条：")]
    public Slider video_Slider;
    [Header("VideoBG_TRF组件：")]
    public RectTransform VideoBG_TRF;
    [Header("视频播放提示：")]
    public Image pause_Img;
    [Header("视频关闭按钮：")]
    public Button close_Btn;
    //鼠标抬起或按下
    bool mouseUp = true;
    bool isPause = false;

    [Header("全屏按钮：")]
    public Button fullScreen_Btn;
    [Header("全屏按钮精灵：")]
    public Sprite fullScreen_Sprite;
    [Header("非全屏按钮精灵：")]
    public Sprite notFullScreen_Sprite;
    [Header("全屏按钮提示文本：")]
    public TMP_Text fullScreen_Txt;
    bool isFullScreen = false;

    [Header("静音按钮：")]
    public Button mute_Btn;
    [Header("静音按钮精灵:")]
    public Sprite mute_Sprite;
    [Header("非静音按钮精灵：")]
    public Sprite notMute_Sprite;
    [Header("静音按钮提示文本：")]
    public TMP_Text mute_Txt;
    bool isMute = false;

    [Header("视频时长：")]
    public TMP_Text videoTime_Txt;
    //视频时长
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
    //设置视频时间 
    void SetVideoTime()
    {
        currentMinute = (int)(videoPlayer.time) / 60;
        currentSecond = (int)(videoPlayer.time - currentMinute * 60);
        videoTime_Txt.text = string.Format("{0:D2}:{1:D2} / {2:D2}:{3:D2}", currentMinute, currentSecond, clipMinute, clipSecond);
    }

    //关闭视频面板
    void OnClickCloseBtn()
    {
        gameObject.SetActive(false);

        fullScreen_Txt.text = "全 屏";
        fullScreen_Btn.GetComponent<Image>().sprite = fullScreen_Sprite;
        VideoBG_TRF.sizeDelta = new Vector2(1511, 950);
        isFullScreen = false;

        videoPlayer.SetDirectAudioMute(0, false);
        mute_Btn.GetComponent<Image>().sprite = notMute_Sprite;
        mute_Txt.text = "静 音";
        isMute = false;
    }

    //全屏
    void OnClickFullScreenBtn()
    {
        if (isFullScreen)
        {
            VideoBG_TRF.sizeDelta = new Vector2(1511, 950);
            fullScreen_Btn.GetComponent<Image>().sprite = fullScreen_Sprite;
            fullScreen_Txt.text = "全 屏";
        }
        else
        {
            VideoBG_TRF.sizeDelta = new Vector2(Screen.width, Screen.height);
            fullScreen_Btn.GetComponent<Image>().sprite = notFullScreen_Sprite;
            fullScreen_Txt.text = "取消全屏";
        }
        isFullScreen = !isFullScreen;
    }

    //静音
    void OnClickMuteBtn()
    {
        if (isMute)
        {
            videoPlayer.SetDirectAudioMute(0, false);
            mute_Btn.GetComponent<Image>().sprite = notMute_Sprite;
            mute_Txt.text = "静 音";
        }
        else
        {
            videoPlayer.SetDirectAudioMute(0, true);
            mute_Btn.GetComponent<Image>().sprite = mute_Sprite;
            mute_Txt.text = "取消静音";
        }
        isMute = !isMute;
    }

    #region EventTrigger相关事件
    //鼠标按下
    public void PointDown()
    {
        videoPlayer.Pause();
        videoPlayer.frame = long.Parse((video_Slider.value * videoPlayer.frameCount).ToString("0."));
        mouseUp = false;
        isPause = false;
    }

    //鼠标抬起
    public void PointUp()
    {
        videoPlayer.Play();
        mouseUp = true;
        pause_Img.gameObject.SetActive(false);
    }

    //拖动开始
    public void PointDragBegin()
    {
        mouseUp = false;
    }
    //拖动结束
    public void PointDragEnd()
    {
        mouseUp = true;
    }

    //设置视频暂停
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
