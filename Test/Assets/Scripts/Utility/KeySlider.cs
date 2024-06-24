using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            if(gameObject.CompareTag("Picture"))
            {
                gameObject.GetComponent<Picture>().UsedPicture();
                GameObject obj = GameObject.FindGameObjectWithTag("Objects");
                obj.GetComponent<ObjectsController>().Target(gameObject);
                GameManager.Instance.GetPlayer().GetComponent<EventManager>().CallEvent(gameObject.tag);
            }
            else if(gameObject.CompareTag("Candle"))
            {
                gameObject.GetComponent<CandleScript>().SetLit(true);
            }
            else if (gameObject.CompareTag("BookCase"))
            {
                ObjectManager.Instance.BookCase();

            }
            else if (gameObject.CompareTag("Vent"))
            {
                SoundManager.Instance.Init();
                UIManager.Instance.SetText(20);
                SceneManager.LoadScene(3);
                UIManager.Instance.CandleUI.gameObject.SetActive(true); // UI 활성화
                UIManager.Instance.StartCoroutine("EscapeTime"); // 코루틴 시작
            }
            else if (gameObject.CompareTag("Cal"))
            {
                UIManager.Instance.VentUI.SetActive(true);
                UIManager.Instance.SetUIOpne(true);                
            }
        }
    }
}
