using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : EnemyController
{



   
   

    protected override void StateUpdate()
    {
        PlayerController playerController = _target.GetComponent<PlayerController>();

        Vector3 _inPlayerSight  = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;


        if (_inPlayerSight.magnitude <= _characterData.DetectRange)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange)
            {
                SetState(3);
                Debug.Log("TeacherStop");
                return;
            }
            else
            {
                base.StateUpdate();
                Debug.Log("TeacherMove");
            }
        }
        else
        {
            base.StateUpdate();
        }


    }
}
