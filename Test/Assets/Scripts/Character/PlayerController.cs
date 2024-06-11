using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class PlayerController : MoveableCharactorController
{

    private Vector2 _mouseValue;

    private float _curStemina = 0.0f;
    private float _steminaDrainRate = 10.0f;

    //Hide
    private bool _isPlayerHide;
    public bool GetIsPlayerHide() { return _isPlayerHide; }

    //LightOn
    private bool _isLightOn = false;
    public bool GetIsLightOn() { return _isLightOn; }

    private Transform _rotateObj;
    private GameObject _prfSteminaBar;
    private Image _curSteminaBar;
    
    public Texture2D cursorTexture;
    private Vector2 hotspot = Vector2.zero;

    protected override void Awake()
    {
        base.Awake();
        Vector3 pos = new Vector3(960, 540,0);
        _rotateObj = transform.GetChild(0);

        //cursorTexture = Resources.Load("Textures/UI/Cursur")as Texture2D;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, Camera.main.WorldToScreenPoint(pos) , CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _prfSteminaBar = GameObject.Find("SteminaBar");
        _curSteminaBar = _prfSteminaBar.transform.GetChild(0).GetComponent<Image>();
        _curStemina = _characterData.Stemina;
    }

    protected override void Update()
    {
        base.Update();

        MoveController();
        RotateController();

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(_curStemina > 0.0f)
            {
                _curStemina -= Time.deltaTime * _steminaDrainRate; // ���׹̳� ����
                _moveSpeed = _characterData.RunSpeed;
            }
            else
            {
                _moveSpeed = _characterData.WalkSpeed;
            }
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
        
        _mouseValue.x = Input.GetAxis("Mouse X");
        _mouseValue.y = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * _characterData.RotateSpeed * _mouseValue.x * Time.deltaTime);
        _rotateObj.Rotate(Vector3.left * _characterData.RotateSpeed * _mouseValue.y * Time.deltaTime);
                
        float rotX = _rotateObj.localEulerAngles.x;

        if( rotX > 180.0f)
        {
            rotX -= 360.0f;
        }

        rotX = Mathf.Clamp(rotX, -30.0f, 30.0f);        
        _rotateObj.localRotation = Quaternion.Euler(rotX, 0, 0);


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
