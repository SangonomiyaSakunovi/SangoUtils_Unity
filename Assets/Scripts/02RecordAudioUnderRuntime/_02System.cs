using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class _02System : BaseSystem
{
    public static _02System Instance = null;
    private Dictionary<int, AudioClip> _audioClips = null;
    private _02Window _02Window = null;
    
    private string _microphoneDevice = string.Empty;
    private int frequency = 44100;
    private int duration = 5;

    private AudioSource _audioSource;
    private AudioClip _currentRecordClip;

    private bool _isRecording = false;
    
    private void Start()
    {
        InitSystem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_isRecording)
        {
            StartRecording();
        }
        else if (Input.GetKeyDown(KeyCode.S) && _isRecording)
        {
            StopRecording();
        }
    }

    public override void InitSystem()
    {
        Instance = this;
        _02Window = GetComponent<_02Window>();
        _audioSource = GetComponent<AudioSource>();
        _microphoneDevice = Microphone.devices[0];
        _audioClips = new Dictionary<int, AudioClip>();
    }

    public void StartRecording()
    {
        _isRecording = true;
        _audioSource.clip = Microphone.Start(null, false, duration, frequency);
        Invoke("StopRecording", duration);
    }

    private void StopRecording()
    {
        Microphone.End(null);
        //TODO Save Wav
        _currentRecordClip = _audioSource.clip;
        var samples = new float[_currentRecordClip.samples];

        _currentRecordClip.GetData(samples, 0);
        byte[] buffer = new byte[samples.Length*2];

        Buffer.BlockCopy(samples, 0, buffer, 0, buffer.Length);
        File.WriteAllBytes(Application.streamingAssetsPath + "/TestSound.wav", buffer);

        _isRecording= false;

        _audioSource.Play();
    }
}

public class _02ClassInfo
{
    public _02ClassInfo(int id, string description, AudioClip audioClip)
    {
        Id = id;
        Description = description;
        AudioClip = audioClip;
    }

    public int Id { get; private set; }
    public string Description { get; private set; }
    public AudioClip AudioClip { get; private set; }
}

