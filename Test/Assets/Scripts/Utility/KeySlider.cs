using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySlider : MonoBehaviour
{
    private Slider _slider;
    private float _pressTime = 0.0f;
    private float _maxPressTime = 3.0f;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void OnPressKey(GameObject gameObject)
    {
        if(Input.GetKey(KeyCode.F))
        {
            _pressTime += Time.deltaTime;
            _slider.value = _pressTime / _maxPressTime;
            MaxValue(gameObject);
        }
        else if(Input.GetKeyUp(KeyCode.F))
        {
            _pressTime = 0.0f;
            _slider.value = _pressTime / _maxPressTime;
        }
    }

    private void MaxValue(GameObject gameObject)
    {
        if(_pressTime >= _maxPressTime)
        {
            // 지금 오브젝트 말고 다른 오브젝트들은 사용 못하는 함수...
            //GameObject obj = GameObject.Find("SCP-096").gameObject;
            //obj.GetComponent<Picture>().UsedPicture();

            if(gameObject.CompareTag("Picture"))
            {
                //StartCoroutine(Observer.OnDesolveEvents[1](gameObject));
                gameObject.GetComponent<Picture>().UsedPicture();
            }
            else if(gameObject.CompareTag("Candle"))
            {

            }

        }
    }
}
