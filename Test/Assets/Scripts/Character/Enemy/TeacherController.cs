using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : EnemyController
{



   
   

    protected override void StateUpdate()
    {
        //공격상태일때 return
        if (_enemyState == EnemyState.Attack)
            return;
        //첫조우전엔 멈춤
        CheckFirstMeetPlayer();
        if (!_isFirstMeet)
            return;
        //숨어있으면 멈춤
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(3);
            return;
        }
        //플레이어가 바라보면 멈춤
        PlayerController playerController = _target.GetComponent<PlayerController>();
        

        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange * 2.0f;

        if (_inPlayerSight.magnitude <= _detectRange)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange)
            {
                SetState(3);
                return;
            }
            else
            {
                SetState(1);
            }
        }
        else
        {
            SetState(1);
        }


    }
}
