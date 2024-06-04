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
        // 스태미너 관리
        _curSteminaBar.fillAmount = _curStemina / _characterData.Stemina;
        _curStemina = Mathf.Clamp(_curStemina, 0, _characterData.Stemina);
        // 이동 조작 관리
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift) && _characterData.Stemina > 0)
        {
            _curStemina -= Time.deltaTime * _steminaDrainRate; // 스테미너 감소
            _moveSpeed = _characterData.RunSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) )
        {
            _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // 스테미너 증가
            _moveSpeed = _characterData.CrawlingSpeed;            
        }
        else
        {
            _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // 스테미너 증가
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
            // 아이템 상호작용 이떄의 아이템 코드에 따라서 행동이 달라지게 
            // 장비템일경우 (Type == 1) 장착하고
            // 키일경우 (Type == 2) 범위 측정해서 해당키와 문이 일치할경우 
            // 소비템일경우 (Type == 3) 게이지 회복하고
            //
        }

    }



}
