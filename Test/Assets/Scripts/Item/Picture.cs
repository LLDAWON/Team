using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
    private ItemData _data;

    private readonly int _dataKey = 406;

    private void Awake()
    {
        _data = DataManager.Instance.GetItemData(_dataKey);
    }
    public void UsedPicture()
    {
        StartCoroutine(Observer.OnDesolveEvents[1](gameObject));
    }
}
