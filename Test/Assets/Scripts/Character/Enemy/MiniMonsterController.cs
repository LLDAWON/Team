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
        //���ݻ����϶� ����
        if (_enemyState == EnemyState.Attack)
            return;

        //�÷��̾ ������ �ǵ�����
        if (_target.GetComponent<PlayerController>().GetIsPlayerDie() == true)
            return;

        PlayerController playerController = _target.GetComponent<PlayerController>();

        if (playerController.GetIsPlayerDie() == true)
            SetState((int)EnemyState.None);
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
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("None");
                }
                break;
        }
    }
   
}
