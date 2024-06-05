using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private ItemData _itemData;
    private int _itemDataKey;

    private void GetItem(int key)
    {
        _itemDataKey = key;
        _itemData = DataManager.Instance.GetItemData(_itemDataKey);
    }



}
