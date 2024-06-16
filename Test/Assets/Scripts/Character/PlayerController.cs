using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class PlayerController : MoveableCharactorController
{
    //플레이어가 손전등가지고 있으니 옵저버 써서 
     

    private Vector2 _mouseValue;

    private float _curStemina = 0.0f;
    private float _steminaDrainRate = 10.0f;

    [SerializeField]
    private GameObject _flash;
    [SerializeField]
    private GameObject _phone;

    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////
    // 플레이어의 정보 추출해주는 부분

    //Hide
    private bool _isPlayerHide = false;
    public bool GetIsPlayerHide() { return _isPlayerHide; }

    // 플레이어 벤트들어간지 여부
    private bool _isPlayerVant = false;
    public void SetPlayerInVant(bool _isVant) { _isPlayerVant = _isVant; }
    public bool GetIsPlayerVant() { return _isPlayerVant; }
    //LightOn
    private bool _isLightOn = false;
    public bool GetIsLightOn() { return _isLightOn; }
    //핸드폰
    private bool _isPhoneOn = false;
    public bool GetIsPhoneOn() { return _isPhoneOn; }

    //플레이어 죽음상태
    private bool _isDie = false;
    public bool GetIsPlayerDie() { return _isDie; }

    //플레이어 이동상태
    private bool _isMove = false;
    public bool GetIsPlayerMove() { return _isMove; }

    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////

    private Transform _rotateObj;
    private GameObject _prfSteminaBar;
    private Image _curSteminaBar;
    
    public Texture2D cursorTexture;
    private Vector2 hotspot = Vector2.zero;

    private GameObject _hand;


    protected override void Awake()
    {
        base.Awake();
        Vector3 pos = new Vector3(960, 540,0);
        _rotateObj = transform.GetChild(0);

        //cursorTexture = Resources.Load("Textures/UI/Cursur")as Texture2D;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, Camera.main.WorldToScreenPoint(pos) , CursorMode.ForceSoftware);
        Cursor.lockState = CursorLockMode.Locked;


        _hand = transform.GetChild(0).transform.GetChild(1).gameObject;
        Observer.OnEvents.Add(302, UseDrink);
        Observer.OnNoneEvents.Add(101, UseFlash);
        Observer.OnNoneEvents.Add(102, UsePhone);
    }

    private void Start()
    {
        _prfSteminaBar = GameObject.Find("SteminaBar");
        _curSteminaBar = _prfSteminaBar.transform.GetChild(0).GetComponent<Image>();
        _curStemina = _characterData.Stemina;


    }

    protected override void Update()
    {
        if (_isDie)
            return;

        base.Update();

        MoveController();
        RotateController();

        ItemUseController();
    }

    private void MoveController()
    {
        if(_velocity.magnitude==0)
        {
            _isMove = false;
        }
        else
        {
            _isMove = true;
        }

        // 스태미너 관리
        _curSteminaBar.fillAmount = _curStemina / _characterData.Stemina;
        _curStemina = Mathf.Clamp(_curStemina, 0, _characterData.Stemina);
        // 이동 조작 관리
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if(_curStemina > 0.0f)
            {
                _isPlayerVant = false;
                _curStemina -= Time.deltaTime * _steminaDrainRate; // 스테미너 감소
                _moveSpeed = _characterData.RunSpeed; 
                
            }
            else
            {
                _moveSpeed = _characterData.WalkSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.C) )
        {
            _isPlayerVant = true;
            _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // 스테미너 증가
            _moveSpeed = _characterData.CrawlingSpeed;
        }
        else
        {
            _isPlayerVant = false;
            _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // 스테미너 증가
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

        rotX = Mathf.Clamp(rotX, -50.0f, 50.0f);        
        _rotateObj.localRotation = Quaternion.Euler(rotX, 0, 0);


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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            _isDie = true;
            //닿았을때 호출 잘된다.
        }

    }

    public void UseDrink(float value)
    {
        _curStemina += value;

    }


    private void UseFlash()
    {

        _isLightOn = !_isLightOn;

        _hand.SetActive(_isLightOn);
        
        //GameObject _phone = _hand.transform.Find("PlayerIPHONE").gameObject;
        _flash.SetActive(_isLightOn);
        //_phone.SetActive(false);
        Animator animator = _hand.GetComponent<Animator>();
        animator.SetBool("IsFlash", _isLightOn);

    }
    private void UsePhone()
    {
        _isPhoneOn = !_isPhoneOn;

        _hand.SetActive(_isPhoneOn);
        //GameObject _flash = _hand.transform.Find("Flashlight").gameObject;
        
        //_flash.SetActive(false);
        _phone.SetActive(_isPhoneOn);
        Animator animator = _hand.GetComponent<Animator>();
        animator.SetBool("IsPhone", _isPhoneOn);

    }


}
