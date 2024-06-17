using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Picture : MonoBehaviour
{
    private ItemData _data;

    private readonly int _dataKey = 406;
    private bool _active;
    public bool Active
    { get { return _active; } }

    private void Awake()
    {
        _data = DataManager.Instance.GetItemData(_dataKey);
        _active = gameObject.activeSelf;
    }
    public void UsedPicture()
    {
        StartCoroutine(Observer.OnDesolveEvents[1](gameObject));

    }
}
