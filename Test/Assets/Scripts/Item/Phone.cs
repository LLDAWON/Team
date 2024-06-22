using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    private ItemData _data;

    private readonly int _dataKey = 102;
    private Light _light;
    private AudioSource _audioSource;
    public bool IsPlaying()
    {
        if (_light == null)
            return false;

        if(_light.enabled)
        {
            return true;
        }
        else 
        { return false; }
    }

    private void Awake()
    {
        _data = DataManager.Instance.GetItemData(_dataKey);
       _light = transform.GetChild(0).GetComponent<Light>();
        _light.enabled = false;
        _audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OnFlash();
        }
    }


    private void OnFlash()
    {
        _light.enabled = true;
        _audioSource.Play();
        Invoke("DiableFlash", 0.5f);
    }

    private void DiableFlash()
    {
        _light.enabled = false;
    }


}
