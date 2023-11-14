using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _07Window : MonoBehaviour
{
    private int _currentIndex = 0;
    private List<Texture> _textureLists = null;
    private string _path = Application.streamingAssetsPath + "/StreamingSprites";

    public RawImage _rawImage;

    void Start()
    {
        _textureLists = TextureUtils.LoadTextureFolder(_path, 1600, 1050);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ChangeImage();
        }
    }

    private void ChangeImage()
    {
        if (_textureLists != null)
        {
            Debug.Log("�ҵ�ͼƬ������Ϊ"+ _textureLists.Count);
            if (_textureLists.Count == 0) 
            {
                return;
            }
            _rawImage.texture = _textureLists[_currentIndex];
            if (_currentIndex < _textureLists.Count - 1)
            {
                _currentIndex++;
            }
            else
            {
                _currentIndex = 0;
            }
        }
        else
        {
            Debug.Log("û���ҵ��κ�ͼƬ");
        }
    }
}
