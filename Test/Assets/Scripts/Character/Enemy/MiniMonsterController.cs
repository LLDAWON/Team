using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMonsterController : EnemyController
{
    private Transform _head;

    override protected void Awake()
    {
        base.Awake();

        _head = FindChildByName(transform, "JNT_Head");
    }

    override protected void Start()
    {
        StartCoroutine(HeadLookPlayer());
       base.Start();
    }
    private IEnumerator HeadLookPlayer()
    {
        while (true)
        {
            if (_head != null && _target != null)
            {
                // _head가 target을 바라보도록 회전
                Vector3 direction = (_target.position - _head.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _head.rotation = Quaternion.Slerp(_head.rotation, lookRotation, Time.deltaTime * 2.0f);
            }

            // 다음 프레임까지 기다립니다.
            yield return null;
        }
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        // 현재 단계에서 이름을 체크
        if (parent.name == name)
        {
            return parent;
        }

        // 자식들 탐색
        foreach (Transform child in parent)
        {
            Transform result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }

        return null; // 찾지 못한 경우
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

                    StartCoroutine(MoveToTarget());
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
                    Debug.Log("None");
                }
                break;
        }
    }
    private IEnumerator MoveToTarget()
    {
        float duration = 1.0f;  // 기간동안이동
        float elapsed = 0.0f;
        //공격당하면 이동
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = _target.GetChild(0).transform.GetChild(0).position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Slerp(initialPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }
}
