using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MoveableCharactorController
{





    protected override void Update()
    {
        MoveController();
        JumpController();
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
            _animator.SetFloat("Speed", 5.0f);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _moveSpeed = _characterData.CrawlingSpeed;
            _animator.SetFloat("Speed", 1.0f);
        }
        else
        {
            _moveSpeed = _characterData.WalkSpeed;
            _animator.SetFloat("Speed", 3.0f);
        }
            _velocity.x *= _moveSpeed;
            _velocity.z *= _moveSpeed;

        if (Mathf.Abs(_velocity.x) < 0.1f && Mathf.Abs(_velocity.z) < 0.1f)
        {
            //_velocity.x = 0.0f;
            //_velocity.z = 0.0f;
            _animator.SetFloat("Speed", 0.0f);
        }



    }


    private void JumpController()
    {
        if (Input.GetKey(KeyCode.Space) && !_isJump)
        {
            _velocity.y = _characterData.JumpPower;
            _isJump = true;
            _animator.SetTrigger("Jump");
        }
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
