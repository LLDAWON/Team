using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    private List<GameObject> _itemSlots = new();
    private Dictionary<string, TextMeshProUGUI> _countTxTs = new();
    private Dictionary<string, int> _items = new Dictionary<string, int>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            _itemSlots.Add(transform.GetChild(i).GetChild(i).GetComponent<GameObject>());
        }

    }

    public void AddItem(ItemData itemdata)
    {
        for(int i = 0; i < _itemSlots.Count; i++)
        {
            if(!CheckRedundancyItem(itemdata.Name))
            {
                //�ߺ��Ǵ� �������� ���� ���
                if (!_itemSlots[i].transform.GetChild(0).gameObject.activeSelf)
                {
                    _items.Add(itemdata.Name, 1);
                    _itemSlots[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(itemdata.ImagePath) as Sprite;
                    _itemSlots[i].gameObject.SetActive(true);                   
                    _countTxTs.Add(itemdata.Name, _itemSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                    _countTxTs[itemdata.Name].text = _items[itemdata.Name].ToString();
                    _countTxTs[itemdata.Name].gameObject.SetActive(true) ;                    

                    return;
                }
            }            
            else
            {
                //�ߺ� �������� ��� �ش� �������� txt�� �÷������
                _items[itemdata.Name]++;
                _countTxTs[itemdata.Name].text = _items[itemdata.Name].ToString();
                return;
            }
        }

    }

    private bool CheckRedundancyItem(string name)
    {
        if (_items.ContainsKey(name))
        {            
            return true;
        }
        return false;
    }
    //Player ������ ��� �� �ʿ��� �Լ� > ���Դ� ��ȣ�� ������� Ŭ������ Ȯ������ ����
    public void UseItem(int key )
    {
        ItemData itemData =  DataManager.Instance.GetItemData(key);
        _items[itemData.Name]--;
        _countTxTs[itemData.Name].text = _items[itemData.Name].ToString () ;

        _itemSlots[key].transform.GetChild(0).GetComponent<Image>().sprite = null;
        _itemSlots[key].transform.GetChild(0).gameObject.SetActive(false); 
    }

}
