using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArchiveTXT : MonoBehaviour
{
    private TextData _txtData;
    private int _txtDataKey;
    private TextMeshProUGUI _txt;
    private float _time = 5.0f;

    private void Awake()
    {
        gameObject.SetActive(false);
        _txt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetText(TextData data)
    {
        _txtData = data;
        _txt.text = _txtData.Text;
        gameObject.SetActive(true);

        Invoke("Exposuretime", _time);
    }

    private void Exposuretime()
    {
        gameObject.SetActive(false);
        _txt.gameObject.SetActive(false);
    }
}
