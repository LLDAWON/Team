using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _audioMixer;

    public void SetLevel(float value)
    {
        _audioMixer.SetFloat("BGM", Mathf.Log10(value)*20);
    }
}
