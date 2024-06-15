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
                // _head�� target�� �ٶ󺸵��� ȸ��
                Vector3 direction = (_target.position - _head.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                _head.rotation = Quaternion.Slerp(_head.rotation, lookRotation, Time.deltaTime * 2.0f);
            }

            // ���� �����ӱ��� ��ٸ��ϴ�.
            yield return null;
        }
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        // ���� �ܰ迡�� �̸��� üũ
        if (parent.name == name)
        {
            return parent;
        }

        // �ڽĵ� Ž��
        foreach (Transform child in parent)
        {
            Transform result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }

        return null; // ã�� ���� ���
    }

    protected override void StateUpdate()
    {
        //���ݻ����϶� ����
        if (_enemyState == EnemyState.Attack)
            return;
        //�÷��̾ ������ �ǵ�����
        if (_target.GetComponent<PlayerController>().GetIsPlayerDie() == true)
            return;

        PlayerController playerController = _target.GetComponent<PlayerController>();
        //�÷��̾��� ��ä�� Ž��
        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange;

        //���������ȿ� ��������
        if (_inPlayerSight.magnitude <= _detectRange)
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //��ä�� �ȿ� ��������
            if (degree <= _angleRange)
            {
                //��������
                if (playerController.GetIsLightOn() == true)
                    SetState(5); //�ֱ�
                else
                    SetState(1); // ����
            }
            //��ä�þȿ� �ȵ�������
            else
                SetState(1); //����
        }
        // �������� ���϶��� �����ؿ���
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
                    Debug.Log("����");
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
        float duration = 1.0f;  // �Ⱓ�����̵�
        float elapsed = 0.0f;
        //���ݴ��ϸ� �̵�
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
