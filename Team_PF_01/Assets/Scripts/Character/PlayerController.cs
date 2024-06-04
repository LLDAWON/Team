using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MoveableCharactorController
{
    public float _rotateSpeed = 50.0f;

    private bool _isJump = false;
    private Vector2 _mouseValue;

    private float _steminaDrainRate = 10.0f;

    private Transform _rotateObj;

    protected override void Awake()
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

        //
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && _characterData.Stemina > 0)
        {
            DecreseStemina();
            _moveSpeed = _characterData.RunSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) )
        {
            IncreseStemina();
            _moveSpeed = _characterData.CrawlingSpeed;            
        }
        else
        {
            IncreseStemina();
            _moveSpeed = _characterData.WalkSpeed;            
        }

        _velocity *= _moveSpeed;        
    }

    private void RotateController()
    {
        Cursor.visible= false;
        Cursor.lockState = CursorLockMode.Locked;
        _mouseValue.x = Input.GetAxis("Mouse X");
        _mouseValue.y = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * _rotateSpeed * _mouseValue.x * Time.deltaTime);
        _rotateObj.Rotate(Vector3.left * _rotateSpeed * _mouseValue.y * Time.deltaTime);
                
        float rotX = _rotateObj.localEulerAngles.x;

        if( rotX > 180.0f)
        {
            rotX -= 360.0f;
        }

        rotX = Mathf.Clamp(rotX, -30.0f, 30.0f);        
        _rotateObj.localRotation = Quaternion.Euler(rotX, 0, 0);


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
            // ������ ��ȣ�ۿ� �̋��� ������ �ڵ忡 ���� �ൿ�� �޶����� 
            // ������ϰ�� (Type == 1) �����ϰ�
            // Ű�ϰ�� (Type == 2) ���� �����ؼ� �ش�Ű�� ���� ��ġ�Ұ�� 
            // �Һ����ϰ�� (Type == 3) ������ ȸ���ϰ�
            //
        }

    }

    private void DecreseStemina()
    {
        _characterData.Stemina -= Time.deltaTime * _steminaDrainRate; // ���׹̳� ����
        _characterData.Stemina = Mathf.Clamp(_characterData.Stemina, 0, _characterData.Stemina);
    }
    private void IncreseStemina()
    {
        _characterData.Stemina += Time.deltaTime*0.5f * _steminaDrainRate; // ���׹̳� ����
        _characterData.Stemina = Mathf.Clamp(_characterData.Stemina, 0, _characterData.Stemina);
    }


}
