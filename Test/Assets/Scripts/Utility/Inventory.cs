using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Image> _itemSlots = new List<Image>();
    private int _itemCount;
    private string _imagePath;

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            _itemSlots.Add(transform.GetChild(i).GetComponent<Image>());
        }
    }

    public void AddItem(ItemData itemdata)
    {
        foreach(Image item in _itemSlots)
        {
            if(!item.transform.GetChild(0).gameObject.activeSelf)
            {
                itemdata.ImagePath = _imagePath;
                item.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(_imagePath) as Sprite;
                item.transform.GetChild(0).gameObject.SetActive(true);
                return;
            }
        }
    }
    //Player 아이템 사용 시 필요한 함수 > 스롯당 번호로 사용할지 클릭으로 확인할지 미정
    public void UseItem(int key )
    {
        _itemSlots[key].transform.GetChild(0).gameObject.SetActive(false); 
    }

}
