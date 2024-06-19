using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DarkMonsterController : EnemyController
{
    // 플레이어가 바라봤을때 한번만 인식하게 설정
    private bool _isMeet = false;
    //디졸브 상태값
    protected float _desolveSpeed = 0.3f;
    // 다크몬스터가 부유하는 상태값
    private float _floatSpeed = 1.0f; //부유하는 속도
    private float _floatAmplitude = 0.3f; // 부유 높이
    public float _darkMonsterSpeed;
    private float _initialY; //초기 y값

    //다크몬스터 주변에있는 해골들
    private Transform _skulPanel;
    private List<Light> _skulLights;

    //다크몬스터의 이동을 제한하는 촛불들을 관리하는 리스트
    //촛불의 처음맵에 off된 상태를 저장
    private List<CandleScript> candles;
    private Dictionary<CandleScript, bool> candlePrevStates;
    private int _count;

    override protected void Awake()
    {
        base.Awake();
        //해골 오브젝트
        _skulPanel = transform.GetChild(1);
        _skulLights = _skulPanel.GetComponentsInChildren<Light>().ToList();
    }

    override protected void Start()
    {
        base.Start();


        _initialY = transform.position.y;
        _darkMonsterSpeed = _characterData.WalkSpeed;


        // 딕셔너리를 촛불들의 초기 상태로 초기화
        candles = new List<CandleScript>(FindObjectsOfType<CandleScript>()); 
        candlePrevStates = new Dictionary<CandleScript, bool>();
        foreach (CandleScript candle in candles)
        {
            candlePrevStates[candle] = candle.GetLit();
        }
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
        Debug.Log(_enemyState);
        if (_isMeet)
            return;
        // 적 공격시 상태값 리턴
        if (_enemyState == EnemyState.Attack)
            return; 
        if (_enemyState == EnemyState.Die)
            return;
        if (_enemyState == EnemyState.Stop)
            return;
        // 촛불안에 들어오면 none상태 유지
        //근데 이러면 계속 안에 들어온 상태 아닐까?
        if (IsWithinAnyCandleLight())
        {
            SetState((int)EnemyState.Die);
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
                if(playerController.GetIsFlashLight() == true)
                {
                    if(!_isMeet)
                    {
                        _isMeet = true;
                        SetState(5); //주금
                    }
                }
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
                    StopCoroutine("DisolveEffect");
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
                    ClosestFire();
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

   

    private void SkulRotate()
    {
        float rotZ = 20.0f;
        _skulPanel.Rotate(0, 0, rotZ * Time.deltaTime);

        foreach (Transform child in _skulPanel)
        {
            child.Rotate(0, 0, -rotZ * Time.deltaTime);
        }

    }

    private void SpeedIncresePerGetSkul()
    {
        //촛불퀘스트 촛불킬때마다 스피드++;

        foreach (CandleScript candle in candles)
        {
            bool previousState = candlePrevStates[candle];
            bool currentState = candle.GetLit();

            // 촛불이 이전에 꺼져있었고 현재 켜져있다면 속도 증가
            if (!previousState && currentState)
            {
                _darkMonsterSpeed += 1.0f; // 촛불 하나당 속도를 1 증가
            }

            int candleIndex = candles.IndexOf(candle);

            if (candleIndex >= 0 && candleIndex < _skulLights.Count)
            {
                Light skulLight = _skulLights[candleIndex];
                skulLight.intensity = currentState ? 1.0f : 0.0f; // 촛불이 켜져 있으면 강도 1, 꺼져 있으면 강도 0
            }
            // 현재 상태를 딕셔너리에 업데이트
            candlePrevStates[candle] = currentState;
        }


        //해골 불 켜주기


    }
        private bool IsWithinAnyCandleLight()
    {
        if(candles.Count == 0) 
            return false;

        foreach (CandleScript candle in candles)
        {
            if (candle.IsWithinLight(transform.position))
                return true;
        }
        return false;
    }

    private void ClosestFire()
    {
        // 촛불 주변을 피하도록 이동 경로 계산
        StartCoroutine(DisolveEffect());
        SetState((int)EnemyState.None);
    }

    

    public IEnumerator DisolveEffect()
    {

        Renderer[] _renderers = transform.GetComponentsInChildren<Renderer>();

        float _time = 0.0f;

        while (_time < 2.0f)
        {
            _time += _desolveSpeed * Time.deltaTime;

            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_DesolveTime", _time);
                renderer.material.SetColor("DesolveColor", Color.white);
            }

            yield return null;
        }
        if (_time > 2.0f)
        {
            foreach (Renderer renderer in _renderers)
            {
                //path의 랜덤한 장소에서 태어나게 해주고 디졸브 초기값과 색상 초기값으로 설정
                //
                renderer.material.SetFloat("_DesolveTime", 0.0f);
                renderer.material.SetColor("DesolveColor", Color.red);

            }

            int random = Random.Range(0, 4);
            transform.position = pathes[random];
            SetState((int)EnemyState.Trace);
            _isMeet = false;

        }
    }
}
