using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _audioSource;
    private Transform _parent;

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
    public void Play(AudioClip clip, Vector3 position)
    {
        transform.position = position;
        _audioSource.clip = clip;
        gameObject.SetActive(true);
        _audioSource.Play();
    }

    public void Play(AudioClip clip, Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        _audioSource.clip = clip;
        gameObject.SetActive(true);
        _audioSource.Play();
    }
}
