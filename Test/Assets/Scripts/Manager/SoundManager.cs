using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct ClipInfo
{
    public string key;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private List<ClipInfo> _clipInfos = new List<ClipInfo>();

    [Header("3D Sound Option")]
    [Range(0f, 1f)]
    private float _volume = 1.0f;
    private int _audioPoolSize = 30;
    private int _soundDistanceMin = 0;
    private int _soundDistanceMax = 100;

    private AudioSource _audioSource;
    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private List<GameObject> _audioObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        foreach(ClipInfo clipInfo in _clipInfos)
        {
            clips.Add(clipInfo.key, clipInfo.clip);
        }

        _audioSource = GetComponent<AudioSource>();

        for(int i = 0; i < _audioPoolSize; i++)
        {
            GameObject obj = new GameObject("Sound" + i);
            AudioSource audioSource =  obj.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = _volume;
            audioSource.minDistance = _soundDistanceMin;
            audioSource.maxDistance = _soundDistanceMax;
            audioSource.spatialBlend = 1f;
            obj.transform.SetParent(transform);
            Sound sound = obj.AddComponent<Sound>();
            _audioObjects.Add(obj);
            obj.SetActive(false);
        }

    }

    public void Play2D(string key)
    {
        _audioSource.clip = clips[key];
        _audioSource.Play();
    }

    public void Play3D(string key, Vector3 pos)
    {
        //��Ƽ�� �����ִ� �� ã�Ƽ� �÷���
        foreach (GameObject audioObject in _audioObjects)
        {
            if (!audioObject.activeSelf)
            {
                Sound sound = audioObject.GetComponent<Sound>();
                sound.Play(clips[key], pos);
                return;
            }

        }
       

    }
}
