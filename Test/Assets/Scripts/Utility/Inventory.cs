using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Image> _itemSlots = new List<Image>();
    private int _itemCount;

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            _itemSlots.Add(transform.GetChild(i).GetComponent<Image>());
        }
    }

    public void AddItem(int key)
    {
        foreach(Image item in _itemSlots)
        {
            if(!item.transform.GetChild(0).gameObject.activeSelf)
            {
                item.transform.GetChild(0).GetComponent<Image>().sprite = null;
            }
        }
    }

    public void UseItem()
    {

    }

}
