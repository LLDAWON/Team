using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// OBJ Layer 에 있는 오브젝트와 충돌 시 진행되는 이벤트(퀘스트) > 특정 위치에서 발생하는 퀘스트 진행 로직 필요 
public class EventManager : MonoBehaviour
{
    private EventData _eventData;
    private string _eventKey = null;
    private List<int> _preEventKey = new List<int>();
    private Vector3 _size;
    private Collider _collider;

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
        Collider[] colliders = Physics.OverlapBox(transform.position, _size, Quaternion.identity, _layerMask);

        if(colliders.Length == 0)
        {
            _eventKey = null;
            UIManager.Instance.ConditionKey.gameObject.SetActive(false);
        }            
        else
        {
            foreach (Collider collider in colliders)
            {
                _eventData = DataManager.Instance.GetEventData(collider.tag);
                
                if (_eventData.EventTag == "None" || CheckRedundancy(_eventData.Key))
                    continue;
                else
                {
                    UIManager.Instance.ConditionKey.gameObject.SetActive(true);

                    _eventKey = collider.tag;
                    
                    if (_eventData.Type == 1)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            CheckPreEvent();
                            collider.GetComponent<Animation>().Play();
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            CheckPreEvent(); 
                            collider.gameObject.SetActive(false);
                        }
                       
                    }
                }
            }
        }       
    }

    private void CheckPreEvent()
    {
        if(_eventData.Condition > 0)
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

    //중복진행되는 퀘스트 없애기
    private bool CheckRedundancy(int key)
    {
        if (_eventData.Key > 100) return false;

        foreach(int preKey in _preEventKey)
        {
            if(preKey == key) 
            {
                return true;
            }            
        }

        return false;
    }
}
