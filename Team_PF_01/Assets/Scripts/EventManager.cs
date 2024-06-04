using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private EventData _eventData;
    private string _eventKey = null;
    private List<int> _preEventKey = new List<int>();

    private void Update()
    {
        ClickKey();
    }


    private void OnTriggerEnter(Collider other)
    {
        _eventKey = other.tag;
        _eventData = DataManager.Instance.GetEventData(_eventKey);
    }

    private void OnTriggerExit(Collider other)
    {
        _eventKey = null;
    }

    private void ClickKey()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (_eventKey == null) return;
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
    }

}
