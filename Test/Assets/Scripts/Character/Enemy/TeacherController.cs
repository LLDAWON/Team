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
                _animator.speed = 1.0f;
            }
        }
        else
        {
            SetState(1);
            _animator.speed = 1.0f;
        }


    }

    override protected void EnemyAiPattern()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                {

                    _animator.speed = 0.5f;
                    _navigation.SetDestination(_destPos);
                    _navigation.speed = _characterData.WalkSpeed;
                    CameraManager.Instance.StopVignette();

                }
                break;
            case EnemyState.Trace:
                {
                    _animator.speed = 2.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _characterData.RunSpeed;
                    SoundManager.Instance.SameStateJustOnePlay3D("Teacer_trace", transform, true, 1.0f);
                    _target.GetComponent<PlayerController>().GetHeartBeatSound().volume = 1.0f;

                    CameraManager.Instance.StartVignette();
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    if (!_isAttack)
                    {
                        _animator.SetTrigger("Attack");

                        Observer.OnTargetEvents[1](gameObject);
                        Debug.Log(_isAttack);
                    }
                    _isAttack = true;
                    Debug.Log("공격중");
                }
                break;
            case EnemyState.Stop:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    _animator.speed = 0.0f;
                    _target.GetComponent<PlayerController>().GetHeartBeatSound().volume = 0.0f;
                    SoundManager.Instance.Stop3D("Teacer_trace");
                }
                break;
            case EnemyState.None:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
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
        }
    }
    override protected void CheckFirstMeetPlayer()
    {
        PlayerController playerController = _target.GetComponent<PlayerController>();

        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        if (_inPlayerSight.magnitude <= playerController.GetCharacterData().DetectRange && !_isFirstMeet)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            if (degree <= _angleRange)
            {
                _isFirstMeet = true;
                _animator.SetTrigger("MeetPlayer");
                Debug.Log("플레이어가 적을 찾았습니다.");
                return;
            }
        }
    }
}
