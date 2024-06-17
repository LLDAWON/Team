using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
    private GameObject _target;

    public void Target(GameObject target)
    { _target = target; }
    private void Update()
    {
        if (_target == null) return;

        if(!_target.activeSelf)
        {
            GameManager.Instance.ChangeCamera();
            StartCoroutine(Observer.OnDesolveEvents[1](gameObject));
            _target = null;
            return;
        }
    }
}
