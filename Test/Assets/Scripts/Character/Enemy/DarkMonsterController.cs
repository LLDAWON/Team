using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMonsterController : EnemyController
{

    override protected void StateUpdate()
    {
        //플레이어가 숨으면 순찰
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }

        // 플레이어가 쳐다보고 손전등 켰을때
        PlayerController playerController = _target.GetComponent<PlayerController>();


        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange  ;

        //감지범위안에 들어왔을때
        if (_inPlayerSight.magnitude <= _detectRange )
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //부채꼴 안에 들어왔을때
            if (degree <= _angleRange)
            {
                //불켰을때
                if(playerController.GetIsLightOn() == true)
                    SetState(5); //주금
                else
                    SetState(1); // 추적
            }
            //부채꼴안에 안들어왔을때
            else
                SetState(1); //추적
        }
        // 감지범위 밖일때도 추적해오자
        else
        {
            SetState(1);
        }


    }



}
