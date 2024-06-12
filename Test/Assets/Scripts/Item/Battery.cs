using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField]
    private int _dataKey;

    private Flashlight _flash;
    private ItemData _data;

    private void Awake()
    {
        _data = DataManager.Instance.GetItemData(_dataKey);
        _flash = GameObject.Find("Flashlight_ON").GetComponent<Flashlight>();
    }

    public void UseBattery()
    {
        //_flash.ChargeBattery(_data.Value);
    }

}
