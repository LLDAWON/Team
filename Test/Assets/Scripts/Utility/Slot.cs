using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private bool _isSetItem = false;
    private int _count = 0;

    private TextMeshProUGUI _countTXT;
    private Image _image;
    private GameObject _itemButton;

    private ItemData _data;

    private void Awake()
    {
        _itemButton = transform.GetChild(0).gameObject;
        _countTXT = _itemButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _image= _itemButton.GetComponent<Image>();
        _itemButton.gameObject.SetActive(false);
    }


    public bool IsSet()
    {
        return _isSetItem;
    }
    public int Count()
    { return _count; }

    public void SetIsItem(bool isItem)
    { _isSetItem = isItem; }
    public ItemData SlotData()
    { return _data; }

    public void SetData(ItemData data)
    {
        _data = data;
        _count++;
        _image.sprite = Resources.Load<Sprite>(data.ImagePath) as Sprite;
        _countTXT.text = _count.ToString();

        Button button = transform.GetChild(0).GetComponent<Button>();

        if(button != null )
        {
            button.onClick.AddListener(() => OnSlotClick());
        }

        _isSetItem = true;
        _itemButton.gameObject.SetActive(true);

    }

    public void AddItem()
    {
        _count++;
        _countTXT.text = _count.ToString();
    }

    private void OnSlotClick()
    {
        if(_image != null)
        {
            UseItem();
        }
    }

    private void UseItem()
    {
        _count--;
        _countTXT.text = _count.ToString();

        if(_count == 0 )
        {
            _image.sprite = null;
        }
    }

}
