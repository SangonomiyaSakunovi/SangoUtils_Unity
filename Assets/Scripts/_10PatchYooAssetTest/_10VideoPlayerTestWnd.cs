using SangoUtils_Bases_UnityEngine;
using SangoUtils_Extensions_UnityEngine.Service;
using SangoUtils_Extensions_UnityEngine.Utils;
using SangoUtils_Logger;
using SangoUtils_Patch_YooAsset;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class _10VideoPlayerTestWnd : BaseWindow
{
    public RawImage videoRawImage;
    public RenderTexture videoRenderTexture;
    public Button playOrPauseBtn;
    public Button fullScreenBtn;
    public Button muteBtn;
    public Slider videoProgressSlider;
    public Slider audioVolumeSlider;
    public TMP_Text videoFullTimeTMPText;
    public TMP_Text videoCurrentTimeTMPText;

    [Header("TestButtons")]
    public Button btn1;
    public Button btn2;
    public Button btn3;

    public float defaultAudioVolume;
    public Vector2 normalScreenRectTransValue;
    public Vector2 fullScreenRectTransValue;

    private VideoClip _videoClip;

    private const string _videoString1 = "Assets/Res/Remote/Video/骄阳视觉宣传视频001.MP4";
    private const string _videoString2 = "Assets/Res/Remote/Video/深港科创区域片沙盘0811无标.mp4";
    private const string _videoString3 = "Assets/Res/Remote/Video/深港无标8-11B.mp4";
    private const string _videoId = "SangoTestId";

    private VideoPlayerConfig videoPlayerConfig;

    private void Start()
    {
        videoPlayerConfig = new VideoPlayerConfig(videoRawImage,videoRenderTexture);
        videoPlayerConfig.PlayOrPauseBtn = playOrPauseBtn;
        videoPlayerConfig.FullScreenBtn = fullScreenBtn;
        videoPlayerConfig.MuteBtn = muteBtn;
        videoPlayerConfig.VideoProgressSlider = videoProgressSlider;
        videoPlayerConfig.AudioVolumeSlider = audioVolumeSlider;
        videoPlayerConfig.VideoFullTimeTMPText = videoFullTimeTMPText;
        videoPlayerConfig.VideoCurrentTimeTMPText = videoCurrentTimeTMPText;
        videoPlayerConfig.DefaultAudioVolume = defaultAudioVolume;
        videoPlayerConfig.NormalScreenRectTransValue = normalScreenRectTransValue;
        videoPlayerConfig.FullScreenRectTransValue = fullScreenRectTransValue;
        videoPlayerConfig.OnPlayOrPauseCallBack = OnPlayOrPauseBtnClickedCallBack;        

        VideoPlayerService.Instance.AddVideoPlayer(_videoId, videoPlayerConfig);

        btn1.onClick.AddListener(OnButton1Clicked);
        btn2.onClick.AddListener(OnButton2Clicked);
        btn3.onClick.AddListener(OnButton3Clicked);
    }

    private void Update()
    {        
        if (Input.GetKeyDown(KeyCode.S))
        {
            VideoPlayerService.Instance.PlayVideo(_videoId);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            VideoPlayerService.Instance.StopVideo(_videoId);
        }
    }

    private void OnButton1Clicked()
    {
        _videoClip = null;
        AssetService.Instance.LoadVideoClipASync(_videoString1, OnVideoClipLoaded, false);
    }
    private void OnButton2Clicked()
    {
        _videoClip = null;
        AssetService.Instance.LoadVideoClipASync(_videoString2, OnVideoClipLoaded, false);
    }
    private void OnButton3Clicked()
    {
        _videoClip = null;
        AssetService.Instance.LoadVideoClipASync(_videoString3, OnVideoClipLoaded, false);
    }

    private void OnVideoClipLoaded(VideoClip videoClip)
    {
        _videoClip = videoClip;
        VideoPlayerService.Instance.LoadVideo(_videoId, videoClip);
        SangoLogger.Done("新视频文件已加载");
    }

    private void OnPlayOrPauseBtnClickedCallBack(bool isPlay)
    {
        if (playOrPauseBtn != null)
        {
            if (isPlay)
            {
                playOrPauseBtn.GetComponentInChildren<TMP_Text>().text = "Pause";
            }
            else
            {
                playOrPauseBtn.GetComponentInChildren<TMP_Text>().text = "Play";
            }
        }
    }

    protected override void OnInit()
    {

    }

    protected override void OnDispose()
    {

    }

    public override void OnAwake()
    {
        WindowLayer = WindowLayer.Base;
        AddWindow(this);
    }
}
