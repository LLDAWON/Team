using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    { get { return _instance; } }

    private float _textTime = 4.0f;

    private TextData _textData;
    private TextMeshProUGUI _text;
    private Image _conditionKey;
    private Inventory _inventory;

    public Inventory GetInventory() { return _inventory; }
    public Image ConditionKey
    { get { return _conditionKey; } }

    public void SetText(int key)
    {
        _textData = DataManager.Instance.GetTextData(key);
        _text.text = _textData.Text;
        _text.gameObject.SetActive(true);

        Invoke("Exposuretime", _textTime);    
    }

    private void Awake()
    {
        _instance = this;
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _conditionKey = transform.GetChild(1).GetComponent<Image>();
        _inventory = transform.GetChild(2).GetComponent<Inventory>();
        _conditionKey.gameObject.SetActive(false);
        _text.gameObject.SetActive(false);
    }

    private void Exposuretime()
    {
        _text.gameObject.SetActive(false);
    }
}
