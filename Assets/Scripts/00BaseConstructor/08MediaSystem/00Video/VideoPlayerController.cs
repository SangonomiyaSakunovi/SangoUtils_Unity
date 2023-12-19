using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : BaseController
{
    private VideoPlayer _videoPlayer;
    private AudioSource _audioSource;
    private RawImage _videoRawImage;
    private RectTransform _videoRawImageRectTrans;
    private RenderTexture _videoRenderTexture;

    private string _videoPath = string.Empty;
    private VideoClip _videoClip = null;

    private Button _playOrPauseBtn;
    private Button _fullScreenBtn;
    private Button _muteBtn;

    private Slider _videoProgressSlider;
    private Slider _audioVolumeSlider;
    private TMP_Text _videoFullTimeTMPText;
    private TMP_Text _videoCurrentTimeTMPText;

    private float _defaultAudioVolume;
    private Vector2 _normalScreenRectTransValue;
    private Vector2 _fullScreenRectTransValue;

    private bool _isGetVideoInfo = false;
    private bool _isFullScreen = false;
    private bool _isMouseUp = true;

    private bool _isUpdatingProgress = false;
    private bool _isForcePlaying = false;

    private Action<bool> OnPlayOrPauseCallBack;
    private Action<bool> OnFullOrNormalScreenCallBack;
    private Action<bool> OnMuteOrUnMuteCallBack;

    private EventTrigger _videoProgressSliderEventTrigger;

    private int _tempHour, _tempMin;
    private float _time;
    private float _timeCount;
    private float _timeCurrent;

    public void InitController(VideoPlayerConfig config)
    {
        _videoRawImage = config.videoRawImage;
        _videoRenderTexture = config.videoRenderTexture;

        _playOrPauseBtn = config.playOrPauseBtn;
        _fullScreenBtn = config.fullScreenBtn;
        _muteBtn = config.muteBtn;

        _videoProgressSlider = config.videoProgressSlider;
        _audioVolumeSlider = config.audioVolumeSlider;

        _videoFullTimeTMPText = config.videoFullTimeTMPText;
        _videoCurrentTimeTMPText = config.videoCurrentTimeTMPText;

        _defaultAudioVolume = config.defaultAudioVolume;
        _normalScreenRectTransValue = config.normalScreenRectTransValue;
        _fullScreenRectTransValue = config.fullScreenRectTransValue;

        OnPlayOrPauseCallBack = config.OnPlayOrPauseCallBack;
        OnFullOrNormalScreenCallBack = config.OnFullOrNormalScreenCallBack;
        OnMuteOrUnMuteCallBack = config.OnMuteOrUnMuteCallBack;

        _videoRawImageRectTrans = _videoRawImage.GetComponent<RectTransform>();

        _audioSource = transform.AddComponent<AudioSource>();
        _videoPlayer = transform.AddComponent<VideoPlayer>();
        _videoPlayer.targetTexture = _videoRenderTexture;

        _videoPlayer.playOnAwake = false;
        _videoPlayer.isLooping = true;
        _audioSource.playOnAwake = false;
        _audioSource.volume = _defaultAudioVolume;

        AddEvent();
        ResetController();
    }

    private void ResetController()
    {
        _videoRenderTexture.Release();
        _isGetVideoInfo = false;
        _isFullScreen = false;
        _isMouseUp = true;
    }

    private void AddEvent()
    {
        _playOrPauseBtn?.onClick.AddListener(OnPlayOrPauseBtnClicked);
        _fullScreenBtn?.onClick.AddListener(OnFullScreenBtnClicked);
        _muteBtn?.onClick.AddListener(OnMuteBtnClicked);
        _audioVolumeSlider?.onValueChanged.AddListener(OnAudioVolumeSliderValueChanged);
        if (_videoProgressSlider != null)
        {
            _videoProgressSlider.onValueChanged.AddListener(OnVideoProgressSliderValueChanged);
            SetGameObjectDragBeginListener(_videoProgressSlider, OnSliderDragBegin);
            SetGameObjectDragEndListener(_videoProgressSlider, OnSliderDragEnd);
        }
    }

    protected override void OnUpdate()
    {
        if (_isUpdatingProgress)
        {
            if (!_isGetVideoInfo && _videoPlayer.clip != null)
            {
                _time = (float)_videoPlayer.clip.length;
                _tempHour = (int)_time / 60;
                _tempMin = (int)_time % 60;
                if (_videoFullTimeTMPText != null)
                {
                    _videoFullTimeTMPText.text = string.Format("{0:D2}:{1:D2}", _tempHour.ToString(), _tempMin.ToString());
                }
                _isGetVideoInfo = true;
            }

            if (_videoProgressSlider != null)
            {
                _time = (float)_videoPlayer.time;
                _tempHour = (int)_time / 60;
                _tempMin = (int)_time % 60;
                if (_videoCurrentTimeTMPText != null)
                {
                    _videoCurrentTimeTMPText.text = string.Format("{0:D2}:{1:D2}", _tempHour.ToString(), _tempMin.ToString());
                }
                if (_videoPlayer.isPlaying && _isMouseUp)
                {
                    _videoProgressSlider.value = (_videoPlayer.frame * 1.0f / (long)_videoPlayer.frameCount);
                }
            }
        }
    }

    public bool LoadVideo(string videoPath)
    {
        bool res = false;
        if (!string.IsNullOrEmpty(videoPath))
        {
            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = "file:///" + videoPath;
            _videoPath = videoPath;
            if (_audioVolumeSlider != null)
            {
                _audioVolumeSlider.value = _defaultAudioVolume;
            }
            _isUpdatingProgress = true;
            res = true;
        }
        return res;
    }

    public bool LoadVideo(VideoClip videoClip)
    {
        bool res = false;
        if (videoClip != null)
        {
            _videoClip = videoClip;
            _videoPlayer.clip = _videoClip;
            if (_audioVolumeSlider != null)
            {
                _audioVolumeSlider.value = _defaultAudioVolume;
            }
            _isUpdatingProgress = true;
            res = true;
        }
        return res;
    }

    public bool PlayVideo()
    {
        bool res = false;
        if (!string.IsNullOrEmpty(_videoPath) || _videoClip != null)
        {

            _videoPlayer.Play();
            res = true;
        }
        return res;
    }

    public bool PauseVideo()
    {
        bool res = false;
        if (!string.IsNullOrWhiteSpace(_videoPath) || _videoClip != null)
        {
            _videoPlayer.Pause();
            res = true;
        }
        return res;
    }

    public bool StopVideo()
    {
        bool res = false;
        if (!string.IsNullOrWhiteSpace(_videoPath))
        {
            _videoPlayer.Stop();
            _isUpdatingProgress = false;
            res = true;
        }
        return res;
    }

    private void OnMouseEnter()
    {
        ShowVideoExternInfo(true);
    }

    private void OnMouseExit()
    {
        ShowVideoExternInfo(false);
    }

    private void ShowVideoExternInfo(bool isShow = false)
    {
        if (_audioVolumeSlider != null && _videoProgressSlider != null && _videoFullTimeTMPText != null && _videoCurrentTimeTMPText != null)
        {
            _audioVolumeSlider.gameObject.SetActive(isShow);
            _videoProgressSlider.gameObject.SetActive(isShow);
            _videoFullTimeTMPText.gameObject.SetActive(isShow);
            _videoCurrentTimeTMPText.gameObject.SetActive(isShow);
        }
    }

    private void OnPlayOrPauseBtnClicked()
    {
        if (_videoPlayer.isPlaying)
        {
            PauseVideo();
            _isForcePlaying = false;
        }
        else
        {
            PlayVideo();
            _isForcePlaying = true;
        }
        OnPlayOrPauseCallBack?.Invoke(_videoPlayer.isPlaying);
    }

    private void OnProgressSliderDragBeginOrEnd()
    {
        if (!_isForcePlaying) return;
        if (_videoPlayer.isPlaying)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }

    private void OnFullScreenBtnClicked()
    {
        if (_isFullScreen)
        {
            _videoRawImageRectTrans.sizeDelta = _normalScreenRectTransValue;
        }
        else
        {
            _videoRawImageRectTrans.sizeDelta = _fullScreenRectTransValue;
        }
        _isFullScreen = !_isFullScreen;
        OnFullOrNormalScreenCallBack?.Invoke(_isFullScreen);
    }

    private void OnMuteBtnClicked()
    {
        _audioSource.mute = !_audioSource.mute;
        OnMuteOrUnMuteCallBack?.Invoke(_audioSource.mute);
    }

    private void OnAudioVolumeSliderValueChanged(float value)
    {
        _audioSource.volume = value;
    }

    private void OnVideoProgressSliderValueChanged(float value)
    {
        if (!_isMouseUp)
        {
            _videoPlayer.frame = (long)(value * _videoPlayer.frameCount);
        }
    }

    private void OnSliderDragBegin(BaseEventData eventData)
    {
        _isMouseUp = false;
        OnProgressSliderDragBeginOrEnd();
    }

    private void OnSliderDragEnd(BaseEventData eventData)
    {
        _isMouseUp = true;
        OnProgressSliderDragBeginOrEnd();
    }
}
