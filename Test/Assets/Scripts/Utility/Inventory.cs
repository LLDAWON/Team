using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    private Slot[] _slots;
    private Dictionary<string, Slot> _items = new Dictionary<string, Slot>();
    private void Awake()
    {
        _slots = GetComponentsInChildren<Slot>();

        gameObject.SetActive(false);
    }

    private void Start()
    {
        
    }


    public void AddItem(ItemData itemdata)
    {
        if (CheckRedundancyItem(itemdata.Name))
        {
            _items[itemdata.Name].AddItem();
            return;
        }

        foreach (Slot slot in _slots)
        {
            if (!slot.IsSet())
            {
                slot.SetData(itemdata);
                _items.Add(itemdata.Name, slot);
                return;
            }
        }
    }

    private void Update()
    {
        DeleteSlotData();
    }

    private bool CheckRedundancyItem(string name)
    {
        return _items.ContainsKey(name);
    }

    private void DeleteSlotData()
    {
        foreach (Slot slot in _slots)
        {
            if (slot.IsSet() && slot.Count() == 0)
            {
                slot.SetIsItem(false);
                slot.transform.GetChild(0).gameObject.SetActive(false);
                _items.Remove(slot.SlotData().Name);
                return;
            }
        }
    }    
}
