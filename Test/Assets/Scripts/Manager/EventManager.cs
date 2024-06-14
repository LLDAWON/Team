using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    private EventData _eventData;
    private string _eventKey = null;
    private List<int> _preEventKey = new List<int>();
    private Vector3 _size;
    private Collider _collider;
    private RaycastHit hit;
    public int CurEvent()
    {
        return _eventData.Key;
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
            else
            {
                UIManager.Instance.GetCursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/HitCursor") as Sprite;
                if (_eventData.Type == 1)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        CheckPreEvent();
                        hit.collider.GetComponent<TestAnimation>().PlayAnimation();
                        return;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        CheckPreEvent();
                        if (_eventData.GetItemKey == 0)
                            return;
                        hit.collider.gameObject.SetActive(false);
                        return;
                    }

                }

            }
        }
        else
        {
            //UIManager.Instance.GetCursor.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/UI/Cursur") as Sprite;
            _eventKey = null;
        }
        

    }
    private void CheckPreEvent()
    {
        _eventKey = hit.collider.tag;

        if (_eventData.Condition > 0)
        {
            foreach(int i in _preEventKey)
            {
                if (_eventData.Condition == i)
                {
                    SendText();
                    return;
                }   
            }
        }
        else
        {
            SendText();
        }
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

    ////중복진행되는 퀘스트 없애기
    //private bool CheckRedundancy(int key)
    //{
    //    if (_eventData.Key >= 100) return false;
    //
    //    foreach(int preKey in _preEventKey)
    //    {
    //        if(preKey == key) 
    //        {
    //            return true;
    //        }            
    //    }
    //
    //    return false;
    //}
}
