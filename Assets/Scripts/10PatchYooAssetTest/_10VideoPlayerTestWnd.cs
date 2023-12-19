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
        videoPlayerConfig = new VideoPlayerConfig
        {
            videoRawImage = videoRawImage,
            videoRenderTexture = videoRenderTexture,
            playOrPauseBtn = playOrPauseBtn,
            fullScreenBtn = fullScreenBtn,
            muteBtn = muteBtn,
            videoProgressSlider = videoProgressSlider,
            audioVolumeSlider = audioVolumeSlider,
            videoFullTimeTMPText = videoFullTimeTMPText,
            videoCurrentTimeTMPText = videoCurrentTimeTMPText,
            defaultAudioVolume = defaultAudioVolume,
            normalScreenRectTransValue = normalScreenRectTransValue,
            fullScreenRectTransValue = fullScreenRectTransValue,
            OnPlayOrPauseCallBack = OnPlayOrPauseBtnClickedCallBack
        };

        VideoPlayerService.Instance.OnInit();
        VideoPlayerService.Instance.AddVideoPlayer(_videoId, videoPlayerConfig);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _videoClip = null;
            AssetService.Instance.LoadVideoClipASync(_videoString1, OnVideoClipLoaded, false);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _videoClip = null;
            AssetService.Instance.LoadVideoClipASync(_videoString2, OnVideoClipLoaded, false);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _videoClip = null;
            AssetService.Instance.LoadVideoClipASync(_videoString3, OnVideoClipLoaded, false);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            VideoPlayerService.Instance.PlayVideo(_videoId);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            VideoPlayerService.Instance.StopVideo(_videoId);
        }
    }

    private void OnVideoClipLoaded(VideoClip videoClip)
    {
        _videoClip = videoClip;
        VideoPlayerService.Instance.LoadVideo(_videoId, videoClip);
        SangoLogger.Log("新视频文件已加载");
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
}
