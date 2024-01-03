using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerService : BaseService<VideoPlayerService>
{
    Dictionary<string, VideoPlayerController> _videoPlayerDict = new();

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public bool AddVideoPlayer(string videoPlayerId, VideoPlayerConfig config)
    {
        bool res = false;
        config.videoRawImage.texture = config.videoRenderTexture;
        Transform videoPlayerTrans = config.videoRawImage.transform;
        VideoPlayerController controller = videoPlayerTrans.GetOrAddComponent<VideoPlayerController>();
        controller.InitController(config);
        if (!_videoPlayerDict.ContainsKey(videoPlayerId))
        {
            _videoPlayerDict.Add(videoPlayerId, controller);
            res = true;
        }
        return res;
    }

    public bool LoadVideo(string videoPlayerId, string videoPath)
    {
        bool res = false;
        if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
        {
            res = controller.LoadVideo(videoPath);
        }
        return res;
    }

    public bool LoadVideo(string videoPlayerId, VideoClip videoClip)
    {
        bool res = false;
        if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
        {
            res = controller.LoadVideo(videoClip);
        }
        return res;
    }

    public bool PlayVideo(string videoPlayerId)
    {
        bool res = false;
        if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
        {
            res = controller.PlayVideo();
        }
        return res;
    }

    public bool PauseVideo(string videoPlayerId)
    {
        bool res = false;
        if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
        {
            res = controller.PauseVideo();
        }
        return res;
    }

    public bool StopVideo(string videoPlayerId)
    {
        bool res = false;
        if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
        {
            res = controller.StopVideo();
        }
        return res;
    }
}

public class VideoPlayerConfig : BaseConfig
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

    public Action<bool> OnPlayOrPauseCallBack;
    public Action<bool> OnFullOrNormalScreenCallBack;
    public Action<bool> OnMuteOrUnMuteCallBack;
}
