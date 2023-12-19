using UnityEngine;
using UnityEngine.UI;

public class _06Window : BaseWindow
{
    public GameObject _mediaPos;
    public Slider _slider;
    public Button _button;

    private void Awake()
    {

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            //SetMediaPlayControllerContentFromFile("SangoTest", RenderHeads.Media.AVProVideo.MediaPlayer.FileLocation.AbsolutePathOrURL,"http://");

        }
        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }
}
