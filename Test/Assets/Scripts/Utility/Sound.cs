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
        if (!_audioSource.isPlaying)
        {
            gameObject.SetActive(false);
            transform.SetParent(_parent);
        }
    }
    public void Play(AudioClip clip, Vector3 position, bool isLoop)
    {
        transform.position = position;
        _audioSource.clip = clip;
        _audioSource.loop = isLoop;
        gameObject.SetActive(true);
        _audioSource.Play();
    }

    public void Play(AudioClip clip, Transform parent, bool isLoop)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        _audioSource.clip = clip;
        _audioSource.loop = isLoop;
        gameObject.SetActive(true);
        _audioSource.Play();
    }
}
