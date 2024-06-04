using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    { get { return _instance; } }

    private float _textTime = 4.0f;

    private TextData _textData;
    private TextMeshProUGUI _text;

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
        _text.gameObject.SetActive(false);
    }

    private void Exposuretime()
    {
        _text.gameObject.SetActive(false);
    }
}
