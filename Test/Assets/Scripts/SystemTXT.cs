using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemTXT : MonoBehaviour
{
    private TextData _txtData;
    private int _txtDataKey;
    private TextMeshProUGUI _txt;

    private void Awake()
    {
        _txt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
        _txt.gameObject.SetActive(false);
    }

    public void SetText(TextData data)
    {
        gameObject.SetActive(true);
        _txt.gameObject.SetActive(true);
        _txtData = data;
        _txt.text = _txtData.Text;
       
    }

}
