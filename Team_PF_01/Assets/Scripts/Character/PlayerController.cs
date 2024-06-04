using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MoveableCharactorController
{
    public float _rotateSpeed = 50.0f;
    private float _curStemina = 0.0f;

    private bool _isJump = false;
    private Vector2 _mouseValue;

    private float _steminaDrainRate = 10.0f;

    private Transform _rotateObj;
    private GameObject _prfSteminaBar;
    private Image _curSteminaBar;

    protected override void Awake()
    {
        base.Awake();

        _rotateObj = transform.GetChild(0);


    }

    private void Start()
    {
        _prfSteminaBar = GameObject.Find("SteminaBar");
        _curSteminaBar = _prfSteminaBar.transform.GetChild(0).GetComponent<Image>();
        _curStemina = _characterData.Stemina;
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
        // ���¹̳� ����
        _curSteminaBar.fillAmount = _curStemina / _characterData.Stemina;
        _curStemina = Mathf.Clamp(_curStemina, 0, _characterData.Stemina);
        // �̵� ���� ����
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && _characterData.Stemina > 0)
        {
            _curStemina -= Time.deltaTime * _steminaDrainRate; // ���׹̳� ����
            _moveSpeed = _characterData.RunSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) )
        {
            _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // ���׹̳� ����
            _moveSpeed = _characterData.CrawlingSpeed;            
        }
        else
        {
            _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // ���׹̳� ����
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



}
