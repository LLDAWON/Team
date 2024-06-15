using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMonsterController : EnemyController
{
    // 다크몬스터가 부유하는 상태값
    private float _floatSpeed = 1.0f; //부유하는 속도
    private float _floatAmplitude = 0.3f; // 부유 높이
    private float _darkMonsterSpeed;
    private float _initialY; //초기 y값

    //다크몬스터 주변에있는 해골들
    private Transform _skulPanel;

    //다크몬스터의 이동을 제한하는 촛불들을 관리하는 리스트
    private List<CandleScript> candles;

    //

    protected override void Awake()
    {
        base.Awake();

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

        SkulRotate();
        SpeedIncresePerGetSkul();
        FloatingAir();
        base.Update();
    }


    override protected void StateUpdate()
    {
        // 적 공격시 상태값 리턴
        if (_enemyState == EnemyState.Attack)
            return;
        // 촛불안에 들어오면 none상태 유지
        //근데 이러면 계속 안에 들어온 상태 아닐까?
        if (IsWithinAnyCandleLight())
        {
            SetState(3);
            return;
        }
        //플레이어가 숨으면 순찰
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }


        PlayerController playerController = _target.GetComponent<PlayerController>();
        //플레이어의 부채꼴 탐색
        Vector3 _inPlayerSight = transform.position - _target.transform.position;
        _inPlayerSight.y = 0;

        float _detectRange = playerController.GetCharacterData().DetectRange  ;

        //감지범위안에 들어왔을때
        if (_inPlayerSight.magnitude <= _detectRange )
        {
            float dot = Vector3.Dot(_inPlayerSight.normalized, playerController.transform.forward);

            float theta = Mathf.Acos(dot);

            float degree = Mathf.Rad2Deg * theta;

            //부채꼴 안에 들어왔을때
            if (degree <= _angleRange)
            {
                //불켰을때
                if(playerController.GetIsLightOn() == true)
                    SetState(5); //주금
                else
                    SetState(1); // 추적
            }
            //부채꼴안에 안들어왔을때
            else
                SetState(1); //추적
        }
        // 감지범위 밖일때도 추적해오자
        else
        {
            SetState(1);
        }


    }


    protected override void EnemyAiPattern()
    {

       

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
                    _navigation.velocity = Vector3.zero;
                    _animator.speed = 1.0f;
                    _isAttack = true;
                    _animator.SetTrigger("Attack");
                    Observer.OnTargetEvents[1](gameObject);
                }
               
                break;
            case EnemyState.Die:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("죽음");
                }
                break;
            case EnemyState.Stop:
                {
                    _navigation.velocity = Vector3.zero;
                    _navigation.speed = 0;
                    Debug.Log("멈춤");
                }
                break;
        }
        Debug.Log(_enemyState);
    }

    private void FloatingAir()
    {

        Vector3 pos = transform.position;
        pos.y = _initialY + Mathf.Sin(Time.time * _floatSpeed) * _floatAmplitude;
        transform.position = pos;
        Debug.Log("부유");

    }

    private void SpeedIncresePerGetSkul()
    {
        // 촛불퀘스트 촛불킬때마다 스피드++;
        //if()
        //{
        //    _darkMonsterSpeed++;
        //}

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
        if(candles.Count == 0) 
            return false;

        foreach (var candle in candles)
        {
            if (candle.IsWithinLight(transform.position))
                return true;
        }
        return false;
    }

    private void AvoidCandles()
    {
        // 촛불 주변을 피하도록 이동 경로 계산
        
    }

    private void EndAttack()
    {
        StartCoroutine(Observer.OnDesolveEvents[1](gameObject));
        //Observer.OnDesolveEvents[1](2.0f);
    }
}
