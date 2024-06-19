using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MoveableCharactorController
{
    //�÷��̾ ��������� ������ ������ �Ἥ 
     

    private Vector2 _mouseValue;

    private float _curStemina = 0.0f;
    private float _steminaDrainRate = 10.0f;

    [SerializeField]
    private GameObject _flash;
    [SerializeField]
    private GameObject _phone;

    /// ///////////////////////////////////////////////////////////////////////////////////////
    /// ///////////////////////////////////////////////////////////////////////////////////////
    // �÷��̾��� ���� �������ִ� �κ�

    //Hide
    private bool _isPlayerHide = false;
    public bool GetIsPlayerHide() { return _isPlayerHide; }

    // �÷��̾� ��Ʈ���� ����
    private bool _isPlayerVant = false;
    public void SetPlayerInVant(bool _isVant) { _isPlayerVant = _isVant; }
    public bool GetIsPlayerVant() { return _isPlayerVant; }
    //LightOn
    private bool _isLightOn = false;
    private bool _isFlashLight = false;
    public bool GetIsFlashLight() { return _isFlashLight; }
    //�ڵ���
    private bool _isPhoneOn = false;
    public bool GetIsPhoneOn() { return _isPhoneOn; }

    //�÷��̾� ��������
    private bool _isDie = false;
    public bool GetIsPlayerDie() { return _isDie; }

    //�÷��̾� �̵�����
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
    public GameObject GetHand() { return _hand; }


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
        {
            _hand.SetActive(false);
            return;
        }

        base.Update();

        if (SceneManager.GetActiveScene().name == "VentScene")
        {
            _isPlayerVant = true;
        }
        Light _flashLight = _flash.GetComponentInChildren<Flashlight>().lightSource;
        _isFlashLight = _flashLight.enabled;


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
            // ���� �ѹ��� ȣ���ؼ� ����
            // SoundManager.Instance.Play3D("PlayerStep", transform, false);
        }

        // ���¹̳� ����
        _curSteminaBar.fillAmount = _curStemina / _characterData.Stemina;
        _curStemina = Mathf.Clamp(_curStemina, 0, _characterData.Stemina);
        // �̵� ���� ����
        _velocity.x = Input.GetAxis("Horizontal");
        _velocity.z = Input.GetAxis("Vertical");

        if (SceneManager.GetActiveScene().name == "VentScene")
        {
            _moveSpeed = _characterData.CrawlingSpeed;
        }
        else
        {

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (_curStemina > 0.0f)
                {
                    _isPlayerVant = false;
                    _curStemina -= Time.deltaTime * _steminaDrainRate; // ���׹̳� ����
                    _moveSpeed = _characterData.RunSpeed;
                    //pitch  �����ؼ� ������� �ϳ� �׳� ������ �����

                }
                else
                {
                    _moveSpeed = _characterData.WalkSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.C))
            {
                _isPlayerVant = true;
                _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // ���׹̳� ����
                _moveSpeed = _characterData.CrawlingSpeed;

            }
            else
            {
                _isPlayerVant = false;
                _curStemina += Time.deltaTime * 0.5f * _steminaDrainRate; // ���׹̳� ����
                _moveSpeed = _characterData.WalkSpeed;
            }

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

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(_isLightOn)
            {
                _isLightOn = false;
                Animator animator = _hand.GetComponent<Animator>();
                animator.SetBool("IsFlash", _isLightOn);
                _flash.SetActive(_isLightOn);
                _hand.SetActive(_isLightOn);
                ItemData data = DataManager.Instance.GetItemData(101);
                UIManager.Instance.GetInventory.AddItem(data);
                
            }
            if(_isPhoneOn)
            {
                _isPhoneOn = false;

                _phone.SetActive(_isPhoneOn);
                Animator animator = _hand.GetComponent<Animator>();
                animator.SetBool("IsPhone", _isPhoneOn);

                _hand.SetActive(_isPhoneOn);
                ItemData data = DataManager.Instance.GetItemData(102);
                UIManager.Instance.GetInventory.AddItem(data);
                
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            _isDie = true;
            _hand.SetActive(false);
            //������� ȣ�� �ߵȴ�.
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
