using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : EnemyController
{



   
   

    protected override void StateUpdate()
    {
        //���ݻ����϶� return
        if (_enemyState == EnemyState.Attack)
            return;
        //ù�������� ����
        CheckFirstMeetPlayer();
        if (!_isFirstMeet)
            return;
        //���������� ����
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(3);
            return;
        }
        //�÷��̾ �ٶ󺸸� ����
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
                _animator.speed = 1.0f;
            }
        }
        else
        {
            SetState(1);
            _animator.speed = 1.0f;
        }


    }
}
