using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    private List<Image> _itemSlots = new();
    private Dictionary<string, TextMeshProUGUI> _countTxTs = new();
    private Dictionary<string, int> _items = new Dictionary<string, int>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            _itemSlots.Add(transform.GetChild(i).GetChild(0).GetComponent<Image>());
        }

    }

    public void AddItem(ItemData itemdata)
    {
        for(int i = 0; i < _itemSlots.Count; i++)
        {
            if(!CheckRedundancyItem(itemdata.Name))
            {
                //중복되는 아이템이 없을 경우
                if (!_itemSlots[i].gameObject.activeSelf)
                {
                    _items.Add(itemdata.Name, 1);

                    _itemSlots[i].sprite = Resources.Load<Sprite>(itemdata.ImagePath) as Sprite;
                    _countTxTs.Add(itemdata.Name, _itemSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                    _countTxTs[itemdata.Name].text = _items[itemdata.Name].ToString();

                    _itemSlots[i].gameObject.SetActive(true);

                    return;
                }
            }            
            else
            {
                //중복 아이템인 경우 해당 아이템의 txt를 올려줘야함
                _items[itemdata.Name]++;
                _countTxTs[itemdata.Name].text = _items[itemdata.Name].ToString();
                return;
            }
        }

    }

    private bool CheckRedundancyItem(string name)
    {
        return _items.ContainsKey(name);
    }
    //이렇게 하면 슬롯 읽어오기 불편할듯...;ㅠ
    public void UseItem(string name )
    {
        _items[name]--;
        _countTxTs[name].text = _items[name].ToString () ;

        if (_items[name] == 0)
        {
            foreach (Image slot in _itemSlots)
            {
                if (slot.sprite.name == name)
                {
                    slot.sprite = null;
                    slot.gameObject.SetActive(false);
                }
            }
            _items.Remove(name);
            _countTxTs.Remove(name);
        }
    }

}
