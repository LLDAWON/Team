using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMonsterController : EnemyController
{
    // ��ũ���Ͱ� �����ϴ� ���°�
    private float _floatSpeed = 1.0f; //�����ϴ� �ӵ�
    private float _floatAmplitude = 0.3f; // ���� ����
    private float _darkMonsterSpeed;
    private float _initialY; //�ʱ� y��

    //��ũ���� �ֺ����ִ� �ذ��
    private Transform _skulPanel;
    //private Renderer[] _renderers;

    //��ũ������ �̵��� �����ϴ� �кҵ��� �����ϴ� ����Ʈ
    private List<CandleScript> candles;

    //
    //private float _desolveSpeed = 0.3f; 

    protected override void Awake()
    {
        base.Awake();

        //_renderers = GetComponentsInChildren<Renderer>();
    }

    override protected void Start()
    {
        base.Start();
        _initialY = transform.position.y;
        _darkMonsterSpeed = _characterData.WalkSpeed;
        _skulPanel = transform.GetChild(0);
        candles = new List<CandleScript>(FindObjectsOfType<CandleScript>());
    }
    override protected void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    StartCoroutine(DisolveEffect());
        //}

        SkulRotate();
        SpeedIncresePerGetSkul();
        FloatingAir();
        base.Update();
    }

    //IEnumerator DisolveEffect()
    //{
    //    float disolveTime = 0.0f;

    //    while(disolveTime < 2.0f)
    //    {
    //        disolveTime += _desolveSpeedTime.deltaTime;

    //        foreach(Renderer renderer in _renderers)
    //        {
    //            renderer.material.SetFloat("_DesolveTime", disolveTime);
    //        }

    //        yield return null;
    //    }
    //}

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
        //�÷��̾��� ��ä�� Ž��
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


    protected override void EnemyAiPattern()
    {

        if (IsWithinAnyCandleLight())
        {
            AvoidCandles();
            return;
        }

        switch (_enemyState)
        {
            case EnemyState.Trace:
                {
                    _animator.speed = 1.0f;
                    _navigation.SetDestination(_target.position);
                    _navigation.speed = _darkMonsterSpeed;
                }
                break;
            case EnemyState.Attack:
                {
                    _navigation.speed = 0;
                    _isAttack = true;
                }
               
                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("����");
                    DestroyMonster();
                }
                break;
        }
    }

    private void FloatingAir()
    {

        Vector3 pos = transform.position;
        pos.y = _initialY + Mathf.Sin(Time.time * _floatSpeed) * _floatAmplitude;
        transform.position = pos;

    }

    private void SpeedIncresePerGetSkul()
    {
        // �к�����Ʈ �к�ų������ ���ǵ�++;
    }

    private void SkulRotate()
    {
        float rotZ = 20.0f;
        _skulPanel.Rotate(0, 0, rotZ * Time.deltaTime);

        foreach (Transform child in _skulPanel)
        {
            child.Rotate(0, 0, -rotZ * Time.deltaTime);
        }

    }

    private bool IsWithinAnyCandleLight()
    {
        foreach (var candle in candles)
        {
            if (candle.IsWithinLight(transform.position))
                return true;
        }
        return false;
    }

    private void AvoidCandles()
    {
        // �к� �ֺ��� ���ϵ��� �̵� ��� ���
        Vector3 avoidDirection = Vector3.zero;
        foreach (var candle in candles)
        {
            if (candle.IsWithinLight(transform.position))
            {
                avoidDirection += (transform.position - candle.transform.position);
            }
        }
        avoidDirection.Normalize();
        Vector3 newTargetPosition = transform.position + avoidDirection * _darkMonsterSpeed * Time.deltaTime;
        _navigation.SetDestination(newTargetPosition);
    }
}
