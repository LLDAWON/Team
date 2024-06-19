using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private EventData _eventData;
    private string _eventKey;
    private List<int> _preEventKey = new List<int>();
    private Vector3 _size;
    private Collider _collider;
    private RaycastHit hit;
  
    public int CurKey
    { get { return _eventData.Key; } }

    public void SetKey(int key)
    {
        _eventData.Key = key;
    }

    [SerializeField]
    private LayerMask _layerMask;
    

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _size = _collider.bounds.size*0.5f;
    }

    private void Update()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        Vector3 pos = transform.GetChild(0).transform.GetChild(0).transform.position;
        Vector3 dir = pos - transform.GetChild(0).transform.position;

        Debug.DrawRay(pos, dir*2f, Color.red);

        if (Physics.Raycast(pos, dir, out hit, 2.0f, _layerMask))
        {
            _eventData = DataManager.Instance.GetEventData(hit.collider.tag);

            if (_eventData.EventTag == "None")
                return;

            if(CheckPreEvent())
            {
                UIManager.Instance.GetCursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/HitCursor") as Sprite;

                if (_eventData.Type == 2)
                {
                    UIManager.Instance.KeySlider.gameObject.SetActive(true);
                    GameObject obj = hit.collider.gameObject;

                    UIManager.Instance.KeySlider.OnPressKey(obj);

                    return;
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    OnKeyDown();
                    return;
                }
            }
        }
        else
        {
            UIManager.Instance.KeySlider.gameObject.SetActive(false);
            UIManager.Instance.GetCursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/Cursur") as Sprite;
            //_eventKey = null;
        }
         

    }
    private void OnKeyDown()
    {
        _eventKey = hit.collider.tag;

        SendText();

        if (_eventData.Type == 1)
        {
            hit.collider.GetComponent<TestAnimation>().PlayAnimation();
            if (hit.collider.CompareTag("ClassDoor1"))
            {
                SoundManager.Instance.Play3D("Door", hit.transform.position, false);
                return;
            }
            
            SoundManager.Instance.Play3D(hit.collider.tag, hit.transform.position, false);

        }
        else if (_eventData.Type == 0)
        {
            if (_eventData.GetItemKey == 0)
                return;
            hit.collider.gameObject.SetActive(false);
        }
    }
    private bool CheckPreEvent()
    {
        if (_eventData.Condition > 0)
        {
            foreach(int i in _preEventKey)
            {
                if (_eventData.Condition == i)
                {
                    return true;
                }
            }
        }
        else
        {
            return true;
        }
        return false;
    }

    private void SendText()
    {
        if(_eventData.GetItemKey > 0)
        {
            ItemManager.Instance.GetItem(_eventData.GetItemKey);
            ConditionText(_eventData.GetItemKey);
        }

        UIManager.Instance.SetText(_eventData.TextDataKey);

        _preEventKey.Add(_eventData.Key);
        _eventKey = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, _size*2);
    }

    private void ConditionText(int key)
    {
        ItemData data = DataManager.Instance.GetItemData(key);

        if(data.Type == 4)
        {
            UIManager.Instance.SetText(_eventData.TextDataKey - 1);
            return;
        }
    }

    public void CallEvent(string key)
    {
        _eventData = DataManager.Instance.GetEventData(key);
        _eventKey = key;

        SendText();
    }

    public void LoadEvent(int key)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("4ChangeScene"))
        {
            SceneManager.LoadScene(1);
            UIManager.Instance.CandleUI.gameObject.SetActive(true);
            UIManager.Instance.StartCoroutine("Candle");
        }
        if (collision.collider.CompareTag("3ChangeScene"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
