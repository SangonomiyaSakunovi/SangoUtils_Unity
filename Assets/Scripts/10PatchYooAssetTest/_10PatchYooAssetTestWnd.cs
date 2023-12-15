using UnityEngine;

public class _10PatchYooAssetTestWnd : MonoBehaviour
{
    public AudioSource _audioSource;
    private AudioClip _bgm;

    private string audioString = "Assets/Res/Remote/Audio/GetReadyかわいい";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _bgm = null;
            _audioSource.clip = null;
            AssetService.Instance.LoadAudioClipASync(audioString, OnBgmLoaded, false);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            _bgm = null;
        }
    }

    private void OnBgmLoaded(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
