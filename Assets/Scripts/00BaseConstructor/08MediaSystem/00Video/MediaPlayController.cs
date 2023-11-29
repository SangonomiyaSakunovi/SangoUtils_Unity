using RenderHeads.Media.AVProVideo;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MediaPlayController : MonoBehaviour
{
    private GameObject _mediaPos;
    private Texture2D _mediaPosTexture;
    private Slider _mediaPlayProgressSlider;
    private Button _mediaPlayOrPauseButton;
    private Action<bool> _hasMediaTrunToPlayCallBack;

    private MediaPlayer _mediaPlayer;
    private DisplayUGUI _displayUGUI;
    private EventTrigger _videoProgressSliderEventTrigger;

    private float _currentVideoDuration = 0;
    private bool _isPlayingMedia = false;

    private MediaPlayer.FileLocation _defaultLocation = MediaPlayer.FileLocation.AbsolutePathOrURL;

    private void Update()
    {
        if (!_isPlayingMedia) return;
        if (_currentVideoDuration == 0 && _isPlayingMedia)
        {
            _currentVideoDuration = _mediaPlayer.Info.GetDurationMs();
        }
        else if (_isPlayingMedia)
        {
            OnSliderUpdating();
        }
    }

    public void InitComponent(MediaPlayer.FileLocation fileLocation, GameObject mediaPos, Texture2D mideaTexture, Slider contorlSlider, Button controlButton, Action<bool> controlCB)
    {
        _defaultLocation = fileLocation;

        _mediaPos = mediaPos;
        _mediaPosTexture = mideaTexture;
        _mediaPlayProgressSlider = contorlSlider;

        _mediaPlayOrPauseButton = controlButton;
        _hasMediaTrunToPlayCallBack = controlCB;

        _mediaPlayer = _mediaPos.AddComponent<MediaPlayer>();
        _displayUGUI = _mediaPos.AddComponent<DisplayUGUI>();
        _videoProgressSliderEventTrigger = _mediaPos.AddComponent<EventTrigger>();
        InitMediaPlayer();
        InitListener();
    }

    public void OpenMediaFromFile(MediaPlayer.FileLocation fileLocation, string path, Action faildToOpenVideoCallBack)
    {        
        bool res = _mediaPlayer.OpenVideoFromFile(fileLocation, path, false);
        if (res)
        {
            _mediaPlayer.Play();
            _isPlayingMedia = true;
        }
        else
        {
            faildToOpenVideoCallBack?.Invoke();
        }
    }

    public void ResetMedia()
    {
        _mediaPlayer.CloseVideo();
        _mediaPlayProgressSlider.value = 0;
        _isPlayingMedia = false;
    }

    private void InitMediaPlayer()
    {
        _mediaPlayer.m_VideoLocation = _defaultLocation;
        _mediaPlayer.m_Volume = 1;
        _mediaPlayer.m_Loop = true;

        _displayUGUI._defaultTexture = _mediaPosTexture;
        _displayUGUI._scaleMode = ScaleMode.StretchToFill;
        _displayUGUI._mediaPlayer = _mediaPlayer;
        _displayUGUI._noDefaultDisplay = true;
    }

    private void InitListener()
    {
        if (_mediaPlayProgressSlider != null)
        {
            _mediaPlayProgressSlider.onValueChanged.AddListener(OnSliderValueChanged);
            BindEventTriger(_videoProgressSliderEventTrigger, EventTriggerType.BeginDrag, OnSliderDragBegin);
            BindEventTriger(_videoProgressSliderEventTrigger, EventTriggerType.EndDrag, OnSliderDragEnd);
        }
        if (_mediaPlayOrPauseButton != null)
        {
            _mediaPlayOrPauseButton.onClick.AddListener(OnVideoPlayButtonClicked);
        }

    }

    private void OnVideoPlayButtonClicked()
    {
        if (_isPlayingMedia)
        {
            _mediaPlayer.Control.Pause();
            _isPlayingMedia = false;
            _hasMediaTrunToPlayCallBack?.Invoke(false);
        }
        else
        {
            _mediaPlayer.Control.Play();
            _isPlayingMedia = true;
            _hasMediaTrunToPlayCallBack?.Invoke(true);
        }
    }

    private void OnSliderValueChanged(float value)
    {
        _mediaPlayer.Control.SeekFast(value * _currentVideoDuration);
    }

    private void OnSliderUpdating()
    {
        float nowTime = _mediaPlayer.Control.GetCurrentTimeMs();
        float progress = nowTime / _currentVideoDuration;
        _mediaPlayProgressSlider.value = progress;
    }

    private void OnSliderDragBegin(BaseEventData baseEventData)
    {
        _mediaPlayer.Control.Pause();
    }

    private void OnSliderDragEnd(BaseEventData baseEventData)
    {
        _mediaPlayer.Control.Play();
    }

    private void BindEventTriger(EventTrigger eventTrigger, EventTriggerType eventTriggerType, UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = null;
        foreach (var existedEntry in eventTrigger.triggers)
        {
            if (existedEntry.eventID == eventTriggerType)
            {
                entry = existedEntry;
                break;
            }
        }

        if (entry == null)
        {
            entry = new EventTrigger.Entry();
            entry.eventID = eventTriggerType;
            entry.callback = new EventTrigger.TriggerEvent();
        }

        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }
}
