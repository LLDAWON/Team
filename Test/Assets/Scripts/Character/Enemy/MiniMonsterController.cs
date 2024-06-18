using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMonsterController : EnemyController
{
    
    override protected void Start()
    {
       base.Start();
    }
    


    protected override void StateUpdate()
    {
        //공격상태일때 빠꾸
        if (_enemyState == EnemyState.Attack)
            return;

        //플레이어가 죽으면 되돌리기
        if (_target.GetComponent<PlayerController>().GetIsPlayerDie() == true)
            return;

        PlayerController playerController = _target.GetComponent<PlayerController>();

        if (playerController.GetIsPlayerDie() == true)
            SetState((int)EnemyState.None);
        //플레이어의 부채꼴 탐색
        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange;

        //감지범위안에 들어왔을때
        if (_inPlayerSight.magnitude <= _detectRange)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //부채꼴 안에 들어왔을때
            if (degree <= _angleRange)
            {
                //불켰을때
                if (playerController.GetIsLightOn() == true)
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
    protected override void EnemyAiPattern()
    {

        switch (_enemyState)
        {
            case EnemyState.Trace:
                {
                    _animator.speed = 1.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.RunSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;

                    _animator.SetTrigger("Attack");
                    Debug.Log("EnemyState.Attack");

                    Observer.OnTargetEvents[1](gameObject);
                }

                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("죽음");
                    gameObject.SetActive(false);
                }
                break;
            case EnemyState.None:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("None");
                }
                break;
        }
    }
   
}
