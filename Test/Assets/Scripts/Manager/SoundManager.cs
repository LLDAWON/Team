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
    private int _soundDistanceMin = 1;
    private int _soundDistanceMax = 10;

    private AudioSource _audioSource;
    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private List<Sound> _audioObjects = new List<Sound>();
    private string _lastPlayedKey = ""; // 중복방지용 검색키 변수

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
            audioSource.loop = false;
            audioSource.volume = _volume;
            audioSource.minDistance = _soundDistanceMin;
            audioSource.maxDistance = _soundDistanceMax;
            audioSource.spatialBlend = 1f;
            obj.transform.SetParent(transform);
            Sound sound = obj.AddComponent<Sound>();
            _audioObjects.Add(sound);
            obj.SetActive(false);
        }

    }

    public void Play2D(string key, bool isLoop)
    {
        _audioSource.clip = clips[key];
        _audioSource.Play();
        _audioSource.loop = isLoop;
    }
    public void Change2D(string key, bool isLoop)
    {
        _audioSource.Stop();
        _audioSource.clip = clips[key];
        _audioSource.Play();
        _audioSource.loop = isLoop;
    }
    public void Stop2D(string key)
    {
        _audioSource.Stop();
    }

    //고정되어있는 정적 물체에 달기
    public void Play3D(string key, Vector3 pos, bool isLoop)
    {
        //액티브 꺼져있는 애 찾아서 플레이
        foreach (Sound audioObject in _audioObjects)
        {
            if (!audioObject.gameObject.activeSelf)
            {
                audioObject.Play(clips[key], pos, isLoop);
                return;
            }

        }     

    }

    //움직이는 동적 물체에 달기 (플레이어/ enemy들)
    public void Play3D(string key, Transform parent, bool isLoop, float speed)
    {
        //액티브 꺼져있는 애 찾아서 플레이
        foreach (Sound audioObject in _audioObjects)
        {
            if (!audioObject.gameObject.activeSelf)
            {
                audioObject.Play(clips[key], parent, isLoop, speed);
                return;
            }

        }

    }

    public void Stop3D(string key)
    {
        foreach (Sound audioObject in _audioObjects)
        {
            if(audioObject.audioSource.clip == clips[key])
            {
                if (audioObject.IsPlaying())
                {
                    audioObject.Stop(clips[key]);
                    ResetLastPlayedKey(key);
                    return;
                }
            }
        }
    }

    //
    //만약 사운드가 1번 다돌면 리셋해주자
    public void ResetLastPlayedKey(string key)
    {
        if (_lastPlayedKey == key)
        {
            // 이미 재생 중이면 반환
            _lastPlayedKey = "";
            Debug.Log("키 리셋");
        }
    }


    //꺼져있는애들중에서 같은이름이면 반환해주자
    public void SameStateJustOnePlay3D(string key, Transform parent, bool isLoop, float speed)
    {

        if (_lastPlayedKey == key)
        {
            return;
        }

        foreach (Sound audioObject in _audioObjects)
        {
            if (audioObject.IsPlaying())
            {
                // 이미 재생 중이면 반환
                return;
            }
        }
        // 재생 중이 아니면 사운드 재생
        Play3D(key, parent, isLoop, speed);
        _lastPlayedKey = key; // 마지막 재생된 사운드 키 업데이트
    }

    public void Init()
    {
        foreach (Sound audioObject in _audioObjects)
        {
            audioObject.transform.parent = transform;
        }
    }
}
