using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private bool _isSetItem = false;
    private int _count = 0;
    
    private ItemData _data;

    private void Awake()
    {
        //_image.gameObject.SetActive(false);
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
        _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _count.ToString();

        Button button = transform.GetChild(0).GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(() => OnSlotClick());
        }

        _isSetItem = true;
        //_image.gameObject.SetActive(true);
    }

    public void AddItem()
    {
        _count++;
        _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _count.ToString();
    }

    private void OnSlotClick()
    {
        if (_image != null)
        {
            UseItem();
        }
    }
    private void UseItem()
    {
        _count--;
        _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _count.ToString();
   
        if(_data.Type == 3)
        {
            Opserver.OnEvents[_data.Key](_data.Value);
        }

        else if (_data.Type == 1) 
        {
            Opserver.OnNoneEvents[_data.Key]();     
        }
        if (_count == 0)
        {
            _image.sprite = null;
        }
    }

}
