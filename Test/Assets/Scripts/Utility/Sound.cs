using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _audioSource;
    private Transform _parent;

    public AudioSource audioSource
    { get { return _audioSource; } }

    private string _currentKey;

    //사운드 재생 끝나면 엑티브 꺼주기

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        _parent = transform.parent;
    }

    private void Update()
    {
        if (!_audioSource.isPlaying && gameObject.activeSelf)
        {
            Debug.Log("마지막 재생된키 " + _currentKey);
            SoundManager.Instance.ResetLastPlayedKey(_currentKey);
            gameObject.SetActive(false);
            transform.SetParent(_parent);
        }
    }
    public void Play(AudioClip clip, Vector3 position, bool isLoop)
    {
        _currentKey = clip.name;
        transform.position = position;
        _audioSource.clip = clip;
        _audioSource.loop = isLoop;
        gameObject.SetActive(true);
        _audioSource.Play();
    }

    public void Play(AudioClip clip, Transform parent, bool isLoop,float speed)
    {
        _currentKey = clip.name;
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        _audioSource.clip = clip;
        _audioSource.loop = isLoop;
        _audioSource.pitch = speed;
        gameObject.SetActive(true);
        _audioSource.Play();
        Debug.Log("현재키 " + _currentKey);
    }
    public void Stop(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Stop();
        gameObject.SetActive(false);
        transform.SetParent(SoundManager.Instance.transform);  //멈추게했을때 다시 풀링으로 돌아가게
    }
    public bool IsPlaying()
    {
        return gameObject.activeSelf && _audioSource.isPlaying;
    }
}
