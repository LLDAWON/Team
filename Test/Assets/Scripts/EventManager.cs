using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private EventData _eventData;
    private string _eventKey = null;
    private List<int> _preEventKey = new List<int>();
    private Vector3 _size;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _size = _collider.bounds.size*2.0f;
    }

    private void Update()
    {
        CheckCollision();
    }

    private void CheckCollision()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, _size);

        if(colliders.Length == 0 )
            _eventKey = null;
        else
        {
            foreach (Collider collider in colliders)
            {
                _eventData = DataManager.Instance.GetEventData(collider.tag);
                UIManager.Instance.ConditionKey.gameObject.SetActive(false);
                if (_eventData.EventTag == "None")
                    continue;
                else
                {
                    UIManager.Instance.ConditionKey.gameObject.SetActive(true);
                    _eventKey = collider.tag;
                    ClickKey();
                }
            }
        }       
    }
    private void ClickKey()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            CheckPreEvent();          
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
                }
                else
                    return;
            }
        }
        else
        {
            SendText();
        }
    }

    private void SendText()
    {
        UIManager.Instance.SetText(_eventData.TextDataKey);
        _preEventKey.Add(_eventData.Key);
        _eventKey = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, _size);
    }
}
