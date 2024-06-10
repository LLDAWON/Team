using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMonsterController : EnemyController
{

    override protected void StateUpdate()
    {
        //�÷��̾ ������ ����
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }

        // �÷��̾ �Ĵٺ��� ������ ������
        PlayerController playerController = _target.GetComponent<PlayerController>();


        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange  ;

        //���������ȿ� ��������
        if (_inPlayerSight.magnitude <= _detectRange )
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //��ä�� �ȿ� ��������
            if (degree <= _angleRange)
            {
                //��������
                if(playerController.GetIsLightOn() == true)
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



}
