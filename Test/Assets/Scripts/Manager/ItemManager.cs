using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{

    private ItemData _itemData;
    private int _itemDataKey;
    public void GetItem(int key)
    {
        _itemDataKey = key;
        _itemData = DataManager.Instance.GetItemData(_itemDataKey);
        Debug.Log(_itemData.Name);
        if (_itemData.Type == 4) return;
        UIManager.Instance.GetInventory.AddItem(_itemData);
        
    }

}
