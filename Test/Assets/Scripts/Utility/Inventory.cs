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
            GameObject slot = transform.GetChild(i).GetChild(0).gameObject;
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

                    Button button = _itemSlots[i].GetComponent<Button>();
                    if (button != null)
                    {
                        int slotIndex = i; // 람다를 위한 인덱스 캡처
                        button.onClick.AddListener(() => OnSlotClick(slotIndex));
                    }

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

    private void Update()
    {
        DeleteSlotData();
    }

    private bool CheckRedundancyItem(string name)
    {
        return _items.ContainsKey(name);
    }

    public void OnSlotClick(int slotIndex)
    {
        Image slotImage = _itemSlots[slotIndex].GetComponent<Image>();
        if (slotImage.sprite == null) return;

        string itemName = slotImage.sprite.name;
        UseItem(itemName);
    }

    public void UseItem(string name )
    {
        _items[name]--;
        _countTxTs[name].text = _items[name].ToString () ;

        if (_items[name] == 0)
        {
            foreach (Image slot in _itemSlots)
            {
<<<<<<< Updated upstream
                slot.SetIsItem(false);
                slot.transform.GetChild(0).gameObject.SetActive(false);
                _items.Remove(slot.SlotData().Name);
                return;
=======
                if(slot.sprite == null) continue;

                if (slot.sprite.name == name)
                {
                    slot.sprite = null;
                    slot.gameObject.SetActive(false);
                    return;
                }
>>>>>>> Stashed changes
            }
            _items.Remove(name);
            _countTxTs.Remove(name);
        }
    }

}
