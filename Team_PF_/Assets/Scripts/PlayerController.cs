using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MoveableCharactorController
{





    protected override void Update()
    {
        MoveController();
        ItemUseController();
    }


    private void MoveController()
    {
        //
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _moveSpeed = _characterData.RunSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _moveSpeed = _characterData.CrawlingSpeed;
        }
        else
        {
            _moveSpeed = _characterData.WalkSpeed;
        }
        _velocity.x *= _moveSpeed;
        _velocity.z *= _moveSpeed;
    }

    private void ItemUseController()
    {
        if (Input.GetKey(KeyCode.F))
        {
            // ������ ��ȣ�ۿ� �̋��� ������ �ڵ忡 ���� �ൿ�� �޶����� 
            // ������ϰ�� (Type == 1) �����ϰ�
            // Ű�ϰ�� (Type == 2) ���� �����ؼ� �ش�Ű�� ���� ��ġ�Ұ�� 
            // �Һ����ϰ�� (Type == 3) ������ ȸ���ϰ�
            //
        }

    }

}
