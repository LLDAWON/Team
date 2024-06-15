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

    private void UsedPicture()
    {
        Vector3 pos = GameManager.Instance.GetPlayer().transform.position;

        transform.position = pos;

    }
}
