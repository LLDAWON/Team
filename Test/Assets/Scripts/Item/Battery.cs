using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField]
    private int _dataKey;

    private ItemData _data;

    private void Awake()
    {
        _data = DataManager.Instance.GetItemData(_dataKey);
    }

    private void UseBattery()
    {

    }

}
