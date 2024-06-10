using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkMonsterController : EnemyController
{
    private float _floatSpeed = 1.0f; //부유하는 속도
    private float _floatAmplitude = 0.3f; // 부유 높이
    private float _darkMonsterSpeed;
    private float _initialY; //초기 y값

    private Transform _skulPanel;


    
    override protected void Start()
    {
        base.Start();
        _initialY = transform.position.y;
        _darkMonsterSpeed = _characterData.WalkSpeed;
        _skulPanel = transform.GetChild(0);
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
        //플레이어가 숨으면 순찰
        bool _playerHide = _target.GetComponent<PlayerController>().GetIsPlayerHide();
        if (_playerHide)
        {
            SetState(0);
            return;
        }


        // 플레이어가 쳐다보고 손전등 켰을때
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
                _animator.speed = 1.0f;
                _navigation.SetDestination(_target.position);
                _navigation.speed = _darkMonsterSpeed;
                break;
            case EnemyState.Attack:
                _navigation.speed = 0;
                _isAttack = true;
                break;
            case EnemyState.Die:
                _navigation.velocity = Vector3.zero;
                _navigation.speed = 0;
                DesolveEffect();
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
        // 촛불퀘스트 촛불킬때마다 스피드++;
    }

    private void SkulRotate()
    {
        float rotZ = 20.0f;
        _skulPanel.Rotate(0,0,rotZ*Time.deltaTime);

        foreach (Transform child in _skulPanel)
        {
            child.Rotate(0, 0, -rotZ * Time.deltaTime);
        }

    }
}
