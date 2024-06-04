using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MoveableCharactorController
{
    public float _rotateSpeed = 50.0f;

    private bool _isJump = false;
    private Vector2 _mouseValue;

    private Transform _rotateObj;

    private void Awake()
    {
        base.Awake();

        _rotateObj = transform.GetChild(0);
    }

    protected override void Update()
    {
        MoveController();
        RotateController();

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
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _moveSpeed = _characterData.CrawlingSpeed;            
        }
        else
        {
            _moveSpeed = _characterData.WalkSpeed;            
        }

        _velocity *= _moveSpeed;        
    }

    private void RotateController()
    {
        _mouseValue.x = Input.GetAxis("Mouse X");
        _mouseValue.y = Input.GetAxis("Mouse Y") * -1.0f;

        transform.Rotate(Vector3.up * _rotateSpeed * _mouseValue.x * Time.deltaTime);
        _rotateObj.Rotate(Vector2.right * _rotateSpeed * _mouseValue.y * Time.deltaTime);
    }

    private void JumpController()
    {
        if (Input.GetKey(KeyCode.Space) && !_isJump)
        {
            _velocity.y = _characterData.JumpPower;
            _isJump = true;
        }
    }
  


    private void ItemUseController()
    {
        if (Input.GetKey(KeyCode.F))
        {
            // 아이템 상호작용 이떄의 아이템 코드에 따라서 행동이 달라지게 
            // 장비템일경우 (Type == 1) 장착하고
            // 키일경우 (Type == 2) 범위 측정해서 해당키와 문이 일치할경우 
            // 소비템일경우 (Type == 3) 게이지 회복하고
            //
        }

    }


}
