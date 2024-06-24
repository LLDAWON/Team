using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    private bool _isSetItem = false;
    private int _count = 0;
    private Button _button;
    private ItemData _data;

    private void Awake()
    {
        //_image.gameObject.SetActive(false);
        _button = transform.GetChild(0).GetComponent<Button>();
        _button.AddComponent<AudioSource>();
        _button.GetComponent<AudioSource>().clip = Resources.Load("Sounds/UIButton") as AudioClip;
        _button.GetComponent<AudioSource>().playOnAwake = false;
        if (_button != null)
        {
            _button.onClick.AddListener(() => OnSlotClick());
        }

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
        _image.transform.GetChild(0).gameObject.SetActive(true);
        _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _count.ToString();
        _isSetItem = true;
        _image.gameObject.SetActive(true);
        _image.color = Color.white;
    }

    public void AddItem()
    {
        _count++;
        _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _count.ToString();
    }

    private void OnSlotClick()
    {
        if (_count > 0)
        {
            _button.GetComponent<AudioSource>().Play();
            UseItem();
        }
    }
    private void UseItem()
    {   
        _count--;
        _image.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _count.ToString();

        if (_data.Type == 3)
        {
            Observer.OnEvents[_data.Key](_data.Value);
        }
        else if (_data.Type == 1) 
        {
            Observer.OnNoneEvents[_data.Key]();     
        }

        if (_count == 0)
        {
            _image.sprite = null;
            _image.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            _image.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
