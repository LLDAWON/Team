using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _audioSource;

    //���� ��� ������ ��Ƽ�� ���ֱ�

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
    public void Play(AudioClip clip, Vector3 position)
    {
        transform.position = position;
        _audioSource.clip = clip;
        gameObject.SetActive(true);
        _audioSource.Play();
    }
}
