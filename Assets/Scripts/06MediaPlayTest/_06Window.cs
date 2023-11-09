using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class _06Window : BaseWindow
{
    public GameObject _mediaPos;
    public Slider _slider;
    public Button _button;

    private void Awake()
    {
        AddMediaPlayController("SangoTest", MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, _mediaPos, null, _slider, _button);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            //SetMediaPlayControllerContentFromFile("SangoTest", RenderHeads.Media.AVProVideo.MediaPlayer.FileLocation.AbsolutePathOrURL,"http://");
            SetMediaPlayControllerContentFromFile("SangoTest", MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "AVProVideoSamples/∑…ÃÏ_1080P_.mp4");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            ResetMediaPlayController("SangoTest");
        }
    }
}
