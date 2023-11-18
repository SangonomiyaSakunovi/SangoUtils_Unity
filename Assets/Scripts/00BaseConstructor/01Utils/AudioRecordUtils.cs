using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class AudioRecordUtils : MonoBehaviour
{
    private float _maxRecordTime;
    private string _outPutPath;
    private string _fileNamePrefix;
    private int _recordFrequnce;

    private AudioSource _audioSource;
    private AudioClip _audioClip;

    private float _currentRecordingTimemer = 0;
    private byte[] _currentRecordedAudioData;
    private string _currentWavFileBase64Str;

    private bool _isRecording = false;
    private Action _currentStopCallBack = null;

    private void Start()
    {
        float maxRecordTime = 10;
        string outPutPath = "I:\\SangoOutputs\\SZY\\AudioRecords";
        string fileNamePrefix = "AudioData";
        int recordFrequnce = 16000;
        InitAudioRecord(maxRecordTime, recordFrequnce, outPutPath, fileNamePrefix);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording(null);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }
    }

    public void InitAudioRecord(float maxRecordTime, int recordFrequnce, string outPutPath, string fileNamePrefix)
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = this.gameObject.AddComponent<AudioSource>();
        }
        _audioClip = _audioSource.clip;
        _maxRecordTime = maxRecordTime;
        _recordFrequnce = recordFrequnce;
        _outPutPath = outPutPath;
        _fileNamePrefix = fileNamePrefix;
    }

    public void StartRecording(Action stopCallBack)
    {
        if (!_isRecording)
        {
            _isRecording = true;
            _currentStopCallBack = stopCallBack;
            StartCoroutine("KeepTime");
            _audioClip = Microphone.Start(null, false, (int)Math.Ceiling(_maxRecordTime), _recordFrequnce);
        }
    }

    public void StopRecording()
    {
        _currentRecordedAudioData = GetRealAudio(ref _audioClip);
        Microphone.End("Built-in Microphone");
        StopCoroutine("KeepTime");
        Debug.Log("Over");
        _audioSource.clip = _audioClip;
        _audioSource.Play();
        SaveRecordedAudio();
        if (_currentStopCallBack != null)
        {
            _currentStopCallBack();
        }
        _isRecording = false;
    }

    private IEnumerator KeepTime()
    {
        for (_currentRecordingTimemer = _maxRecordTime; _currentRecordingTimemer >= 0; _currentRecordingTimemer -= Time.deltaTime)
        {
            if (_currentRecordingTimemer <= 10)
            {
                if (_currentRecordingTimemer < 1)
                {
                    StopRecording();
                }
            }
            yield return 0;
        }
    }


    private void SaveRecordedAudio()
    {
        if (!Microphone.IsRecording("Built-in Microphone"))
        {
            string wavFileName = _fileNamePrefix + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".wav";
            string txtFileName = _fileNamePrefix + DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss") + ".txt";

            string wavPath = Path.Combine(_outPutPath, wavFileName);
            string txtPath = Path.Combine(_outPutPath, txtFileName);

            Debug.Log("保存文件路径" + _outPutPath);

            if (!Directory.Exists(_outPutPath))
            {
                Directory.CreateDirectory(_outPutPath);
            }
            FileStream fs = CreateEmpty(wavPath);
            fs.Write(_currentRecordedAudioData, 0, _currentRecordedAudioData.Length);
            WriteWavHeader(fs, _audioSource.clip);
            fs.Close();

            ConvertWavToBase64(wavPath);
            SaveWavBase64Txt(txtPath);
        }
    }

    private byte[] GetRealAudio(ref AudioClip recordedClip)
    {
        int position = Microphone.GetPosition(null);
        if (position <= 0 || position > recordedClip.samples)
        {
            position = recordedClip.samples;
        }
        float[] soundata = new float[position * recordedClip.channels];
        recordedClip.GetData(soundata, 0);
        recordedClip = AudioClip.Create(recordedClip.name, position,
        recordedClip.channels, recordedClip.frequency, false);
        recordedClip.SetData(soundata, 0);
        int rescaleFactor = 32767;
        byte[] outData = new byte[soundata.Length * 2];
        for (int i = 0; i < soundata.Length; i++)
        {
            short temshort = (short)(soundata[i] * rescaleFactor);
            byte[] temdata = BitConverter.GetBytes(temshort);
            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        return outData;
    }

    private void WriteWavHeader(FileStream stream, AudioClip clip)
    {
        int hz = clip.frequency;

        int channels = clip.channels;
        int samples = clip.samples;

        stream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        UInt16 one = 1;
        UInt16 two = 2;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        stream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        stream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2);
        stream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        stream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        stream.Write(subChunk2, 0, 4);
    }

    private FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) //为wav文件头留出空间
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    private void ConvertWavToBase64(string wavFilepath)
    {
        FileStream fsForRead = new FileStream(wavFilepath, FileMode.Open);
        try
        {
            fsForRead.Seek(0, SeekOrigin.Begin);
            byte[] bs = new byte[fsForRead.Length];
            int log = Convert.ToInt32(fsForRead.Length);
            fsForRead.Read(bs, 0, log);
            _currentWavFileBase64Str = Convert.ToBase64String(bs);
            Debug.Log("base64编码：" + _currentWavFileBase64Str);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            fsForRead.Close();
        }
    }

    private void SaveWavBase64Txt(string path)
    {
        if (!File.Exists(path))
        {
            File.WriteAllText(path, _currentWavFileBase64Str);
        }
    }
}
