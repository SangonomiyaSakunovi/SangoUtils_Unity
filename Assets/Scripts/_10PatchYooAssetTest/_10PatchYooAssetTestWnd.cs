using DG.Tweening;
using UnityEngine;

public class _10PatchYooAssetTestWnd : MonoBehaviour
{
    public AudioSource _audioSource;

    private string audioString = "Assets/Res/Remote/Audio/GetReadyかわいい.mp3";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _audioSource.clip = null;
            AssetService.Instance.LoadAudioClipASync(audioString, OnBgmLoaded, false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }

    private void OnBgmLoaded(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
