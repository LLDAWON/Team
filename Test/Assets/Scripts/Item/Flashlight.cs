using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light lightSource;
    public KeyCode switchKey = KeyCode.Tab;

    private ItemData _data;

    private float _maxValue;
    private float _usedValue = 35.0f;
    private readonly int _dataKey = 101;
    private bool _enabled = true;


    private void Awake()
    {
        _data = DataManager.Instance.GetItemData(_dataKey);
        _maxValue = _data.Value;

        Observer.OnEvents.Add(301, ChargeBattery);
    }
    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            lightSource.enabled = !lightSource.enabled;
            UIManager.Instance.Battery.SetActive(lightSource.enabled);
        }

        Battery();
    }

    private void Battery()
    {
        if(lightSource.enabled)
        {
            StartCoroutine("FlashRoutine");
        }
        else
        {
            StopCoroutine("FlashRoutine");
        }

        if (!_enabled)
        {
            lightSource.enabled = _enabled;
        }
    }

    IEnumerator FlashRoutine()
    {
        while (_enabled)
        {
            yield return new WaitForSeconds(_usedValue);
            OnFlash();

            StopCoroutine("FlashRoutine");
        }
    }

    private void OnFlash()
    {
        int batteryCount = (int)((int)_maxValue / _usedValue);
        _maxValue -= _usedValue;

        if(_maxValue <= 0.0f)
        {
            _enabled = false;
            return;
        }    

        if (batteryCount == UIManager.Instance.Battery.transform.childCount)
        {
            return;
        }            
        
        UIManager.Instance.Battery.transform.GetChild(batteryCount).gameObject.SetActive(false);
    }

    public void ChargeBattery(float value)
    {
        _maxValue += value;
        _enabled = true;

        if(_maxValue > _data.Value)
        {
            _maxValue = _data.Value;
        }

        for(int i = 0; i < UIManager.Instance.Battery.transform.childCount; i++)
        {
            UIManager.Instance.Battery.transform.GetChild(i).gameObject.SetActive(true);
        }

    }
}
